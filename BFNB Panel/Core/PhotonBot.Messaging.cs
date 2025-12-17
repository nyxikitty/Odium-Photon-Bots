using System;
using System.Collections.Generic;

namespace OdiumPhoton.Core
{
    public partial class PhotonBot
    {
        public void SendChat(string msg)
        {
            if (!hasSpawned)
                return;

            string displayName = BotName;
            if (!string.IsNullOrWhiteSpace(ClanTag))
                displayName = ClanTag + " " + BotName;

            rpcs.RpcSendChatMessage(displayName, msg, 255, 255, 255);
        }

        public void SendAnnouncement(string txt, float dur = 5f)
        {
            if (!hasSpawned)
                return;

            rpcs.ShowAnnouncement(txt, dur);
        }

        public void ShowPerkMessage(string message)
        {
            if (!hasSpawned) return;

            string name = BotName;
            if (!string.IsNullOrWhiteSpace(ClanTag))
                name = ClanTag + " " + BotName;

            rpcs.RpcShowPerkMessage(name, message);
        }

        public void ShowCustomPerk(string username, string message)
        {
            if (!hasSpawned) return;
            rpcs.RpcShowPerkMessage(username, message);
        }

        public Dictionary<int, Vec3> GetPlayerPositions()
        {
            Dictionary<int, Vec3> positions = new Dictionary<int, Vec3>();

            lock (posLock)
            {
                foreach (var kvp in playerPositions)
                {
                    positions[kvp.Key] = kvp.Value;
                }
            }

            return positions;
        }
    }
}