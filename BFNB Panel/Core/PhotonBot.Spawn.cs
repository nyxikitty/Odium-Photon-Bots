using BLF_Odium_Network_Bots.Photon;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
using System.Threading;
namespace OdiumPhoton.Core
{
    public partial class PhotonBot
    {
        private void HandleRoomJoined()
        {
            if (hasSpawned)
            {
                LogWarning("Already spawned, skipping setup");
                return;
            }
            LogSuccess("Room joined, initializing bot...");
            FairCollection.InitOperationAsync();
            Thread.Sleep(100);
            SetupPlayer();
            Thread.Sleep(300);
            PerformSpawn();
            hasSpawned = true;
            LogSuccess("Bot ready");
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
            LogInfo($"Player setup complete | Team {Team} | Rank {Rank}");
        }
        private void PerformSpawn()
        {
            viewID = LocalPlayer.ActorNumber * 1000 + 1;
            rpcs = new RPCs(this, viewID);
            LogInfo($"Spawning with ViewID {viewID}");
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
            LogSuccess("PlayerBody instantiated!");
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
            LogInfo("Loadout applied");
            Thread.Sleep(200);
            BroadcastMovement(true);
            Thread.Sleep(1000);
            isActive = true;

            // log all players
            LogInfo("Current players in room:");
            foreach (Player p in CurrentRoom.Players.Values)
            {
                LogInfo($"- Actor {p.ActorNumber}: {p.NickName}");
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
    }
}