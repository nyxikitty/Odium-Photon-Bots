[Serializable]
public struct Vec3
{
    public float x, y, z;
    public Vec3(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }
}

[Serializable]
public struct Quat
{
    public float x, y, z, w;
    public Quat(float x, float y, float z, float w)
    {
        this.x = x; this.y = y; this.z = z; this.w = w;
    }
}
