using System;

// Had to write custom Vec3 struct for Photon serialization, UnityEngine.dll gave me issues when importing.
[Serializable]
public struct Vec3
{
    public float x, y, z;

    public Vec3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vec3 operator +(Vec3 a, Vec3 b)
    {
        return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Vec3 operator -(Vec3 a, Vec3 b)
    {
        return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vec3 operator *(Vec3 v, float scalar)
    {
        return new Vec3(v.x * scalar, v.y * scalar, v.z * scalar);
    }

    public static Vec3 operator *(float scalar, Vec3 v)
    {
        return new Vec3(v.x * scalar, v.y * scalar, v.z * scalar);
    }

    public static Vec3 operator /(Vec3 v, float scalar)
    {
        return new Vec3(v.x / scalar, v.y / scalar, v.z / scalar);
    }

    public static Vec3 operator -(Vec3 v)
    {
        return new Vec3(-v.x, -v.y, -v.z);
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt(x * x + y * y + z * z);
    }

    public Vec3 Normalized()
    {
        float mag = Magnitude();
        if (mag > 0.00001f)
            return this / mag;
        return new Vec3(0, 0, 0);
    }

    public static float Distance(Vec3 a, Vec3 b)
    {
        return (a - b).Magnitude();
    }
}