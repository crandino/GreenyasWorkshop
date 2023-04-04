using UnityEngine;

public static class VectorExtensions
{
    public static void ZeroX(ref this Vector3 v)
    {
        v.x = 0f;
    }

    public static void ZeroY(ref this Vector3 v)
    {
        v.y = 0f;
    }

    public static void ZeroZ(ref this Vector3 v)
    {
        v.z = 0f;
    }

    public static void ZeroXY(ref this Vector3 v)
    {
        v.ZeroX();
        v.ZeroY();
    }

    public static void ZeroYZ(ref this Vector3 v)
    {
        v.ZeroY();
        v.ZeroZ();
    }

    public static void ZeroXZ(ref this Vector3 v)
    {
        v.ZeroX();
        v.ZeroZ();
    }

    public static Vector3 GetZeroX(this Vector3 v)
    {
        return new Vector3(0f, v.y, v.z);
    }

    public static Vector3 GetZeroY(this Vector3 v)
    {
        return new Vector3(v.x, 0f, v.z);
    }

    public static Vector3 GetZeroZ(this Vector3 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    public static Vector3 GetZeroXY(this Vector3 v)
    {
        Vector3 u = v.GetZeroX();
        u.ZeroY();
        return u;
    }

    public static Vector3 GetZeroYZ(this Vector3 v)
    {
        Vector3 u = v.GetZeroY();
        u.ZeroZ();
        return u;
    }

    public static Vector3 GetZeroXZ(this Vector3 v)
    {
        Vector3 u = v.GetZeroX();
        u.ZeroZ();
        return u;
    }

    public static void SetX(ref this Vector3 v, float value)
    {
        v.x = value;
    }

    public static void SetY(ref this Vector3 v, float value)
    {
        v.y = value;
    }

    public static void SetZ(ref this Vector3 v, float value)
    {
        v.z = value;
    }

    public static Vector2 GetXY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 GetXZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 GetYZ(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }
}
