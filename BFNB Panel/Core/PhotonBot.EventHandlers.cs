using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
namespace OdiumPhoton.Core
{
    public partial class PhotonBot
    {
        private void OnEventReceived(EventData photonEvent)
        {
            if (photonEvent.Code != MOVEMENT_EVENT)
                return;
            try
            {
                if (photonEvent.CustomData is Hashtable)
                {
                    Hashtable data = (Hashtable)photonEvent.CustomData;
                    if (data.ContainsKey((byte)10))
                    {
                        object[] movData = data[(byte)10] as object[];
                        if (movData != null && movData.Length >= 23)
                        {
                            int vID = (int)movData[0];
                            Vec3 encPos = (Vec3)movData[22];
                            int actorNum = vID / 1000;
                            Vec3 decPos = FairCollection.GetDecryptedVector3(encPos);
                            lock (posLock)
                            {
                                playerPositions[actorNum] = decPos;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error parsing movement event: {ex.Message}");
            }
            if (photonEvent.Sender == LocalPlayer.ActorNumber)
            {
                return;
            }
            if (photonEvent.CustomData is Hashtable)
            {
                Hashtable data = (Hashtable)photonEvent.CustomData;
                if (data.ContainsKey((byte)10))
                {
                    object[] movData = data[(byte)10] as object[];
                    if (movData != null && movData.Length >= 23)
                    {
                        int vID = (int)movData[0];
                        Vec3 encPos = (Vec3)movData[22];
                        int actorNum = vID / 1000;
                        Vec3 decPos = FairCollection.GetDecryptedVector3(encPos);
                        lock (posLock)
                        {
                            playerPositions[actorNum] = decPos;
                        }
                        // LogInfo($"Actor {actorNum} -> ({decPos.x:F2}, {decPos.y:F2}, {decPos.z:F2})");
                    }
                }
            }
        }
        private void OnStateChange(ClientState prev, ClientState curr)
        {
            LogInfo($"{prev} -> {curr}");
            if (curr == ClientState.Joined)
            {
                HandleRoomJoined();
            }
            else if (curr == ClientState.Disconnected)
            {
                OnDisconnected();
            }
        }
        private void OnDisconnected()
        {
            hasSpawned = false;
            isActive = false;
            viewID = 0;
            rpcs = null;
            lock (posLock)
            {
                playerPositions.Clear();
            }
            LogWarning("Cleanup complete, bot reset");
        }
    }
}