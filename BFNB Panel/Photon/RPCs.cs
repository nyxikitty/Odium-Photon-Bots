using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;

namespace BLF_Odium_Network_Bots.Photon
{
    /// <summary>
    /// Central RPC call manager for Bullet Force Photon operations
    /// </summary>
    public class RPCs
    {
        private const byte RPC_EVENT = 200;
        private readonly LoadBalancingClient _client;
        private readonly int _viewID;

        public RPCs(LoadBalancingClient client, int viewID)
        {
            _client = client;
            _viewID = viewID;
        }

        #region Core RPC Infrastructure
        private void SendRPC(byte methodId, params object[] parameters)
        {
            var data = new Hashtable
            {
                [(byte)0] = _viewID,
                [(byte)2] = _client.LoadBalancingPeer.ServerTimeInMilliSeconds,
                [(byte)5] = methodId,
                [(byte)4] = parameters
            };

            _client.OpRaiseEvent(RPC_EVENT, data,
                RaiseEventOptions.Default,
                SendOptions.SendReliable);
        }

        private void SendRPCCached(byte methodId, EventCaching cache, params object[] parameters)
        {
            var data = new Hashtable
            {
                [(byte)0] = _viewID,
                [(byte)2] = _client.LoadBalancingPeer.ServerTimeInMilliSeconds,
                [(byte)5] = methodId,
                [(byte)4] = parameters
            };

            _client.OpRaiseEvent(RPC_EVENT, data,
                new RaiseEventOptions { CachingOption = cache },
                SendOptions.SendReliable);
        }
        #endregion

        #region Player Combat & Damage (0-17)
        /// <summary>RPC 0: Acknowledge damage done to another player</summary>
        public void AcknowledgeDamageDoneRPC(string status, float damage, int victimID)
            => SendRPC(0, status, damage, victimID);

        /// <summary>RPC 2: Become the new master client</summary>
        public void BecomeNewMasterClient()
            => SendRPC(2);

        /// <summary>RPC 16: Notify got kill assist</summary>
        public void GotKillAssist(float amount, int killedID)
            => SendRPC(16, amount, killedID);

        /// <summary>RPC 17: Update player health</summary>
        public void HealthUpdated(float value)
            => SendRPC(17, value);
        #endregion

        #region Network & Latency (20-22)
        /// <summary>RPC 20: Kick a player (requires hashed password)</summary>
        public void KickPlayer(string playerToKick, string hashedPassword)
            => SendRPC(20, playerToKick, hashedPassword);

        /// <summary>RPC 22: Send latency ping</summary>
        public void LatencySend()
            => SendRPC(22);
        #endregion

        #region Weapon Actions (23-26, 30-31)
        /// <summary>RPC 23: Create grenade locally</summary>
        public void LocalCreateGrenade(Vec3 position, Vec3 velocity, float forcedDelay, byte grenadeWeaponType)
            => SendRPC(23, position, velocity, forcedDelay, grenadeWeaponType);

        /// <summary>RPC 24: Local player hurt</summary>
        public void LocalHurt(int damagerID, float damage, Vec3 localPosition, byte damagerWeapon, float newHealth)
            => SendRPC(24, damagerID, damage, localPosition, damagerWeapon, newHealth);

        /// <summary>RPC 25: Local reload animation</summary>
        public void LocalReload()
            => SendRPC(25);

        /// <summary>RPC 26: Spawn throwing weapon locally</summary>
        public void LocalSpawnThrowingWeapon(Vec3 position, Vec3 velocity, byte weaponType)
            => SendRPC(26, position, velocity, weaponType);

        /// <summary>RPC 30: Melee attack animation</summary>
        public void MpMeleeAnimation()
            => SendRPC(30);

        /// <summary>RPC 31: Throw grenade animation</summary>
        public void MpThrowGrenadeAnimation()
            => SendRPC(31);
        #endregion

        #region Match & Game State (29, 33, 66-73)
        /// <summary>RPC 29: Match over state changed</summary>
        public void MatchOverChanged(bool value)
            => SendRPC(29, value);

        /// <summary>RPC 33: Nuke kill activated</summary>
        public void NukeKill()
            => SendRPC(33);

        /// <summary>RPC 66: Update alive players count</summary>
        public void UpdateAlivePlayers(int team0Alive, int team1Alive)
            => SendRPC(66, team0Alive, team1Alive);

