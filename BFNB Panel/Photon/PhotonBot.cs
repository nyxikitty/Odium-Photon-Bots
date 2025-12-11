using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BLF_Odium_Network_Bots.Photon;

namespace OdiumPhoton.Core
{
    /// <summary>
    /// Automated Photon client for testing and load simulation
    /// </summary>
    public class PhotonBot : LoadBalancingClient
    {
        #region Constants
        private const int TICK_RATE = 10;
        private const int SEND_RATE = 50;
        private const int MOVEMENT_UPDATE_MS = 200;

        private const byte RPC_EVENT = 200;
        private const byte MOVEMENT_EVENT = 201;
        private const byte SPAWN_EVENT = 202;
        private const byte ACTOR_EVENT = 207;

        private const float DEFAULT_HEIGHT = 1.75f;
        private const float ORBIT_SPEED = 0.08f;
        private const float FOLLOW_SPEED = 0.05f;
        private const float IDLE_SPEED = 0.05f;
        #endregion

        #region Fields
        private readonly Thread _networkThread;
        private readonly Thread _updateThread;
        private readonly Dictionary<int, Vec3> _playerPositions;
        private readonly object _positionLock = new object();

        private int _lastTick;
        private int _lastSend;
        private int _viewID;
        private float _rotationAngle;

        private volatile bool _hasSpawned;
        private volatile bool _isActive;

        private RPCs _rpcs;

        public string BotName { get; set; }
        public string ClanTag { get; set; }
        public byte Team { get; set; }
        public byte Rank { get; set; }
        public MovementMode CurrentMode { get; private set; }
        public int TargetActor { get; private set; }
        public float OrbitOffset { get; private set; }
        #endregion

        #region Properties
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        /// <summary>Access to all RPC methods</summary>
        public RPCs RPC => _rpcs;

        public enum MovementMode
        {
            Idle,
            Circle,
            Follow,
            Orbit
        }
        #endregion

        public PhotonBot(string appId, string appVersion)
        {
            CustomTypes.Register();

            BotName = GenerateBotName();
            ClanTag = "[BOT]";
            Team = 1;
            Rank = 200;

            _playerPositions = new Dictionary<int, Vec3>();
            CurrentMode = MovementMode.Circle;

            AppId = appId;
            AppVersion = appVersion;
            NameServerHost = "ns.exitgames.com";

            _networkThread = new Thread(NetworkLoop) { IsBackground = true };
            _updateThread = new Thread(UpdateLoop) { IsBackground = true };

            this.AddCallbackTarget(this);
            this.EventReceived += OnEventReceived;
            this.StateChanged += OnStateChange;

            _networkThread.Start();
            _updateThread.Start();

            ConnectToRegionMaster("us");
        }

        #region Core Loops
        private void NetworkLoop()
        {
            while (true)
            {
                int now = Environment.TickCount;

                if (now - _lastTick > TICK_RATE)
                {
                    _lastTick = now;
                    LoadBalancingPeer?.DispatchIncomingCommands();
                }

                if (now - _lastSend > SEND_RATE)
                {
                    _lastSend = now;
                    LoadBalancingPeer?.SendOutgoingCommands();
                }

                Thread.Sleep(TICK_RATE);
            }
        }

