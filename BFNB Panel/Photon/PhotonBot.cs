using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BLF_Odium_Network_Bots.Photon;

namespace OdiumPhoton.Core
{
    public class PhotonBot : LoadBalancingClient
    {
        // timing stuff
        private const int TICK_RATE = 10;
        private const int SEND_RATE = 50;
        private const int MOVEMENT_UPDATE_MS = 200;

        // photon event codes
        private const byte RPC_EVENT = 200;
        private const byte MOVEMENT_EVENT = 201;
        private const byte SPAWN_EVENT = 202;
        private const byte ACTOR_EVENT = 207;

        private const float DEFAULT_HEIGHT = 1.75f;
        private const float ORBIT_SPEED = 0.08f;
        private const float FOLLOW_SPEED = 0.05f;
        private const float IDLE_SPEED = 0.05f;

        private Thread networkThread;
        private Thread updateThread;
        private Dictionary<int, Vec3> playerPositions = new Dictionary<int, Vec3>();
        private object posLock = new object();

        private int lastTick = 0;
        private int lastSend = 0;
        private int viewID = 0;
        private float rotAngle = 0f;

        private bool hasSpawned = false;
        private bool isActive = false;

        private RPCs rpcs;

        public string BotName;
        public string ClanTag;
        public byte Team;
        public byte Rank;
        
        private MovementMode currentMode;
        private int targetActor = -1;
        private float orbitOffset = 0f;

        public MovementMode CurrentMode 
        { 
            get { return currentMode; } 
        }
        
        public int TargetActor 
        { 
            get { return targetActor; } 
        }
        
