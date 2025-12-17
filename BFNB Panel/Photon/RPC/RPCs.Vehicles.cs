namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 89: Request to enter vehicle
        public void RpcRequestEnterVehicle()
        {
            SendRPC(89);
        }

        // RPC 90: Entered vehicle
        public void RpcEnteredVehicle()
        {
            SendRPC(90);
        }

        // RPC 99: Damage vehicle
        public void RpcDamageVehicle()
        {
            SendRPC(99);
        }

        // RPC 100: Vehicle destroyed
        public void RpcVehicleDestroyed()
        {
            SendRPC(100);
        }
    }
}
