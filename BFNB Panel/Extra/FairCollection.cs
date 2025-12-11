using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

public struct Vec2 { public float x, y; public Vec2(float x, float y) { this.x = x; this.y = y; } }
public struct Vec4 { public float x, y, z, w; public Vec4(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; } }

public static class FairCollection
{
    private const string WEB_ADDRESS = "https://server.blayzegames.com/OnlineAccountSystem/fairplay_spec.php";

    // ts hasn't been updated in years
    private const string MAGIC = "1983031920131006";
    private const int SEC_SIZE = 16;

    private static int off1 = 0;
    private static int off2 = 0;
    private static byte[] sec1 = new byte[SEC_SIZE];
    private static byte[] sec2 = new byte[SEC_SIZE];
    private static string response = string.Empty;
    private static bool enabled = false;

    public static async Task<string> InitOperationAsync()
    {
        if (enabled) return response;

        await InitRequestAsync();
        InitData();
        return response;
    }

    private static async Task InitRequestAsync()
    {
        try
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent($"magic={Uri.EscapeDataString(MAGIC)}", Encoding.UTF8, "application/x-www-form-urlencoded");
                var httpResp = await client.PostAsync(WEB_ADDRESS, content).ConfigureAwait(false);

                if (!httpResp.IsSuccessStatusCode)
                {
                    throw new Exception($"HTTP error: {httpResp.StatusCode} - {httpResp.ReasonPhrase}");
                }

                response = await httpResp.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {

            throw new Exception("FairCollection initialization failed: " + ex.Message, ex);
        }
    }

    private static void InitData()
    {
        byte[] bytes = HexStringToByteArray(response);

        if (bytes.Length < 5 || bytes[1] != 0) return;

        off1 = bytes[3];
        off2 = bytes[4];

        if (bytes.Length < 5 + SEC_SIZE * 2)
        {

            return;
        }

        Array.Copy(bytes, 5, sec1, 0, SEC_SIZE);
        Array.Copy(bytes, 5 + SEC_SIZE, sec2, 0, SEC_SIZE);

        enabled = true;
    }

    private static byte[] HexStringToByteArray(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return new byte[0];
        if (hex.Length % 2 != 0) throw new ArgumentException("Hex string must have even length");
        var outBytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
            outBytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return outBytes;
    }

    private static void TransformBuffer(byte[] buffer, byte[] sec, int off)
    {
        int k = 0;
        int secLen = sec.Length;
        for (int i = 0; i < buffer.Length; ++i)
        {
            int secIndex = off + (k >> 1);

            byte s = sec[secIndex % secLen];

            if ((k & 1) != 0)
            {
                buffer[i] ^= (byte)(s >> 4);
            }
            else
            {
                buffer[i] ^= (byte)(s & 0xF);
            }

            ++k;
            if (k >= secLen) k = 0;
        }
    }

    public static double GetEncryptedDouble(double value)
    {
        if (!enabled) return value;
        byte[] b = BitConverter.GetBytes(value);

        TransformBuffer(b, sec1, off1);
        return BitConverter.ToDouble(b, 0);
    }

    public static double GetDecryptedDouble(double value)
    {
        if (!enabled) return value;
        byte[] b = BitConverter.GetBytes(value);
        TransformBuffer(b, sec2, off2);
        return BitConverter.ToDouble(b, 0);
    }

    public static float GetEncryptedFloat(float value)
    {
        if (!enabled) return value;
        byte[] b = BitConverter.GetBytes(value);
        TransformBuffer(b, sec1, off1);
        return BitConverter.ToSingle(b, 0);
    }

    public static float GetDecryptedFloat(float value)
    {
        if (!enabled) return value;
        byte[] b = BitConverter.GetBytes(value);
        TransformBuffer(b, sec2, off2);
        return BitConverter.ToSingle(b, 0);
    }

    public static int GetEncryptedInteger(int value)
    {
        if (!enabled) return value;
        byte[] b = BitConverter.GetBytes(value);
        TransformBuffer(b, sec1, off1);
        return BitConverter.ToInt32(b, 0);
    }

    public static int GetDecryptedInteger(int value)
    {
        if (!enabled) return value;
        byte[] b = BitConverter.GetBytes(value);
        TransformBuffer(b, sec2, off2);
        return BitConverter.ToInt32(b, 0);
    }

    public static string GetEncryptedString(string value)
    {
        if (!enabled) return value;
        byte[] b = Encoding.UTF8.GetBytes(value);
        TransformBuffer(b, sec1, off1);
        return Encoding.UTF8.GetString(b);
    }

    public static string GetDecryptedString(string value)
    {
        if (!enabled) return value;
        byte[] b = Encoding.UTF8.GetBytes(value);
        TransformBuffer(b, sec2, off2);
        return Encoding.UTF8.GetString(b);
    }

    public static Vec2 GetEncryptedVector2(Vec2 v)
    {
        if (!enabled) return v;
        return new Vec2(
            GetEncryptedFloat(v.x),
            GetEncryptedFloat(v.y)
        );
    }

    public static Vec2 GetDecryptedVector2(Vec2 v)
    {
        if (!enabled) return v;
        return new Vec2(
            GetDecryptedFloat(v.x),
            GetDecryptedFloat(v.y)
        );
    }

    public static Vec3 GetEncryptedVector3(Vec3 v)
    {
        if (!enabled) return v;
        return new Vec3(
            GetEncryptedFloat(v.x),
            GetEncryptedFloat(v.y),
            GetEncryptedFloat(v.z)
        );
    }

    public static Vec3 GetDecryptedVector3(Vec3 v)
    {
        if (!enabled) return v;
        return new Vec3(
            GetDecryptedFloat(v.x),
            GetDecryptedFloat(v.y),
            GetDecryptedFloat(v.z)
        );
    }

    public static Vec4 GetEncryptedVector4(Vec4 v)
    {
        if (!enabled) return v;
        return new Vec4(
            GetEncryptedFloat(v.x),
            GetEncryptedFloat(v.y),
            GetEncryptedFloat(v.z),
            GetEncryptedFloat(v.w)
        );
    }

    public static Vec4 GetDecryptedVector4(Vec4 v)
    {
        if (!enabled) return v;
        return new Vec4(
            GetDecryptedFloat(v.x),
            GetDecryptedFloat(v.y),
            GetDecryptedFloat(v.z),
            GetDecryptedFloat(v.w)
        );
    }

    public static Quat GetEncryptedQuat(Quat q)
    {
        if (!enabled) return q;
        return new Quat(
            GetEncryptedFloat(q.x),
            GetEncryptedFloat(q.y),
            GetEncryptedFloat(q.z),
            GetEncryptedFloat(q.w)
        );
    }

    public static Quat GetDecryptedQuat(Quat q)
    {
        if (!enabled) return q;
        return new Quat(
            GetDecryptedFloat(q.x),
            GetDecryptedFloat(q.y),
            GetDecryptedFloat(q.z),
            GetDecryptedFloat(q.w)
        );
    }
}