        public float OrbitOffset 
        { 
            get { return orbitOffset; } 
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public RPCs RPC 
        { 
            get { return rpcs; } 
        }

        public bool IsFloorColliderRemovalActive = false;
        public bool IsMovementSpamActive = false;

        public enum MovementMode
        {
            Idle = 0,
            Circle = 1,
            Follow = 2,
            Orbit = 3
        }

        public PhotonBot(string appId, string appVersion)
        {
            CustomTypes.Register();

            this.BotName = GenerateBotName();
            this.ClanTag = "[BOT]";
            this.Team = 1;
            this.Rank = 200;
            this.currentMode = MovementMode.Circle;

            this.AppId = appId;
            this.AppVersion = appVersion;
            this.NameServerHost = "ns.exitgames.com";

            networkThread = new Thread(new ThreadStart(NetworkLoop));
            networkThread.IsBackground = true;
            networkThread.Start();
            
            updateThread = new Thread(new ThreadStart(UpdateLoop));
            updateThread.IsBackground = true;
            updateThread.Start();

            this.AddCallbackTarget(this);
            this.EventReceived += OnEventReceived;
            this.StateChanged += OnStateChange;

            ConnectToRegionMaster("us");
        }

        private void NetworkLoop()
        {
            while (true)
            {
                try
                {
                    int now = Environment.TickCount;

                    if (now - lastTick > TICK_RATE)
                    {
                        lastTick = now;
                        if (LoadBalancingPeer != null)
                            LoadBalancingPeer.DispatchIncomingCommands();
                    }

                    if (now - lastSend > SEND_RATE)
                    {
                        lastSend = now;
                        if (LoadBalancingPeer != null)
                            LoadBalancingPeer.SendOutgoingCommands();
                    }

                    Thread.Sleep(TICK_RATE);
                }
                catch
                {
                    // network loop should never crash
                }
            }
        }

        private void UpdateLoop()
        {
            while (true)
            {
                try
                {
                    if (hasSpawned == true && viewID > 0)
                    {
                        if (IsFloorColliderRemovalActive)
                        {
                            BroadcastCorruptedMovement();
                        }
                        else if (isActive)
                        {
                            BroadcastMovement();
                        }

                        // spam movement packets if enabled
                        if (IsMovementSpamActive)
                        {
                            for (int i = 0; i < 10; i++)
                                BroadcastMovement();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log errors here if needed
                    Console.WriteLine("UpdateLoop error: " + ex.Message);
                }
                
                Thread.Sleep(MOVEMENT_UPDATE_MS);
            }
        }

        public bool JoinRandom()
        {
            return OpJoinRandomRoom();
        }

        public bool JoinRoom(string name)
        {
            EnterRoomParams prms = new EnterRoomParams();
            prms.RoomName = name;
            return OpJoinRoom(prms);
        }

        public bool CreateRoom(string name, string map, string mode, byte maxPlayers)
        {
            RoomOptions opts = new RoomOptions();
            opts.IsVisible = true;
            opts.IsOpen = true;
            opts.MaxPlayers = maxPlayers;
            
            Hashtable roomProps = new Hashtable();
            roomProps.Add("mapName", map);
            roomProps.Add("modeName", mode);
            roomProps.Add("matchStarted", false);
            roomProps.Add("gameVersion", AppVersion);
            opts.CustomRoomProperties = roomProps;

            EnterRoomParams enterParams = new EnterRoomParams();
            if (name == null || name == "")
            {
                Random r = new Random();
                enterParams.RoomName = "BotRoom (#" + r.Next(1000, 10000).ToString() + ")";
            }
            else
            {
                enterParams.RoomName = name;
            }
            enterParams.RoomOptions = opts;

            return OpCreateRoom(enterParams);
        }

        public void LeaveRoom()
        {
            OpLeaveRoom(false);
        }

        public void SetMovement(MovementMode mode, int actor = -1, float offset = 0f)
        {
            this.currentMode = mode;
            this.targetActor = actor;
            this.orbitOffset = offset;
            
            if (mode != MovementMode.Idle)
                this.isActive = true;
            else
                this.isActive = false;
        }

        public void StopMovement()
        {
            currentMode = MovementMode.Idle;
            isActive = false;
        }

        private void HandleRoomJoined()
        {
            if (hasSpawned) 
                return;

            FairCollection.InitOperationAsync();
            Thread.Sleep(100);
            
            SetupPlayer();
            Thread.Sleep(300);
            
            PerformSpawn();
            hasSpawned = true;
        }

        private void SetupPlayer()
        {
            string name = BotName;
            if (!string.IsNullOrWhiteSpace(ClanTag))
            {
                name = ClanTag + " " + BotName;
            }

            LocalPlayer.NickName = name;
            
            Hashtable playerProps = new Hashtable();
            playerProps[(byte)255] = name;
            playerProps["teamNumber"] = Team;
            playerProps["rank"] = Rank;
            playerProps["killstreak"] = (byte)0;
            
            LocalPlayer.SetCustomProperties(playerProps);
        }

        private void PerformSpawn()
        {
            viewID = LocalPlayer.ActorNumber * 1000 + 1;
            rpcs = new RPCs(this, viewID);

            // actor event
            Hashtable actorEvt = new Hashtable();
            actorEvt[(byte)0] = LocalPlayer.ActorNumber;
            SendReliableEvent(ACTOR_EVENT, actorEvt);
            Thread.Sleep(100);

            // spawn event
            Hashtable spawnEvt = new Hashtable();
            spawnEvt[(byte)0] = "PlayerBody";
            spawnEvt[(byte)6] = LoadBalancingPeer.ServerTimeInMilliSeconds;
            spawnEvt[(byte)7] = viewID;
            SendReliableEvent(SPAWN_EVENT, spawnEvt, EventCaching.AddToRoomCache);
            Thread.Sleep(200);

            // equip loadout
            rpcs.LatencySend();
            Thread.Sleep(50);
            rpcs.WeaponTypeChanged(14);
            Thread.Sleep(50);
            rpcs.WeaponCamoChanged(0);
            Thread.Sleep(50);
            rpcs.SetRank(Rank);
            Thread.Sleep(50);
            rpcs.UpdateTeamNumber(Team);
            Thread.Sleep(200);

            BroadcastMovement(true);
            Thread.Sleep(1000);

            isActive = true;
        }

        private void BroadcastMovement(bool reliable = false)
        {
            Vec3 pos = CalculateNextPosition();
            Vec3 enc = FairCollection.GetEncryptedVector3(pos);

            Hashtable pkt = new Hashtable();
            pkt[(byte)10] = BuildMovementData(pos, enc);
            pkt[(byte)0] = LoadBalancingPeer.ServerTimeInMilliSeconds;
            pkt[(byte)1] = (short)0;

            RaiseEventOptions opts = new RaiseEventOptions();
            opts.Receivers = ReceiverGroup.Others;
            
            SendOptions sendOpt = SendOptions.SendUnreliable;
            if (reliable)
                sendOpt = SendOptions.SendReliable;

            OpRaiseEvent(MOVEMENT_EVENT, pkt, opts, sendOpt);
        }

        // corrupted movement packet to trigger floor collider bug
        public void BroadcastCorruptedMovement()
        {
            if (!hasSpawned) 
                return;

            Vec3 pos = CalculateNextPosition();
            Vec3 enc = FairCollection.GetEncryptedVector3(pos);

            Quat badQuat = new Quat(float.MaxValue, float.MaxValue, float.MaxValue, 1f);

            short px = (short)(pos.x * 1000);
            short py = (short)(pos.y * 1000);
            short pz = (short)(pos.z * 1000);

            object[] movData = new object[24];
            movData[0] = viewID;
            movData[1] = false;
            movData[2] = null;
            movData[3] = (short)40;
            movData[4] = px;
            movData[5] = py;
            movData[6] = (short)0;
            movData[7] = (short)0;
            movData[8] = (short)0;
            movData[9] = pz;
            movData[10] = (short)0;
            movData[11] = (short)0;
            movData[12] = (short)97;
            movData[13] = (short)0;
            movData[14] = (short)0;
            movData[15] = (short)10000;
            movData[16] = (byte)1;
            movData[17] = (byte)0;
            movData[18] = (byte)1;
            movData[19] = (byte)0;
            movData[20] = (byte)0;
            movData[21] = 999;
            movData[22] = enc;
            movData[23] = badQuat;

            Hashtable pkt = new Hashtable();
            pkt[(byte)10] = movData;
            pkt[(byte)0] = LoadBalancingPeer.ServerTimeInMilliSeconds;
            pkt[(byte)1] = (short)0;

            OpRaiseEvent(MOVEMENT_EVENT, pkt, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private Vec3 CalculateNextPosition()
        {
            if (currentMode == MovementMode.Follow)
            {
                return GetFollowPosition();
            }
            else if (currentMode == MovementMode.Orbit)
            {
                return GetOrbitPosition();
            }
            else
            {
                return GetCirclePosition();
            }
        }

        private Vec3 GetFollowPosition()
        {
            Vec3 targetPos;
            
            lock (posLock)
            {
                if (!playerPositions.ContainsKey(targetActor))
                    return GetCirclePosition();
                    
                targetPos = playerPositions[targetActor];
            }

            Vec3 currentPos = GetCirclePosition();
            float dist = GetDistance(currentPos, targetPos);

            if (dist < 2f)
                return currentPos;

            Vec3 dir = NormalizeVector(targetPos - currentPos);
            float moveAmount = FOLLOW_SPEED * dist;
            
            return new Vec3(
                currentPos.x + dir.x * moveAmount,
                currentPos.y + dir.y * moveAmount,
                currentPos.z + dir.z * moveAmount
            );
        }

        private Vec3 GetOrbitPosition()
        {
            Vec3 center;
            
            lock (posLock)
            {
                if (!playerPositions.ContainsKey(targetActor))
                    return GetCirclePosition();
                    
                center = playerPositions[targetActor];
            }

            rotAngle = rotAngle + ORBIT_SPEED;
            float ang = rotAngle + orbitOffset;
            float radius = 3f;

            float x = center.x + ((float)Math.Cos(ang) * radius);
            float y = center.y;
            float z = center.z + ((float)Math.Sin(ang) * radius);

            return new Vec3(x, y, z);
        }

        private Vec3 GetCirclePosition()
        {
            rotAngle += IDLE_SPEED;

            float cx = (float)Math.Cos(rotAngle) * 5f;
            float cy = DEFAULT_HEIGHT;
            float cz = (float)Math.Sin(rotAngle) * 5f;

            return new Vec3(cx, cy, cz);
        }

        private object[] BuildMovementData(Vec3 p, Vec3 encrypted)
        {
            short sx = (short)(p.x * 1000);
            short sy = (short)(p.y * 1000);
            short sz = (short)(p.z * 1000);

            return new object[]
            {
                viewID, false, null,
                (short)40, sx, sy, (short)0, (short)0, (short)0, sz,
                (short)0, (short)0, (short)97, (short)0, (short)0,
                (short)10000, (byte)1, (byte)0, (byte)1, (byte)0, (byte)0,
                999, encrypted, new Quat(0f, 0f, 0f, 1f)
            };
        }

        // chat messages
        public void SendChat(string msg)
        {
            if (!hasSpawned) 
                return;

            string displayName = BotName;
            if (!string.IsNullOrWhiteSpace(ClanTag))
                displayName = ClanTag + " " + BotName;

            rpcs.RpcSendChatMessage(displayName, msg, 255, 255, 255);
        }

        // announcements
        public void SendAnnouncement(string txt, float dur = 5f)
        {
            if (!hasSpawned) 
                return;

            rpcs.ShowAnnouncement(txt, dur);
        }

        private void OnEventReceived(EventData photonEvent)
        {
            if (photonEvent.Code != MOVEMENT_EVENT) 
                return;

            try
            {
                if (photonEvent.CustomData is Hashtable)
                {
                    Hashtable data = (Hashtable)photonEvent.CustomData;
                    
                    if (data.ContainsKey((byte)10))
                    {
                        object[] movData = data[(byte)10] as object[];
                        
                        if (movData != null && movData.Length >= 23)
                        {
                            int vID = (int)movData[0];
                            Vec3 encPos = (Vec3)movData[22];
                            
                            int actorNum = vID / 1000;
                            Vec3 decPos = FairCollection.GetDecryptedVector3(encPos);

                            lock (posLock)
                            {
                                playerPositions[actorNum] = decPos;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ignore bad packets
            }
        }

        private void OnStateChange(ClientState prev, ClientState curr)
        {
            if (curr == ClientState.Joined)
            {
                HandleRoomJoined();
            }
            else if (curr == ClientState.Disconnected)
            {
                OnDisconnected();
            }
        }

        private void OnDisconnected()
        {
            hasSpawned = false;
            isActive = false;
            viewID = 0;
            rpcs = null;

            lock (posLock)
            {
                playerPositions.Clear();
            }
        }

        private void SendReliableEvent(byte evtCode, Hashtable data, EventCaching caching = EventCaching.DoNotCache)
        {
            RaiseEventOptions opts = RaiseEventOptions.Default;
            
            if (caching != EventCaching.DoNotCache)
            {
                opts = new RaiseEventOptions();
                opts.CachingOption = caching;
            }

            OpRaiseEvent(evtCode, data, opts, SendOptions.SendReliable);
        }

        private string GenerateBotName()
        {
            Random rand = new Random();
            int num = rand.Next(1000, 9999);
            return "PC-Bot_" + num.ToString();
        }

        private static Vec3 NormalizeVector(Vec3 vector)
        {
            float magnitude = (float)Math.Sqrt(
                vector.x * vector.x + 
                vector.y * vector.y + 
                vector.z * vector.z
            );
            
            if (magnitude > 0)
            {
                return new Vec3(
                    vector.x / magnitude,
                    vector.y / magnitude,
                    vector.z / magnitude
                );
            }
            
            return vector;
        }

        private static float GetDistance(Vec3 from, Vec3 to)
        {
            float deltaX = to.x - from.x;
            float deltaY = to.y - from.y;
            float deltaZ = to.z - from.z;
            
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        public List<Player> GetPlayers()
        {
            if (CurrentRoom != null)
            {
                List<Player> players = new List<Player>();
                foreach (Player p in CurrentRoom.Players.Values)
                {
                    players.Add(p);
                }
                return players;
            }
            
            return new List<Player>();
        }

        public new void Disconnect()
        {
            isActive = false;
            base.Disconnect();
        }
    }
}
