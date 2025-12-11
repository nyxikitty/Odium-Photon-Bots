using BLF_Odium_Network_Bots;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Windows.Forms;

namespace BFBot.Core
{
    public class BFPhotonClient : LoadBalancingClient, IConnectionCallbacks, IInRoomCallbacks, ILobbyCallbacks, IMatchmakingCallbacks
    {
        private readonly Thread photonThread;
        private readonly Thread movementThread;
        public List<string> logs;
        public List<string> eventLogs;

        private readonly Hashtable SendInstantiateEvHashtable = new Hashtable();
        private RaiseEventOptions SendInstantiateRaiseEventOptions = new RaiseEventOptions();

        private int intervalDispatch = 10;
        private int lastDispatch = Environment.TickCount;
        private int intervalSend = 50;
        private int lastSend = Environment.TickCount;
        private BFPhotonClient instance;

        private bool hasSpawned = false;
        private int allocatedViewID = 0;
        private float angle = 0f;
        public bool isMoving = false;

        private string botName;
        private string clanTag = "";
        private int teamNumber = 0;
        private static byte rank = (byte)new Random().Next(0, 255);

        public bool isOrbiting = false;
        public int orbitTargetActorNumber = -1;
        public float orbitOffset = 0f;

        private Dictionary<int, Vec3> playerPositions = new Dictionary<int, Vec3>();

        public bool isFollowing = false;
        public int followTargetActorNumber = -1;
        private float followStopDistance = 2f;

        public BFPhotonClient(string appId = "8c2cad3e-2e3f-4941-9044-b390ff2c4956", string appVersion = "1.84.0_1.99")
        {
            PhotonCustomTypes.Register();

            logs = new List<string>(150);
            eventLogs = new List<string>(75);
            instance = this;

            this.botName = "Meow " + new Random().Next(1000, 10000);

            this.AppId = appId;
            AppVersion = appVersion;
            this.NameServerHost = "ns.exitgames.com";

            photonThread = new Thread(PhotonLoop);
            photonThread.IsBackground = true;
            photonThread.Start();

            movementThread = new Thread(MovementLoop);
            movementThread.IsBackground = true;
            movementThread.Start();

            this.AddCallbackTarget(this);
            this.EventReceived += CustomOnEvent;
            this.StateChanged += OnStateChanged;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[BFBot] Connecting to US region...");

            if (this.ConnectToRegionMaster("us"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[BFBot] Connection initiated successfully");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[BFBot] Failed to initiate connection!");
            }
        }

        public bool JoinRandomRoom()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[BFBot] Attempting to join random room...");
            return this.OpJoinRandomRoom();
        }

        public bool JoinRoomByName(string roomName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[BFBot] Attempting to join room: {roomName}");
            return this.OpJoinRoom(new EnterRoomParams { RoomName = roomName });
        }

        public bool CreateRoom(string roomName = null, string mapName = "Outpost", string modeName = "Team Deathmatch", int maxPlayers = 10, int maxPing = 1000)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "BotRoom_" + new Random().Next(1000, 10000);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[BFBot] Creating room: {roomName}");
            Console.WriteLine($"[BFBot] Map: {mapName}, Mode: {modeName}, Max Players: {maxPlayers}, Max Ping: {maxPing}");

