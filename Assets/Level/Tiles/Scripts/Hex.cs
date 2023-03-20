using Greenyas.VectorExtensions.Swizzle;
using UnityEngine;
using UnityEngine.Assertions;

public class Hex
{
    private readonly static float hexagonSize = 0.5f;

    public int R { private set; get; }
    public int Q { private set; get; }
    public int S { private set; get; }

    public Hex(int q, int r) : this(q, r, -r - q)
    { }

    public Hex(int q, int r, int s)
    {
        Assert.IsTrue((r + q + s) == 0, "Wrong cube coordinates. Unfulfilled (r + q + s = 0) restriction");
        R = r;
        Q = q;
        S = s;
    }

    public Vector2 Get2DCartesianWorldPos()
    {
        float x = hexagonSize * (3f / 2) * Q;
        float y = hexagonSize * (Mathf.Sqrt(3f) / 2 * Q + Mathf.Sqrt(3f) * R);
        return new Vector2(x, y);
    }

    public Vector3 Get3DCartesianWorldPos(float defaultYValue = 0)
    {
        return Get2DCartesianWorldPos().SwizzleX_Y(defaultYValue);
    }

    public static Hex GetNearestHex(Vector3 from)
    {
        float fracQ = ((2f / 3) * from.x) / hexagonSize;
        float fracR = ((-1f / 3) * from.x + Mathf.Sqrt(3f) / 3 * from.z) / hexagonSize;

        FractionalHex fracHex = new FractionalHex(fracQ, fracR);
        return fracHex.RoundCubeCoordinates();
    }

    private class FractionalHex 
    {
        public float fracR { private set; get; }
        public float fracQ { private set; get; }
        public float fracS { private set; get; }

        public FractionalHex(float q, float r) 
        {
            fracQ = q;
            fracR = r;
            fracS = -fracQ - fracR;
        }

        public Hex RoundCubeCoordinates()
        {
            int q = Mathf.RoundToInt(fracQ);
            int r = Mathf.RoundToInt(fracR);
            int s = Mathf.RoundToInt(fracS);

            float qDiff = Mathf.Abs(q - fracQ);
            float rDiff = Mathf.Abs(r - fracR);
            float sDiff = Mathf.Abs(s - fracS);

            if (qDiff > rDiff && qDiff > sDiff)
                q = -r - s;
            else if (rDiff < sDiff)
                r = -q - s;
            else
                s = -q - r;

            return new Hex(q, r, s);
        }
    }
}
