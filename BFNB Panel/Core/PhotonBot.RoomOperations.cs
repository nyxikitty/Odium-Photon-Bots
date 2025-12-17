using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections;
namespace OdiumPhoton.Core
{
    public partial class PhotonBot
    {
        public bool JoinRandom()
        {
            LogInfo("Attempting to join random room...");
            bool result = OpJoinRandomRoom();
            if (!result)
                LogError("Failed to send join random request");
            return result;
        }
        public bool JoinRoom(string name)
        {
            LogInfo($"Joining room: {name}");
            EnterRoomParams prms = new EnterRoomParams();
            prms.RoomName = name;
            bool result = OpJoinRoom(prms);
            if (!result)
                LogError($"Couldn't join {name}");
            return result;
        }
        public bool CreateRoom(string name, string map, string mode, byte maxPlayers)
        {
            RoomOptions opts = new RoomOptions();
            opts.IsVisible = true;
            opts.IsOpen = true;
            opts.MaxPlayers = maxPlayers;
            Hashtable roomProps = new Hashtable();
            roomProps.Add("mapName", map);
            roomProps.Add("modeName", mode);
            roomProps.Add("matchStarted", false);
            roomProps.Add("gameVersion", AppVersion);
            opts.CustomRoomProperties = roomProps;
            EnterRoomParams enterParams = new EnterRoomParams();
            if (name == null || name == "")
            {
                Random r = new Random();
                enterParams.RoomName = "BotRoom (#" + r.Next(1000, 10000).ToString() + ")";
            }
            else
            {
                enterParams.RoomName = name;
            }
            enterParams.RoomOptions = opts;
            LogInfo($"Creating room '{enterParams.RoomName}' | Map: {map} | Mode: {mode} | Max: {maxPlayers}");
            bool result = OpCreateRoom(enterParams);
            if (!result)
                LogError("Room creation failed");
            return result;
        }
        public void LeaveRoom()
        {
            LogWarning("Leaving room...");
            OpLeaveRoom(false);
        }
    }
}