using ExitGames.Client.Photon;
using System;

public static class CustomTypes
{
    public static void Register()
    {
        PhotonPeer.RegisterType(typeof(Vec3), (byte)'V', SerializeVec3, DeserializeVec3);
        PhotonPeer.RegisterType(typeof(Quat), (byte)'Q', SerializeQuat, DeserializeQuat);
    }

    private static short SerializeVec3(StreamBuffer outStream, object customobject)
    {
        Vec3 v = (Vec3)customobject;
        byte[] bytes = new byte[12];
        int idx = 0;

        Protocol.Serialize(v.x, bytes, ref idx);
        Protocol.Serialize(v.y, bytes, ref idx);
        Protocol.Serialize(v.z, bytes, ref idx);

        outStream.Write(bytes, 0, 12);
        return 12;
    }

    private static object DeserializeVec3(StreamBuffer inStream, short length)
    {
        byte[] bytes = new byte[12];
        inStream.Read(bytes, 0, 12);

        int idx = 0;
        Protocol.Deserialize(out float x, bytes, ref idx);
        Protocol.Deserialize(out float y, bytes, ref idx);
        Protocol.Deserialize(out float z, bytes, ref idx);

        return new Vec3(x, y, z);
    }

    private static short SerializeQuat(StreamBuffer outStream, object customobject)
    {
        Quat q = (Quat)customobject;
        byte[] buffer = new byte[16];
        int idx = 0;

        Protocol.Serialize(q.x, buffer, ref idx);
        Protocol.Serialize(q.y, buffer, ref idx);
        Protocol.Serialize(q.z, buffer, ref idx);
        Protocol.Serialize(q.w, buffer, ref idx);

        outStream.Write(buffer, 0, 16);
        return 16;
    }

    private static object DeserializeQuat(StreamBuffer inStream, short length)
    {
        byte[] buffer = new byte[16];
        inStream.Read(buffer, 0, 16);

        int idx = 0;
        Protocol.Deserialize(out float x, buffer, ref idx);
        Protocol.Deserialize(out float y, buffer, ref idx);
        Protocol.Deserialize(out float z, buffer, ref idx);
        Protocol.Deserialize(out float w, buffer, ref idx);

        return new Quat(x, y, z, w);
    }
}