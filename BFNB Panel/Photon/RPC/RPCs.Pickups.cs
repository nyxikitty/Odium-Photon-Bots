namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 34: Initialize pickup item sync
        public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
        {
            SendRPC(34, timeBase, inactivePickupsAndTimes);
        }

        // RPC 38: Pickup item
        public void PunPickup()
        {
            SendRPC(38);
        }

        // RPC 39: Simple pickup
        public void PunPickupSimple()
        {
            SendRPC(39);
        }

        // RPC 40: Respawn pickup (no position)
        public void PunRespawn()
        {
            SendRPC(40);
        }

        // RPC 40: Respawn pickup (with position)
        public void PunRespawn(Vec3 position)
        {
            SendRPC(40, position);
        }

        // RPC 43: Request pickup items
        public void RequestForPickupItems()
        {
            SendRPC(43);
        }

        // RPC 44: Request pickup times
        public void RequestForPickupTimes()
        {
            SendRPC(44);
        }
    }
}
