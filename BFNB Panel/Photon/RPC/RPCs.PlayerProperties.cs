namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 57: Set player ping
        public void SetPing(short ping)
        {
            SendRPC(57, ping);
        }

        // RPC 58: Set player rank
        public void SetRank(byte rank)
        {
            SendRPC(58, rank);
        }

        // RPC 71: Update team number
        public void UpdateTeamNumber(byte value)
        {
            SendRPC(71, value);
        }

        // RPC 75: Username changed
        public void UsernameChanged(string value)
        {
            SendRPC(75, value);
        }

        // RPC 84: Set K/D ratio
        public void SetKD(float kd)
        {
            SendRPC(84, kd);
        }

        // RPC 68: Update multiplayer deaths
        public void UpdateMPDeaths(int value)
        {
            SendRPC(68, value);
        }

        // RPC 69: Update multiplayer kills
        public void UpdateMPKills(int value)
        {
            SendRPC(69, value);
        }

        // RPC 70: Update multiplayer rounds
        public void UpdateMPRounds(int value)
        {
            SendRPC(70, value);
        }
    }
}
