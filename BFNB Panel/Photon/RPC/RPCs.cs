using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;

namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        private const byte RPC_EVENT = 200;
        private readonly LoadBalancingClient client;
        private readonly int viewID;

        public RPCs(LoadBalancingClient client, int viewID)
        {
            this.client = client;
            this.viewID = viewID;
        }

        private void SendRPC(byte methodId, params object[] parameters)
        {
            Hashtable data = new Hashtable();
            data[(byte)0] = viewID;
            data[(byte)2] = client.LoadBalancingPeer.ServerTimeInMilliSeconds;
            data[(byte)5] = methodId;
            data[(byte)4] = parameters;

            client.OpRaiseEvent(RPC_EVENT, data,
                RaiseEventOptions.Default,
                SendOptions.SendReliable);
        }

        private void SendRPCCached(byte methodId, EventCaching cache, params object[] parameters)
        {
            Hashtable data = new Hashtable();
            data[(byte)0] = viewID;
            data[(byte)2] = client.LoadBalancingPeer.ServerTimeInMilliSeconds;
            data[(byte)5] = methodId;
            data[(byte)4] = parameters;

            RaiseEventOptions opts = new RaiseEventOptions();
            opts.CachingOption = cache;

            client.OpRaiseEvent(RPC_EVENT, data, opts, SendOptions.SendReliable);
        }

        private void SendSpoofedRPC(int spoofedActorNumber, byte methodId, params object[] parameters)
        {
            int fakeViewID = (spoofedActorNumber * 1000) + 1;

            Hashtable data = new Hashtable();
            data[(byte)0] = fakeViewID;
            data[(byte)2] = client.LoadBalancingPeer.ServerTimeInMilliSeconds;
            data[(byte)5] = methodId;
            data[(byte)4] = parameters;

            client.OpRaiseEvent(RPC_EVENT, data,
                RaiseEventOptions.Default,
                SendOptions.SendReliable);
        }

        public enum Killstreak
        {
            None = 0,
            UAV = 1,
            SuperSoldier = 2,
            CounterUAV = 3,
            AdvancedUAV = 4,
            Haste = 5,
            Nuke = 6
        }
    }
}