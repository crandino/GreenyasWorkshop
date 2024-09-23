using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;

namespace Greenyas.Hexagon
{
    //https://www.redblobgames.com/grids/hexagons/

    [System.Serializable]
    public struct CubeCoord
    {
        [SerializeField]
        private int r, q, s;

        public readonly int Q => q;
        public readonly int R => r;
        public readonly int S => s;

        public readonly static CubeCoord Origin = new(0, 0, 0);

        public CubeCoord(int q, int r, int s)
        {
            Assert.IsTrue((q + r + s) == 0, "Wrong cube coordinates. Unfulfilled (r + q + s = 0) restriction");
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public CubeCoord(int q, int r) : this(q, r, -r - q)
        { }       

        public static CubeCoord operator +(CubeCoord coordA, CubeCoord coordB)
        {
            return new CubeCoord(coordA.q + coordB.q, coordA.r + coordB.r, coordA.s + coordB.s);
        }

        public override string ToString()
        {
            return $"(Q,R,S)=({q},{r},{s})";
        }

        public class CoordinateComparer : IEqualityComparer<CubeCoord>
        {
            public bool Equals(CubeCoord coordA, CubeCoord coordB)
            {
                return coordA.Q == coordB.Q &&
                       coordA.R == coordB.R &&
                       coordA.S == coordB.S;
            }

            public int GetHashCode(CubeCoord obj)
            {
                return 0;
            }
        }

        private readonly static CubeCoord[] neighborCoords = new CubeCoord[]
        {
            new CubeCoord( 0, -1,  1), // north
            new CubeCoord( 1, -1,  0), // north-east
            new CubeCoord( 1,  0, -1), // south-east
            new CubeCoord( 0,  1, -1), // south
            new CubeCoord(-1,  1,  0), // south-west
            new CubeCoord(-1 , 0,  1)  // north-west
        };

        public static CubeCoord GetToNeighborCoord(Side hexSide)
        {
            return neighborCoords[(int)hexSide];
        }

        public static Vector2 GetVectorToNeighborHexOn(Side side)
        {
            CubeCoord coord = GetToNeighborCoord(side);

            return new Vector2()
            {
                x = HexTools.hexagonSize * (3f / 2) * coord.Q,
                y = -HexTools.hexagonSize * ((Mathf.Sqrt(3f) / 2) * coord.Q + Mathf.Sqrt(3f) * coord.R)
            };
        }

        private class FractionalCubeCoord
        {
            public float FracQ { private set; get; }
            public float FracR { private set; get; }
            public float FracS { get => -FracQ - FracR; }

            public FractionalCubeCoord(float q, float r)
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
            float fracQ = ((2f / 3) * cartesianFrom.x) / HexTools.hexagonSize;
            float fracR = ((-1f / 3) * cartesianFrom.x + (Mathf.Sqrt(3f) / 3) * -cartesianFrom.z) / HexTools.hexagonSize;

            FractionalCubeCoord fracHex = new FractionalCubeCoord(fracQ, fracR);
            return fracHex.RoundCubeCoordinates();
        }
    }
}