        private void UpdateLoop()
        {
            while (true)
            {
                if (_hasSpawned && _viewID > 0)
                {
                    if (IsFloorColliderRemovalActive)
                    {
                        // Send corrupted movement to remove floor collision
                        BroadcastCorruptedMovement();
                    }
                    else if (_isActive)
                    {
                        // Normal movement
                        BroadcastMovement();
                    }

                    // Spam movement packets if enabled
                    if (IsMovementSpamActive)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            BroadcastMovement();
                        }
                    }
                }
                Thread.Sleep(MOVEMENT_UPDATE_MS);
            }
        }
        #endregion

        #region Room Operations
        public bool JoinRandom() => OpJoinRandomRoom();

        public bool JoinRoom(string name) => OpJoinRoom(new EnterRoomParams { RoomName = name });

        public bool CreateRoom(string name, string map, string mode, byte maxPlayers)
        {
            var options = new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = maxPlayers,
                CustomRoomProperties = new Hashtable
                {
                    ["mapName"] = map,
                    ["modeName"] = mode,
                    ["matchStarted"] = false,
                    ["gameVersion"] = AppVersion
                }
            };

            return OpCreateRoom(new EnterRoomParams
            {
                RoomName = name ?? $"BotRoom (#{new Random().Next(1000, 10000)})",
                RoomOptions = options
            });
        }

        public void LeaveRoom() => OpLeaveRoom(false);
        #endregion

        #region Movement Control
        public void SetMovement(MovementMode mode, int targetActor = -1, float offset = 0f)
        {
            CurrentMode = mode;
            TargetActor = targetActor;
            OrbitOffset = offset;
            _isActive = mode != MovementMode.Idle;
        }

        public void StopMovement()
        {
            CurrentMode = MovementMode.Idle;
            _isActive = false;
        }
        #endregion

        #region Spawn Sequence
        private void HandleRoomJoined()
        {
            if (_hasSpawned) return;

            FairCollection.InitOperationAsync();

            Thread.Sleep(100);
            SetupPlayer();
            Thread.Sleep(300);
            PerformSpawn();

            _hasSpawned = true;
        }

        private void SetupPlayer()
        {
            string displayName = string.IsNullOrWhiteSpace(ClanTag)
                ? BotName
                : $"{ClanTag} {BotName}";

            LocalPlayer.NickName = displayName;
            LocalPlayer.SetCustomProperties(new Hashtable
            {
                [(byte)255] = displayName,
                ["teamNumber"] = Team,
                ["rank"] = Rank,
                ["killstreak"] = (byte)0
            });
        }

        private void PerformSpawn()
        {
            _viewID = AllocateViewID();

            _rpcs = new RPCs(this, _viewID);

            RaiseReliable(ACTOR_EVENT, new Hashtable { [(byte)0] = LocalPlayer.ActorNumber });
            Thread.Sleep(100);

            RaiseReliable(SPAWN_EVENT, new Hashtable
            {
                [(byte)0] = "PlayerBody",
                [(byte)6] = LoadBalancingPeer.ServerTimeInMilliSeconds,
                [(byte)7] = _viewID
            }, EventCaching.AddToRoomCache);
            Thread.Sleep(200);

            EquipLoadout();
            Thread.Sleep(200);

            BroadcastMovement(true);
            Thread.Sleep(1000);

            _isActive = true;
        }

        private void EquipLoadout()
        {
            _rpcs.LatencySend();
            Thread.Sleep(50);
            _rpcs.WeaponTypeChanged(14);
            Thread.Sleep(50);
            _rpcs.WeaponCamoChanged(0);
            Thread.Sleep(50);
            _rpcs.SetRank(Rank);
            Thread.Sleep(50);
            _rpcs.UpdateTeamNumber(Team);
        }

        private int AllocateViewID()
        {
            return LocalPlayer.ActorNumber * 1000 + 1;
        }
        #endregion

        #region Movement Broadcasting
        private void BroadcastMovement(bool reliable = false)
        {
            Vec3 position = CalculateNextPosition();
            Vec3 encrypted = FairCollection.GetEncryptedVector3(position);

            var packet = new Hashtable
            {
                [(byte)10] = BuildMovementData(position, encrypted),
                [(byte)0] = LoadBalancingPeer.ServerTimeInMilliSeconds,
                [(byte)1] = (short)0
            };

            var options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            var send = reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable;

            OpRaiseEvent(MOVEMENT_EVENT, packet, options, send);
        }

        /// <summary>Send corrupted movement packet with invalid quaternion</summary>
        public void BroadcastCorruptedMovement()
        {
            if (!_hasSpawned) return;

            Vec3 position = CalculateNextPosition();
            Vec3 encrypted = FairCollection.GetEncryptedVector3(position);

            // Create corrupted quaternion with float.MaxValue to break collision
            var corruptedQuat = new Quat(float.MaxValue, float.MaxValue, float.MaxValue, 1f);

            short x = (short)(position.x * 1000);
            short y = (short)(position.y * 1000);
            short z = (short)(position.z * 1000);

            object[] corruptedMovement = new object[]
            {
                _viewID, false, null,
                (short)40, x, y, (short)0, (short)0, (short)0, z,
                (short)0, (short)0, (short)97, (short)0, (short)0,
                (short)10000, (byte)1, (byte)0, (byte)1, (byte)0, (byte)0,
                999, encrypted, corruptedQuat
            };

            var packet = new Hashtable
            {
                [(byte)10] = corruptedMovement,
                [(byte)0] = LoadBalancingPeer.ServerTimeInMilliSeconds,
                [(byte)1] = (short)0
            };

            OpRaiseEvent(MOVEMENT_EVENT, packet, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        public bool IsFloorColliderRemovalActive { get; set; }
        public bool IsMovementSpamActive { get; set; }

        private Vec3 CalculateNextPosition()
        {
            switch (CurrentMode)
            {
                case MovementMode.Follow:
                    return GetFollowPosition();

                case MovementMode.Orbit:
                    return GetOrbitPosition();

                case MovementMode.Circle:
                default:
                    return GetCirclePosition();
            }
        }

        private Vec3 GetFollowPosition()
        {
            lock (_positionLock)
            {
                if (!_playerPositions.TryGetValue(TargetActor, out Vec3 target))
                    return GetCirclePosition();

                Vec3 current = GetCirclePosition();
                Vec3 direction = Normalize(target - current);
                float distance = Distance(current, target);

                if (distance < 2f)
                    return current;

                return current + (direction * FOLLOW_SPEED * distance);
            }
        }

        private Vec3 GetOrbitPosition()
        {
            lock (_positionLock)
            {
                if (!_playerPositions.TryGetValue(TargetActor, out Vec3 target))
                    return GetCirclePosition();

                _rotationAngle += ORBIT_SPEED;
                float angle = _rotationAngle + OrbitOffset;

                return new Vec3(
                    target.x + (float)Math.Cos(angle) * 3f,
                    target.y,
                    target.z + (float)Math.Sin(angle) * 3f
                );
            }
        }

        private Vec3 GetCirclePosition()
        {
            _rotationAngle += IDLE_SPEED;

            return new Vec3(
                (float)Math.Cos(_rotationAngle) * 5f,
                DEFAULT_HEIGHT,
                (float)Math.Sin(_rotationAngle) * 5f
            );
        }

        private object[] BuildMovementData(Vec3 pos, Vec3 encrypted)
        {
            short x = (short)(pos.x * 1000);
            short y = (short)(pos.y * 1000);
            short z = (short)(pos.z * 1000);

            return new object[]
            {
                _viewID, false, null,
                (short)40, x, y, (short)0, (short)0, (short)0, z,
                (short)0, (short)0, (short)97, (short)0, (short)0,
                (short)10000, (byte)1, (byte)0, (byte)1, (byte)0, (byte)0,
                999, encrypted, new Quat(0f, 0f, 0f, 1f)
            };
        }
        #endregion

        #region Messaging
        /// <summary>Send chat message using RPC system</summary>
        public void SendChat(string message)
        {
            if (!_hasSpawned) return;

            string displayName = string.IsNullOrWhiteSpace(ClanTag)
                ? BotName
                : $"{ClanTag} {BotName}";

            _rpcs.RpcSendChatMessage(displayName, message, 255, 255, 255);
        }

        /// <summary>Send announcement using RPC system</summary>
        public void SendAnnouncement(string text, float duration = 5f)
        {
            if (!_hasSpawned) return;

            _rpcs.ShowAnnouncement(text, duration);
        }
        #endregion

        #region Event Handlers
        private void OnEventReceived(EventData evt)
        {
            if (evt.Code != MOVEMENT_EVENT) return;

            try
            {
                if (evt.CustomData is Hashtable data && data[(byte)10] is object[] movement)
                {
                    if (movement.Length >= 23 && movement[0] is int viewID && movement[22] is Vec3 encrypted)
                    {
                        int actor = viewID / 1000;
                        Vec3 position = FairCollection.GetDecryptedVector3(encrypted);

                        lock (_positionLock)
                        {
                            _playerPositions[actor] = position;
                        }
                    }
                }
            }
            catch { }
        }

        private void OnStateChange(ClientState previous, ClientState current)
        {
            if (current == ClientState.Joined)
                HandleRoomJoined();
            else if (current == ClientState.Disconnected)
                HandleDisconnect();
        }

        private void HandleDisconnect()
        {
            _hasSpawned = false;
            _isActive = false;
            _viewID = 0;
            _rpcs = null;

            lock (_positionLock)
            {
                _playerPositions.Clear();
            }
        }
        #endregion

        #region Utility
        private void RaiseReliable(byte code, Hashtable data, EventCaching cache = EventCaching.DoNotCache)
        {
            var options = cache != EventCaching.DoNotCache
                ? new RaiseEventOptions { CachingOption = cache }
                : RaiseEventOptions.Default;

            OpRaiseEvent(code, data, options, SendOptions.SendReliable);
        }

        private string GenerateBotName() => $"PC-Bot_{new Random().Next(1000, 9999)}";

        private static Vec3 Normalize(Vec3 v)
        {
            float mag = (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            return mag > 0 ? new Vec3(v.x / mag, v.y / mag, v.z / mag) : v;
        }

        private static float Distance(Vec3 a, Vec3 b)
        {
            float dx = b.x - a.x;
            float dy = b.y - a.y;
            float dz = b.z - a.z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public List<Player> GetPlayers()
        {
            return CurrentRoom != null
                ? new List<Player>(CurrentRoom.Players.Values)
                : new List<Player>();
        }

        public new void Disconnect()
        {
            _isActive = false;
            base.Disconnect();
        }
        #endregion
    }
}