        /// <summary>RPC 67: Update Hard Mode Free For All rounds</summary>
        public void UpdateHMFFARounds(int playerID, int roundsWon)
            => SendRPC(67, playerID, roundsWon);

        /// <summary>RPC 68: Update multiplayer deaths</summary>
        public void UpdateMPDeaths(int value)
            => SendRPC(68, value);

        /// <summary>RPC 69: Update multiplayer kills</summary>
        public void UpdateMPKills(int value)
            => SendRPC(69, value);

        /// <summary>RPC 70: Update multiplayer rounds</summary>
        public void UpdateMPRounds(int value)
            => SendRPC(70, value);

        /// <summary>RPC 71: Update team number</summary>
        public void UpdateTeamNumber(byte value)
            => SendRPC(71, value);
        #endregion

        #region Pickups & Items (34, 38-40, 43-44)
        /// <summary>RPC 34: Initialize pickup item sync</summary>
        public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
            => SendRPC(34, timeBase, inactivePickupsAndTimes);

        /// <summary>RPC 38: Pickup item</summary>
        public void PunPickup()
            => SendRPC(38);

        /// <summary>RPC 39: Simple pickup</summary>
        public void PunPickupSimple()
            => SendRPC(39);

        /// <summary>RPC 40: Respawn pickup (no position)</summary>
        public void PunRespawn()
            => SendRPC(40);

        /// <summary>RPC 40: Respawn pickup (with position)</summary>
        public void PunRespawn(Vec3 position)
            => SendRPC(40, position);

        /// <summary>RPC 43: Request pickup items</summary>
        public void RequestForPickupItems()
            => SendRPC(43);

        /// <summary>RPC 44: Request pickup times</summary>
        public void RequestForPickupTimes()
            => SendRPC(44);
        #endregion

        #region Player Actions & Death (48, 51-53, 87)
        /// <summary>RPC 48: Player died</summary>
        public void RpcDie(int killedPlayerID, int killerID, byte killerHealth, byte killerWeapon, bool headshot)
            => SendRPC(48, killedPlayerID, killerID, killerHealth, killerWeapon, headshot);

        /// <summary>RPC 51: Shoot weapon</summary>
        public void RpcShoot(int actorID, float damage, Vec3 position, Vec3 direction, byte numberOfBullets, byte spread, double timeShot, int weaponType)
            => SendRPC(51, actorID, damage, position, direction, numberOfBullets, spread, timeShot, weaponType);

        /// <summary>RPC 52: Show hitmarker</summary>
        public void RpcShowHitmarker()
            => SendRPC(52);

        /// <summary>RPC 53: Show perk message</summary>
        public void RpcShowPerkMessage(string msgUsername, string msg)
            => SendRPC(53, msgUsername, msg);

        /// <summary>RPC 87: Player respawned</summary>
        public void RpcRespawned(Vec3 spawnPoint)
            => SendRPC(87, spawnPoint);
        #endregion

        #region Chat & Communication (50, 61)
        /// <summary>RPC 50: Send chat message</summary>
        public void RpcSendChatMessage(string msgUsername, string msg, short r, short g, short b)
            => SendRPC(50, msgUsername, msg, r, g, b);

        /// <summary>RPC 61: Show announcement (cached globally)</summary>
        public void ShowAnnouncement(string text, float time)
            => SendRPCCached(61, EventCaching.AddToRoomCacheGlobal, text, time);
        #endregion

        #region Player Properties (57-58, 75-77, 84)
        /// <summary>RPC 57: Set player ping</summary>
        public void SetPing(short ping)
            => SendRPC(57, ping);

        /// <summary>RPC 58: Set player rank</summary>
        public void SetRank(byte rank)
            => SendRPC(58, rank);

        /// <summary>RPC 75: Username changed</summary>
        public void UsernameChanged(string value)
            => SendRPC(75, value);

        /// <summary>RPC 76: Weapon camo changed</summary>
        public void WeaponCamoChanged(int value)
            => SendRPC(76, value);

        /// <summary>RPC 77: Weapon type changed</summary>
        public void WeaponTypeChanged(byte value)
            => SendRPC(77, value);

        /// <summary>RPC 84: Set K/D ratio</summary>
        public void SetKD(float kd)
            => SendRPC(84, kd);
        #endregion

        #region Spawn & Movement (15, 59, 65)
        /// <summary>RPC 15: Get best spawn point for player</summary>
        public void GetBestSpawnPointForPlayer(int flagIDToSpawnOn)
            => SendRPC(15, flagIDToSpawnOn);

