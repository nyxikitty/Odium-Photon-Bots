using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;

namespace OdiumPhoton.Core
{
    public partial class PhotonBot
    {
        private void BroadcastMovement(bool reliable = false)
        {
            Vec3 pos = CalculateNextPosition();
            Vec3 enc = FairCollection.GetEncryptedVector3(pos);

            Hashtable pkt = new Hashtable();
            pkt[(byte)10] = BuildMovementData(pos, enc);
            pkt[(byte)0] = LoadBalancingPeer.ServerTimeInMilliSeconds;
            pkt[(byte)1] = (short)0;

            RaiseEventOptions opts = new RaiseEventOptions();
            opts.Receivers = ReceiverGroup.Others;
            
            SendOptions sendOpt = SendOptions.SendUnreliable;
            if (reliable)
                sendOpt = SendOptions.SendReliable;

            OpRaiseEvent(MOVEMENT_EVENT, pkt, opts, sendOpt);
        }

        // corrupted movement packet to trigger floor collider bug
        public void BroadcastCorruptedMovement()
        {
            if (!hasSpawned) 
                return;

            Vec3 pos = CalculateNextPosition();
            Vec3 enc = FairCollection.GetEncryptedVector3(pos);

            Quat badQuat = new Quat(float.MaxValue, float.MaxValue, float.MaxValue, 1f);

            short px = (short)(pos.x * 1000);
            short py = (short)(pos.y * 1000);
            short pz = (short)(pos.z * 1000);

            object[] movData = new object[24];
            movData[0] = viewID;
            movData[1] = false;
            movData[2] = null;
            movData[3] = (short)40;
            movData[4] = px;
            movData[5] = py;
            movData[6] = (short)0;
            movData[7] = (short)0;
            movData[8] = (short)0;
            movData[9] = pz;
            movData[10] = (short)0;
            movData[11] = (short)0;
            movData[12] = (short)97;
            movData[13] = (short)0;
            movData[14] = (short)0;
            movData[15] = (short)10000;
            movData[16] = (byte)1;
            movData[17] = (byte)0;
            movData[18] = (byte)1;
            movData[19] = (byte)0;
            movData[20] = (byte)0;
            movData[21] = 999;
            movData[22] = enc;
            movData[23] = badQuat;

            Hashtable pkt = new Hashtable();
            pkt[(byte)10] = movData;
            pkt[(byte)0] = LoadBalancingPeer.ServerTimeInMilliSeconds;
            pkt[(byte)1] = (short)0;

            OpRaiseEvent(MOVEMENT_EVENT, pkt, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private object[] BuildMovementData(Vec3 p, Vec3 encrypted)
        {
            short sx = (short)2560;
            short sy = (short)2560;
            short sz = GetFakePing();

            return new object[]
            {
                viewID, // [0]
                false, // [1]
                null, // [2]
                (short)40, // [3] 
                (short)2560, // [4] 
                (short)2560, // [5] 
                (short)0, // [6] 
                (short)0, // [7] 
                (short)0, // [8] 
                sz, // [9]
                (short)0, // [10] 
                (short)0, // [11] 
                (short)97, // [12] 
                (short)0, // [13] 
                (short)0, // [14]
                (short)10000, // [15]
                (byte)1, // [16]
                (byte)0, // [17]
                (byte)1, // [18]
                (byte)0, // [19]
                (byte)0, // [20]
                999, // [21]
                encrypted, // [22]
                new Quat(1f, 0f, 0f, 0f) // [23]
            };
        }
    }
}
