namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 14: Flash effect
        public void Flash()
        {
            SendRPC(14);
        }

        // RPC 60: Set time scale
        public void SetTimeScale(float scale)
        {
            SendRPC(60, scale);
        }

        // RPC 62: Show debug capsule
        public void ShowDebugCapsule(Vec3 position)
        {
            SendRPC(62, position);
        }

        // RPC 106: Send/receive custom map
        public void RpcSendReceiveCustomMapToServer(string mapID)
        {
            SendRPC(106, mapID);
        }

        // RPC 107: Custom map like/dislike
        public void RpcCustomMapLikeDislike()
        {
            SendRPC(107);
        }
    }
}