        /// <summary>RPC 59: Set spawn point</summary>
        public void SetSpawnPoint(Vec3 spawnPoint, int spawnedPlayerActorNr)
            => SendRPC(59, spawnPoint, spawnedPlayerActorNr);

        /// <summary>RPC 65: Teleport to position</summary>
        public void TeleportToPosition(Vec3 position)
            => SendRPC(65, position);
        #endregion

        #region Killstreaks (79)
        /// <summary>RPC 79: Force killstreak</summary>
        public void RpcForceKillstreak(int killstreak, bool isOnTheSameTeam)
            => SendRPC(79, killstreak, isOnTheSameTeam);

        // Killstreak enum values for reference
        public enum Killstreak
        {
            None = 0,
            UAV = 1,
            SuperSoldier = 2,
            CounterUAV = 3,
            AdvancedUAV = 4,
            Haste = 5,
            Nuke = 6
        }
        #endregion

        #region Authentication & Admin (88, 91-92)
        /// <summary>RPC 88: Send multiplayer auth token (REQUIRED)</summary>
        public void RpcSendMultiplayerAuthToken(string token)
            => SendRPC(88, token);

        /// <summary>RPC 91: Kick player with reason</summary>
        public void RpcGetKicked(string reason)
            => SendRPC(91, reason);

        /// <summary>RPC 92: Report player for hacking</summary>
        public void RpcReportHack(int reportID, int hackerID, int hackType)
            => SendRPC(92, reportID, hackerID, hackType);
        #endregion

        #region Hit Detection (85-86, 93)
        /// <summary>RPC 85: Hit verification result</summary>
        public void RpcHitVerified(string shotID, bool verified, string info)
            => SendRPC(85, shotID, verified, info);

        /// <summary>RPC 86: Verify hit request</summary>
        public void RpcVerifyHit(string shotID, int damagerID, int damagedPlayerID, int weaponTypeID,
            float bulletStartPosX, float bulletStartPosY, float bulletStartPosZ,
            float bulletHitPosX, float bulletHitPosY, float bulletHitPosZ)
            => SendRPC(86, shotID, damagerID, damagedPlayerID, weaponTypeID,
                bulletStartPosX, bulletStartPosY, bulletStartPosZ,
                bulletHitPosX, bulletHitPosY, bulletHitPosZ);

        /// <summary>RPC 93: Player hit player hitmarker</summary>
        public void RpcPlayerHitPlayerHitmarker(int damagerID, int damagedPlayerID, byte weaponType, bool headshot)
            => SendRPC(93, damagerID, damagedPlayerID, weaponType, headshot);
        #endregion

        #region Vehicles (89-90, 99-100)
        /// <summary>RPC 89: Request to enter vehicle</summary>
        public void RpcRequestEnterVehicle()
            => SendRPC(89);

        /// <summary>RPC 90: Entered vehicle</summary>
        public void RpcEnteredVehicle()
            => SendRPC(90);

        /// <summary>RPC 99: Damage vehicle</summary>
        public void RpcDamageVehicle()
            => SendRPC(99);

        /// <summary>RPC 100: Vehicle destroyed</summary>
        public void RpcVehicleDestroyed()
            => SendRPC(100);
        #endregion

        #region Custom Maps (106-107)
        /// <summary>RPC 106: Send/receive custom map</summary>
        public void RpcSendReceiveCustomMapToServer(string mapID)
            => SendRPC(106, mapID);

        /// <summary>RPC 107: Custom map like/dislike</summary>
        public void RpcCustomMapLikeDislike()
            => SendRPC(107);
        #endregion

        #region Countdown & Master Client (81-83)
        /// <summary>RPC 81: Decrease countdown</summary>
        public void DecreaseCountDown()
            => SendRPC(81);

        /// <summary>RPC 82: Increase number</summary>
        public void IncreaseNumber()
            => SendRPC(82);

        /// <summary>RPC 83: Send new countdown to clients</summary>
        public void SendNewCountDownToClients()
            => SendRPC(83);
        #endregion

        #region Debug & Misc (14, 60, 62)
        /// <summary>RPC 14: Flash effect</summary>
        public void Flash()
            => SendRPC(14);

        /// <summary>RPC 60: Set time scale</summary>
        public void SetTimeScale(float scale)
            => SendRPC(60, scale);

        /// <summary>RPC 62: Show debug capsule</summary>
        public void ShowDebugCapsule(Vec3 position)
            => SendRPC(62, position);
        #endregion
    }
}