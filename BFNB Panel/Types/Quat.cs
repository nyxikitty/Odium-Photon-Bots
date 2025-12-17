using System;

// Had to write custom Quat struct for Photon serialization, UnityEngine.dll gave me issues when importing.
[Serializable]
public struct Quat
{
    public float x, y, z, w;

    public Quat(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}