using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Photon.Realtime;

namespace OdiumPhoton.Core
{

    public class MatchRecorder
    {
        public class PlayerSnapshot
        {
            public int ActorNumber { get; set; }
            public string Username { get; set; }
            public string ClanTag { get; set; }
            public byte Team { get; set; }
            public byte Rank { get; set; }

            public Vec3 Position { get; set; }
            public Vec3 EncryptedPosition { get; set; }
            public Quat Rotation { get; set; }
            public object[] RawMovementData { get; set; }

            public byte WeaponType { get; set; }
            public int WeaponCamo { get; set; }
            public int Health { get; set; }
            public int Kills { get; set; }
            public int Deaths { get; set; }
            public float KD { get; set; }
            public long Timestamp { get; set; }
        }

        public class RPCEvent
        {
            public long Timestamp { get; set; }
            public int ActorNumber { get; set; }
            public string RPCName { get; set; }
            public object[] Parameters { get; set; }
            public byte EventCode { get; set; }
            public object RawEventData { get; set; }
        }

        public class RecordingSession
        {
            public DateTime RecordingStartTime { get; set; }
            public DateTime RecordingEndTime { get; set; }
            public string MapName { get; set; }
            public string GameMode { get; set; }
            public Dictionary<int, PlayerInfo> Players { get; set; } = new Dictionary<int, PlayerInfo>();
            public List<PlayerSnapshot> MovementData { get; set; } = new List<PlayerSnapshot>();
            public List<RPCEvent> RPCEvents { get; set; } = new List<RPCEvent>();
            public long Duration => (RecordingEndTime - RecordingStartTime).Ticks;
        }

        public class PlayerInfo
        {
            public int ActorNumber { get; set; }
            public string Username { get; set; }
            public string ClanTag { get; set; }
            public byte Team { get; set; }
            public byte Rank { get; set; }
            public byte InitialWeapon { get; set; }
            public int InitialCamo { get; set; }
        }

        private RecordingSession _currentSession;
        private bool _isRecording;
        private DateTime _recordingStart;
        private readonly Dictionary<int, PlayerSnapshot> _lastKnownState = new Dictionary<int, PlayerSnapshot>();

        public bool IsRecording => _isRecording;
        public RecordingSession CurrentSession => _currentSession;

        public void StartRecording(string mapName, string gameMode)
        {
            if (_isRecording) return;

            _currentSession = new RecordingSession
            {
                RecordingStartTime = DateTime.Now,
                MapName = mapName,
                GameMode = gameMode
            };

            _recordingStart = DateTime.Now;
            _isRecording = true;
            _lastKnownState.Clear();
        }

        public void StopRecording()
        {
            if (!_isRecording) return;

            _currentSession.RecordingEndTime = DateTime.Now;
            _isRecording = false;
        }

        public void RecordPlayer(Player player)
        {
            if (!_isRecording) return;

            int actorNumber = player.ActorNumber;
            if (_currentSession.Players.ContainsKey(actorNumber)) return;

            string fullName = player.NickName ?? "Unknown";
            string clanTag = "";
            string username = fullName;

            if (fullName.Contains("[") && fullName.Contains("]"))
            {
                int start = fullName.IndexOf('[');
                int end = fullName.IndexOf(']');
                if (end > start)
                {
                    clanTag = fullName.Substring(start, end - start + 1);
                    username = fullName.Substring(end + 1).Trim();
                }
            }

            byte team = 1;
            if (player.CustomProperties.ContainsKey("teamNumber"))
                team = (byte)player.CustomProperties["teamNumber"];

            byte rank = 0;
            if (player.CustomProperties.ContainsKey("rank"))
                rank = (byte)player.CustomProperties["rank"];

            var playerInfo = new PlayerInfo
            {
                ActorNumber = actorNumber,
                Username = username,
                ClanTag = clanTag,
                Team = team,
                Rank = rank,
                InitialWeapon = 14,
                InitialCamo = 0
            };

            _currentSession.Players[actorNumber] = playerInfo;
        }