            RoomOptions roomOptions = new RoomOptions
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = (byte)maxPlayers,
                EmptyRoomTtl = 0,
                DeleteNullProperties = true,
                PublishUserId = false,
                CustomRoomProperties = new Hashtable
                {
                    { "mapName", mapName },
                    { "modeName", modeName },
                    { "password", "" },
                    { "hardcore", false },
                    { "dedicated", false },
                    { "matchStarted", false },
                    { "maxPing", maxPing },
                    { "roomType", (byte)0 },
                    { "scorelimit", 150 },
                    { "gameVersion", "1.84.0" }
                },
                CustomRoomPropertiesForLobby = new string[]
                {
                    "mapName", "modeName", "password", "hardcore",
                    "dedicated", "matchStarted", "maxPing"
                }
            };

            return this.OpCreateRoom(new EnterRoomParams
            {
                RoomName = roomName,
                RoomOptions = roomOptions
            });
        }

        public void SendAnnouncement(string text, float duration = 5.0f)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[BFBot] Sending announcement: {text}");

            Hashtable announcementData = new Hashtable();
            announcementData[(byte)0] = allocatedViewID;
            announcementData[(byte)4] = new object[] { text, duration };
            announcementData[(byte)5] = (byte)61;

            RaiseEventOptions eventOptions = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCacheGlobal
            };

            SendOptions sendOptions = new SendOptions
            {
                Reliability = true
            };

            this.OpRaiseEvent(200, announcementData, eventOptions, sendOptions);
        }

        public void StartOrbitingPlayer(int actorNumber, float angleOffset = 0f)
        {
            orbitTargetActorNumber = actorNumber;
            orbitOffset = angleOffset;
            isOrbiting = true;
            isFollowing = false;
            isMoving = true;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[BFBot] Started orbiting player {actorNumber} with offset {angleOffset:F2}");
        }

        public void StopOrbiting()
        {
            isOrbiting = false;
            orbitTargetActorNumber = -1;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[BFBot] Stopped orbiting");
        }

        public void StartFollowingPlayer(int actorNumber)
        {
            followTargetActorNumber = actorNumber;
            isFollowing = true;
            isOrbiting = false;
            isMoving = true;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[BFBot] Started following player {actorNumber}");
        }

        public void StopFollowing()
        {
            isFollowing = false;
            followTargetActorNumber = -1;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[BFBot] Stopped following");
        }

        public List<Player> GetPlayerList()
        {
            if (this.CurrentRoom != null)
            {
                return new List<Player>(this.CurrentRoom.Players.Values);
            }
            return new List<Player>();
        }

        private void OnStateChanged(ClientState before, ClientState now)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[BFBot] State: {before} -> {now}");
        }

        private void CustomOnEvent(EventData eventData)
        {
            if (eventData.Code != 201 || eventData.CustomData == null)
                return;

            try
            {
                var data = eventData.CustomData as Hashtable;
                if (data == null || !data.ContainsKey((byte)10))
                    return;

                var movementArray = data[(byte)10] as object[];
                if (movementArray == null || movementArray.Length < 23)
                    return;

                int viewID = (int)movementArray[0];
                int actorNum = viewID / 1000;

                if (movementArray[22] is Vec3 encPos)
                {
                    Vec3 pos = FairCollection.GetDecryptedVector3(encPos);
                    playerPositions[actorNum] = pos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERR] Position parse failed: " + ex.Message);
            }
        }

        private void PhotonLoop()
        {
            while (true)
            {
                DoPhotonStuff();
                Thread.Sleep(10);
            }
        }

        private void DoPhotonStuff()
        {
            if (Environment.TickCount - this.lastDispatch > this.intervalDispatch)
            {
                lastDispatch = Environment.TickCount;
                this.LoadBalancingPeer.DispatchIncomingCommands();
            }
            if (Environment.TickCount - this.lastSend > this.intervalSend)
            {
                lastSend = Environment.TickCount;
                this.LoadBalancingPeer.SendOutgoingCommands();
            }
        }

        private void MovementLoop()
        {
            while (true)
            {
                if (isMoving && allocatedViewID > 0)
                {
                    SendMovementUpdate();
                }
                Thread.Sleep(200);
            }
        }

        public void OnConnected()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BFBot] Connected to Photon server");
        }

        public void OnConnectedToMaster()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BFBot] Connected to Master server");
            Console.WriteLine($"[BFBot] Bot name: {clanTag} {botName}");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BFBot] Disconnected! Cause: {cause}");
            isMoving = false;
            isOrbiting = false;
            isFollowing = false;
        }

        public void OnRegionListReceived(RegionHandler regionHandler) { }
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BFBot] Auth failed: {debugMessage}");
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[BFBot] Player joined: {newPlayer.NickName} [{newPlayer.ActorNumber}]");
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BFBot] Player left: {otherPlayer.NickName} [{otherPlayer.ActorNumber}]");
            playerPositions.Remove(otherPlayer.ActorNumber);

            if (isOrbiting && orbitTargetActorNumber == otherPlayer.ActorNumber)
            {
                StopOrbiting();
            }

            if (isFollowing && followTargetActorNumber == otherPlayer.ActorNumber)
            {
                StopFollowing();
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }
        public void OnMasterClientSwitched(Player newMasterClient)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[BFBot] New master: {newMasterClient.NickName} [{newMasterClient.ActorNumber}]");
        }

        public void OnJoinedLobby()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BFBot] Joined lobby");
        }

        public void OnLeftLobby()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[BFBot] Left lobby");
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList) { }
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnCreatedRoom()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BFBot] Room created successfully!");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BFBot] Failed to create room! Code: {returnCode} Message: {message}");
        }

        public void OnJoinedRoom()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[BFBot] Joined room: {this.CurrentRoom.Name}");
            Console.WriteLine($"[BFBot] My Actor Number: {this.LocalPlayer.ActorNumber}");
            FairCollection.InitOperationAsync();

            if (!hasSpawned)
            {
                Thread.Sleep(100);
                SetPlayerName();
                Thread.Sleep(100);
                SetPlayerName();
                Thread.Sleep(200);
                SetPlayerProperties();
                Thread.Sleep(300);
                SpawnBot();
            }
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BFBot] Failed to join room! Code: {returnCode} Message: {message}");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BFBot] Failed to join random room! Code: {returnCode} Message: {message}");
        }

        public void OnLeftRoom()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[BFBot] Left room");
            hasSpawned = false;
            isMoving = false;
            isOrbiting = false;
            isFollowing = false;
            playerPositions.Clear();
        }

        private int AllocateViewID()
        {
            int actorNumber = this.LocalPlayer.ActorNumber;
            int maxViewIDs = 1000;
            int subId = 1;
            int viewID = (actorNumber * maxViewIDs) + subId;
            Console.WriteLine($"[BFBot] Allocated ViewID: {actorNumber} * {maxViewIDs} + {subId} = {viewID}");
            return viewID;
        }

        private void SetPlayerName()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[BFBot] Setting player name: {botName}");

            this.LocalPlayer.NickName = clanTag + " " + botName;

            Hashtable nameProps = new Hashtable
            {
                { (byte)255, clanTag + " " + botName }
            };
            this.LocalPlayer.SetCustomProperties(nameProps);
        }

        private void SetPlayerProperties()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[BFBot] Sending Op 252 - Full player properties");

            Hashtable playerProps = new Hashtable
            {
                { "teamNumber", (byte)teamNumber },
                { "rank", (byte)rank },
                { "killstreak", (byte)0 },
                { "characterCamo", (byte)1 },
                { "bulletTracerColor", (byte)0 },
                { "unlockedweapons", new int[] { 0, 0 } }
            };

            this.LocalPlayer.SetCustomProperties(playerProps);
        }

        private void SpawnBot()
        {
            if (hasSpawned) return;

            hasSpawned = true;
            allocatedViewID = AllocateViewID();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[BFBot] Spawning with ViewID: {allocatedViewID}");

            Console.WriteLine("[BFBot] Sending Event 207 (Actor Announcement)");
            Hashtable event207 = new Hashtable
            {
                { (byte)0, this.LocalPlayer.ActorNumber }
            };
            this.OpRaiseEvent(207, event207, RaiseEventOptions.Default, SendOptions.SendReliable);
            Thread.Sleep(100);

            Console.WriteLine("[BFBot] Sending Event 202 (PlayerBody Instantiation)");
            Hashtable event202 = new Hashtable
            {
                { (byte)0, "PlayerBody" },
                { (byte)6, this.LoadBalancingPeer.ServerTimeInMilliSeconds },
                { (byte)7, allocatedViewID }
            };
            RaiseEventOptions cacheOptions = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache
            };
            this.OpRaiseEvent(202, event202, cacheOptions, SendOptions.SendReliable);
            Thread.Sleep(200);

            Console.WriteLine("[BFBot] Sending Event 200 (First Weapon - method 22)");
            int weaponTime = this.LoadBalancingPeer.ServerTimeInMilliSeconds;
            Hashtable weapon1 = new Hashtable
            {
                { (byte)0, allocatedViewID },
                { (byte)2, weaponTime },
                { (byte)5, (byte)22 },
                { (byte)4, new object[] { (byte)1 } }
            };
            this.OpRaiseEvent(200, weapon1, RaiseEventOptions.Default, SendOptions.SendReliable);
            Thread.Sleep(50);

            Console.WriteLine("[BFBot] Sending Op 252 - Character customization");
            Hashtable customProps = new Hashtable
            {
                { "characterCamo", (byte)0 },
                { "bulletTracerColor", (byte)0 }
            };
            this.LocalPlayer.SetCustomProperties(customProps);
            Thread.Sleep(50);

            Console.WriteLine("[BFBot] Sending Event 200 (Second Weapon - method 77)");
            Hashtable weapon2 = new Hashtable
            {
                { (byte)0, allocatedViewID },
                { (byte)2, this.LoadBalancingPeer.ServerTimeInMilliSeconds },
                { (byte)5, (byte)77 },
                { (byte)4, new object[] { (byte)14 } }
            };
            this.OpRaiseEvent(200, weapon2, RaiseEventOptions.Default, SendOptions.SendReliable);
            Thread.Sleep(50);

            Console.WriteLine("[BFBot] Sending Event 200 (Third Weapon - method 76)");
            Hashtable weapon3 = new Hashtable
            {
                { (byte)0, allocatedViewID },
                { (byte)2, this.LoadBalancingPeer.ServerTimeInMilliSeconds },
                { (byte)5, (byte)76 },
                { (byte)4, new object[] { (byte)0 } }
            };
            this.OpRaiseEvent(200, weapon3, RaiseEventOptions.Default, SendOptions.SendReliable);
            Thread.Sleep(200);

            Console.WriteLine("[BFBot] Sending Event 201 (Initial Position/Health)");
            SendInitialPosition();
            Thread.Sleep(100);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BFBot] Spawn sequence complete! Bot is visible and ready for movement.");

            Thread.Sleep(2000);

            this.SendChatMessage($"Whats good in the hood twin! I am {botName}! Nice to meet you twin...", 255, 255, 255);

            isMoving = true;
        }

        private void SendInitialPosition()
        {
            object[] movementArray = new object[]
            {
                allocatedViewID,
                false,
                null,
                (short)0,
                (short)1750,
                (short)1750,
                (short)0,
                (short)0,
                (short)0,
                (short)106,
                (short)0,
                (short)0,
                (short)97,
                (short)0,
                (short)0,
                (short)10000,
                (byte)1,
                (byte)0,
                (byte)1,
                (byte)0,
                (byte)0,
                999,
                new Vec3(0f, 0f, 0f),
                new Quat(0f, 0f, 0f, 1f)
            };

            Hashtable event201 = new Hashtable
            {
                { (byte)10, movementArray },
                { (byte)0, this.LoadBalancingPeer.ServerTimeInMilliSeconds },
                { (byte)1, (short)0 }
            };

            RaiseEventOptions targetOthers = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };

            this.OpRaiseEvent(201, event201, targetOthers, SendOptions.SendReliable);
        }

        private void SendMovementUpdate()
        {
            float x, y, z;

            if (isFollowing && followTargetActorNumber > 0 && playerPositions.ContainsKey(followTargetActorNumber))
            {

                Vec3 targetPos = playerPositions[followTargetActorNumber];

                float currentX = (float)Math.Cos(angle) * 5f;
                float currentZ = (float)Math.Sin(angle) * 5f;

                float dx = targetPos.x - currentX;
                float dz = targetPos.z - currentZ;
                float distance = (float)Math.Sqrt(dx * dx + dz * dz);

                if (distance > followStopDistance)
                {

                    float moveSpeed = 0.05f;
                    float dirX = dx / distance;
                    float dirZ = dz / distance;

                    x = currentX + dirX * moveSpeed * distance;
                    z = currentZ + dirZ * moveSpeed * distance;
                    y = 1.75f;
                }
                else
                {

                    x = currentX;
                    z = currentZ;
                    y = 1.75f;
                }

                angle += 0.01f;

            }
            else if (isOrbiting && orbitTargetActorNumber > 0 && playerPositions.ContainsKey(orbitTargetActorNumber))
            {

                Vec3 targetPos = playerPositions[orbitTargetActorNumber];
                float radius = 3f;

                x = targetPos.x + (float)Math.Cos(angle + orbitOffset) * radius;
                y = targetPos.y;
                z = targetPos.z + (float)Math.Sin(angle + orbitOffset) * radius;

                angle += 0.08f;
            }
            else
            {

                float radius = 5f;
                x = (float)Math.Cos(angle) * radius;
                z = (float)Math.Sin(angle) * radius;
                y = 1.75f;
                angle += 0.05f;
            }

            short scaledX = (short)Math.Round(x * 1000);
            short scaledY = (short)Math.Round(y * 1000);
            short scaledZ = (short)Math.Round(z * 1000);
            var encPos = FairCollection.GetEncryptedVector3(new Vec3(x, y, z));

            object[] movementArray = new object[]
            {
                allocatedViewID,
                false,
                null,
                (short)40,
                scaledX,
                scaledY,
                (short)0,
                (short)0,
                (short)0,
                scaledZ,
                (short)0,
                (short)0,
                (short)97,
                (short)0,
                (short)0,
                (short)10000,
                (byte)1,
                (byte)0,
                (byte)1,
                (byte)0,
                (byte)0,
                999,
                encPos,
                new Quat(0f, 0f, 0f, 1f)
            };

            Hashtable event201 = new Hashtable
            {
                { (byte)10, movementArray },
                { (byte)0, this.LoadBalancingPeer.ServerTimeInMilliSeconds },
                { (byte)1, (short)0 }
            };

            RaiseEventOptions targetOthers = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };

            this.OpRaiseEvent(201, event201, targetOthers, SendOptions.SendUnreliable);
        }

        public void SendChatMessage(string msg, byte r = 255, byte g = 255, byte b = 255)
        {
            Hashtable eventData = new Hashtable
            {
                { (byte)0, allocatedViewID },
                { (byte)2, this.LoadBalancingPeer.ServerTimeInMilliSeconds },
                { (byte)5, (byte)50 },
                { (byte)4, new object[]
                    {
                        $"{clanTag + " " + botName}",
                        msg,
                        r,
                        g,
                        b
                    }
                }
            };

            this.OpRaiseEvent(200, eventData, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        public void StartMovement()
        {
            isMoving = true;
        }

        public void StopMovement()
        {
            isMoving = false;
        }

        public new void Disconnect()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[BFBot] Disconnecting...");
            isMoving = false;
            isOrbiting = false;
            isFollowing = false;
            base.Disconnect();
        }
    }
}