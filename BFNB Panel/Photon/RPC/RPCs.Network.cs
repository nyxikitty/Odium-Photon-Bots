namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 20: Kick a player (requires hashed password)
        public void KickPlayer(string playerToKick, string hashedPassword)
        {
            SendRPC(20, playerToKick, hashedPassword);
        }

        // RPC 22: Send latency ping
        public void LatencySend()
        {
            SendRPC(22);
        }

        // RPC 88: Send multiplayer auth token (REQUIRED)
        public void RpcSendMultiplayerAuthToken(string token)
        {
            SendRPC(88, token);
        }

        // RPC 91: Kick player with reason
        public void RpcGetKicked(string reason)
        {
            SendRPC(91, reason);
        }

        // RPC 92: Report player for hacking
        public void RpcReportHack(int reportID, int hackerID, int hackType)
        {
            SendRPC(92, reportID, hackerID, hackType);
        }
    }
}