        public void RecordMovement(int actorNumber, Vec3 position, Vec3 encrypted, Quat rotation, object[] rawMovementData)
        {
            if (!_isRecording) return;

            var snapshot = new PlayerSnapshot
            {
                ActorNumber = actorNumber,
                Position = position,
                EncryptedPosition = encrypted,
                Rotation = rotation,
                RawMovementData = rawMovementData,
                Timestamp = GetTimestamp()
            };

            if (_lastKnownState.TryGetValue(actorNumber, out var lastState))
            {
                snapshot.Username = lastState.Username;
                snapshot.ClanTag = lastState.ClanTag;
                snapshot.Team = lastState.Team;
                snapshot.Rank = lastState.Rank;
                snapshot.WeaponType = lastState.WeaponType;
                snapshot.WeaponCamo = lastState.WeaponCamo;
                snapshot.Health = lastState.Health;
                snapshot.Kills = lastState.Kills;
                snapshot.Deaths = lastState.Deaths;
                snapshot.KD = lastState.KD;
            }
            else if (_currentSession.Players.TryGetValue(actorNumber, out var playerInfo))
            {
                snapshot.Username = playerInfo.Username;
                snapshot.ClanTag = playerInfo.ClanTag;
                snapshot.Team = playerInfo.Team;
                snapshot.Rank = playerInfo.Rank;
                snapshot.WeaponType = playerInfo.InitialWeapon;
                snapshot.WeaponCamo = playerInfo.InitialCamo;
                snapshot.Health = 100;
            }

            _currentSession.MovementData.Add(snapshot);
            _lastKnownState[actorNumber] = snapshot;
        }

        public void RecordRPC(int actorNumber, string rpcName, params object[] parameters)
        {
            if (!_isRecording) return;

            var rpcEvent = new RPCEvent
            {
                Timestamp = GetTimestamp(),
                ActorNumber = actorNumber,
                RPCName = rpcName,
                Parameters = parameters
            };

            if (rpcName == "RawEvent" && parameters.Length >= 2)
            {
                rpcEvent.EventCode = (byte)parameters[0];
                rpcEvent.RawEventData = parameters[1];
            }

            _currentSession.RPCEvents.Add(rpcEvent);

            if (rpcName != "RawEvent" && rpcName != "RawRPCEvent")
            {
                UpdateStateFromRPC(actorNumber, rpcName, parameters);
            }
        }

        private void UpdateStateFromRPC(int actorNumber, string rpcName, object[] parameters)
        {
            if (!_lastKnownState.ContainsKey(actorNumber)) return;

            var state = _lastKnownState[actorNumber];

            switch (rpcName)
            {
                case "WeaponTypeChanged":
                    if (parameters.Length > 0 && parameters[0] is byte weapon)
                        state.WeaponType = weapon;
                    break;
                case "WeaponCamoChanged":
                    if (parameters.Length > 0 && parameters[0] is int camo)
                        state.WeaponCamo = camo;
                    break;
                case "UpdateMPKills":
                    if (parameters.Length > 0 && parameters[0] is int kills)
                        state.Kills = kills;
                    break;
                case "UpdateMPDeaths":
                    if (parameters.Length > 0 && parameters[0] is int deaths)
                        state.Deaths = deaths;
                    break;
                case "SetKD":
                    if (parameters.Length > 0 && parameters[0] is float kd)
                        state.KD = kd;
                    break;
                case "HealthUpdated":
                    if (parameters.Length > 0 && parameters[0] is float health)
                        state.Health = (int)health;
                    break;
            }
        }

        public void SaveRecording(string filepath)
        {
            if (_currentSession == null) return;

            var json = JsonConvert.SerializeObject(_currentSession, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filepath, json);
        }

        public RecordingSession LoadRecording(string filepath)
        {
            if (!File.Exists(filepath)) return null;

            var json = File.ReadAllText(filepath);
            return JsonConvert.DeserializeObject<RecordingSession>(json);
        }

        private long GetTimestamp()
        {
            return (DateTime.Now - _recordingStart).Ticks;
        }

        public Dictionary<long, List<PlayerSnapshot>> GetSnapshotsByTimestamp()
        {
            var grouped = new Dictionary<long, List<PlayerSnapshot>>();

            foreach (var snapshot in _currentSession.MovementData)
            {

                long interval = snapshot.Timestamp / TimeSpan.TicksPerMillisecond / 100;

                if (!grouped.ContainsKey(interval))
                    grouped[interval] = new List<PlayerSnapshot>();

                grouped[interval].Add(snapshot);
            }

            return grouped;
        }

        public List<RPCEvent> GetRPCsInTimeRange(long startTicks, long endTicks)
        {
            return _currentSession.RPCEvents
                .Where(rpc => rpc.Timestamp >= startTicks && rpc.Timestamp <= endTicks)
                .ToList();
        }

        public void Clear()
        {
            _currentSession = null;
            _isRecording = false;
            _lastKnownState.Clear();
        }
    }
}