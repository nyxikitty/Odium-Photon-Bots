using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BLF_Odium_Network_Bots.Photon;

namespace OdiumPhoton.Core
{
    public partial class PhotonBot : LoadBalancingClient
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

        private short fakePing = 25;
        private int lastPingUpdate = 0;
        private const int PING_UPDATE_INTERVAL = 3000;

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
        private float orbitRadius = 3f;
        private float orbitSpeed = ORBIT_SPEED;

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

        private short GetFakePing()
        {
            int now = Environment.TickCount;

            if (now - lastPingUpdate > PING_UPDATE_INTERVAL)
            {
                lastPingUpdate = now;
                Random rnd = new Random();
                fakePing = (short)rnd.Next(12, 51);
            }

            return fakePing;
        }

        private string Log(string message)
        {
            return $"[{BotName}]: {message}";
        }

        private void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{BotName}]: {message}");
            Console.ResetColor();
        }

        private void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{BotName}]: {message}");
            Console.ResetColor();
        }

        private void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{BotName}]: {message}");
            Console.ResetColor();
        }

        private void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{BotName}]: {message}");
            Console.ResetColor();
        }

        public new void Disconnect()
        {
            isActive = false;
            base.Disconnect();
        }

        private string GenerateBotName()
        {
            Random rand = new Random();
            int num = rand.Next(1000, 9999);
            return "PC-Bot_" + num.ToString();
        }
    }
}