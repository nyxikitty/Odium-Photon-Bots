using ExitGames.Client.Photon;
using System;

public static class PhotonCustomTypes
{
    public static void Register()
    {
        PhotonPeer.RegisterType(typeof(Vec3), (byte)'V',
            SerializeVec3, DeserializeVec3);

        PhotonPeer.RegisterType(typeof(Quat), (byte)'Q',
            SerializeQuat, DeserializeQuat);
    }

    // ----------- VEC3 SERIALIZATION -----------

    private static short SerializeVec3(StreamBuffer outStream, object customobject)
    {
        Vec3 v = (Vec3)customobject;

        byte[] bytes = new byte[12];
        int index = 0;

        Protocol.Serialize(v.x, bytes, ref index);
        Protocol.Serialize(v.y, bytes, ref index);
        Protocol.Serialize(v.z, bytes, ref index);

        outStream.Write(bytes, 0, 12);
        return 12;
    }

    private static object DeserializeVec3(StreamBuffer inStream, short length)
    {
        byte[] bytes = new byte[12];
        inStream.Read(bytes, 0, 12);

        int index = 0;

        float x, y, z;
        Protocol.Deserialize(out x, bytes, ref index);
        Protocol.Deserialize(out y, bytes, ref index);
        Protocol.Deserialize(out z, bytes, ref index);

        return new Vec3(x, y, z);
    }


    // ----------- QUAT SERIALIZATION -----------

    private static short SerializeQuat(StreamBuffer outStream, object customobject)
    {
        Quat q = (Quat)customobject;

        byte[] bytes = new byte[16];
        int index = 0;

        Protocol.Serialize(q.x, bytes, ref index);
        Protocol.Serialize(q.y, bytes, ref index);
        Protocol.Serialize(q.z, bytes, ref index);
        Protocol.Serialize(q.w, bytes, ref index);

        outStream.Write(bytes, 0, 16);
        return 16;
    }

    private static object DeserializeQuat(StreamBuffer inStream, short length)
    {
        byte[] bytes = new byte[16];
        inStream.Read(bytes, 0, 16);

        int index = 0;

        float x, y, z, w;
        Protocol.Deserialize(out x, bytes, ref index);
        Protocol.Deserialize(out y, bytes, ref index);
        Protocol.Deserialize(out z, bytes, ref index);
        Protocol.Deserialize(out w, bytes, ref index);

        return new Quat(x, y, z, w);
    }
}
