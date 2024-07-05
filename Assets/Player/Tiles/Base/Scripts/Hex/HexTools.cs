using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Greenyas.Hexagon
{
	public static class HexTools
	{
        public static readonly float hexagonSize = 0.5f;
        public const int ROTATION_ANGLE = 60;

        public static Vector3 GetGridCartesianWorldPos(Vector3 worldPos)
        {
            return GetGridCartesianWorldPos(GetNearestCubeCoord(worldPos));
        }


        public static Vector3 GetGridCartesianWorldPos(CubeCoord hexCoord)
        {
            float x = hexagonSize * (3f / 2) * hexCoord.Q;
            float y = hexagonSize * ((Mathf.Sqrt(3f) / 2) * hexCoord.Q + Mathf.Sqrt(3f) * hexCoord.R);
            return new Vector3(x, 0f, y);
        }

        private class FractionalHex
        {
            public float FracR { private set; get; }
            public float FracQ { private set; get; }
            public float FracS { get => -FracQ - FracR; }

            public FractionalHex(float q, float r)
            {
                FracQ = q;
                FracR = r;
            }

            public CubeCoord RoundCubeCoordinates()
            {
                int q = Mathf.RoundToInt(FracQ);
                int r = Mathf.RoundToInt(FracR);
                int s = Mathf.RoundToInt(FracS);

                float qDiff = Mathf.Abs(q - FracQ);
                float rDiff = Mathf.Abs(r - FracR);
                float sDiff = Mathf.Abs(s - FracS);

                if (qDiff > rDiff && qDiff > sDiff)
                    q = -r - s;
                else if (rDiff < sDiff)
                    r = -q - s;
                else
                    s = -q - r;

                return new CubeCoord(q, r, s);
            }
        }

        public static CubeCoord GetNearestCubeCoord(Vector3 cartesianFrom)
        {
            float fracQ = ((2f / 3) * cartesianFrom.x) / hexagonSize;
            float fracR = ((-1f / 3) * cartesianFrom.x + (Mathf.Sqrt(3f) / 3) * cartesianFrom.z) / hexagonSize;

            FractionalHex fracHex = new FractionalHex(fracQ, fracR);
            return fracHex.RoundCubeCoordinates();
        }
    } 
}
