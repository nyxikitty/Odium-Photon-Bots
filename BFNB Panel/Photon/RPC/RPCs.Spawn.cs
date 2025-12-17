namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 15: Get best spawn point for player
        public void GetBestSpawnPointForPlayer(int flagIDToSpawnOn)
        {
            SendRPC(15, flagIDToSpawnOn);
        }

        // RPC 59: Set spawn point
        public void SetSpawnPoint(Vec3 spawnPoint, int spawnedPlayerActorNr)
        {
            SendRPC(59, spawnPoint, spawnedPlayerActorNr);
        }

        // RPC 65: Teleport to position
        public void TeleportToPosition(Vec3 position)
        {
            SendRPC(65, position);
        }
    }
}
