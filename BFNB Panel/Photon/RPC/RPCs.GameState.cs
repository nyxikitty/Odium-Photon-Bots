namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 2: Become the new master client
        public void BecomeNewMasterClient()
        {
            SendRPC(2);
        }

        // RPC 29: Match over state changed
        public void MatchOverChanged(bool value)
        {
            SendRPC(29, value);
        }

        // RPC 33: Nuke kill activated
        public void NukeKill()
        {
            SendRPC(33);
        }

        // RPC 66: Update alive players count
        public void UpdateAlivePlayers(int team0Alive, int team1Alive)
        {
            SendRPC(66, team0Alive, team1Alive);
        }

        // RPC 67: Update Hard Mode Free For All rounds
        public void UpdateHMFFARounds(int playerID, int roundsWon)
        {
            SendRPC(67, playerID, roundsWon);
        }

        // RPC 79: Force killstreak
        public void RpcForceKillstreak(int killstreak, bool isOnTheSameTeam)
        {
            SendRPC(79, killstreak, isOnTheSameTeam);
        }

        // RPC 81: Decrease countdown
        public void DecreaseCountDown()
        {
            SendRPC(81);
        }

        // RPC 82: Increase number
        public void IncreaseNumber()
        {
            SendRPC(82);
        }

        // RPC 83: Send new countdown to clients
        public void SendNewCountDownToClients()
        {
            SendRPC(83);
        }
    }
}
