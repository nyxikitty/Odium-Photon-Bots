namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 0: Acknowledge damage done to another player
        public void AcknowledgeDamageDoneRPC(string status, float damage, int victimID)
        {
            SendRPC(0, status, damage, victimID);
        }

        // RPC 16: Notify got kill assist
        public void GotKillAssist(float amount, int killedID)
        {
            SendRPC(16, amount, killedID);
        }

        // RPC 17: Update player health
        public void HealthUpdated(float value)
        {
            SendRPC(17, value);
        }

        // RPC 48: Player died
        public void RpcDie(int killedPlayerID, int killerID, byte killerHealth, byte killerWeapon, bool headshot)
        {
            SendRPC(48, killedPlayerID, killerID, killerHealth, killerWeapon, headshot);
        }

        // RPC 52: Show hitmarker
        public void RpcShowHitmarker()
        {
            SendRPC(52);
        }

        // RPC 87: Player respawned
        public void RpcRespawned(Vec3 spawnPoint)
        {
            SendRPC(87, spawnPoint);
        }

        // RPC 93: Player hit player hitmarker
        public void RpcPlayerHitPlayerHitmarker(int damagerID, int damagedPlayerID, byte weaponType, bool headshot)
        {
            SendRPC(93, damagerID, damagedPlayerID, weaponType, headshot);
        }
    }
}
