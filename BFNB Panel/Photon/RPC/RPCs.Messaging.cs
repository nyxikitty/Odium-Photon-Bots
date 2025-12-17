using ExitGames.Client.Photon;
using Photon.Realtime;

namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 50: Send chat message
        public void RpcSendChatMessage(string msgUsername, string msg, short r, short g, short b)
        {
            SendRPC(50, msgUsername, msg, r, g, b);
        }

        // RPC 53: Show perk message
        public void RpcShowPerkMessage(string msgUsername, string msg)
        {
            SendRPC(53, msgUsername, msg);
        }

        // RPC 61: Show announcement (cached globally)
        public void ShowAnnouncement(string text, float time)
        {
            SendRPCCached(61, EventCaching.AddToRoomCacheGlobal, text, time);
        }
    }
}