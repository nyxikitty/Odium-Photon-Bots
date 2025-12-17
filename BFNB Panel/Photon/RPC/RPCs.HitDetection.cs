namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 85: Hit verification result
        public void RpcHitVerified(string shotID, bool verified, string info)
        {
            SendRPC(85, shotID, verified, info);
        }

        // RPC 86: Verify hit request
        public void RpcVerifyHit(string shotID, int damagerID, int damagedPlayerID, int weaponTypeID,
            float bulletStartPosX, float bulletStartPosY, float bulletStartPosZ,
            float bulletHitPosX, float bulletHitPosY, float bulletHitPosZ)
        {
            SendRPC(86, shotID, damagerID, damagedPlayerID, weaponTypeID,
                bulletStartPosX, bulletStartPosY, bulletStartPosZ,
                bulletHitPosX, bulletHitPosY, bulletHitPosZ);
        }
    }
}
