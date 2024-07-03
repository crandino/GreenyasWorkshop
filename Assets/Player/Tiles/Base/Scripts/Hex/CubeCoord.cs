using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;

namespace Greenyas.Hexagon
{
    public class CubeCoord
    {
        public int R { private set; get; }
        public int Q { private set; get; }
        public int S { get => -R - Q; }

        public CubeCoord(int q, int r, int s)
        {
            Assert.IsTrue((r + q + s) == 0, "Wrong cube coordinates. Unfulfilled (r + q + s = 0) restriction");
            R = r;
            Q = q;
        }

        public CubeCoord(int q, int r) : this(q, r, -r - q)
        { }

        public static CubeCoord operator +(CubeCoord coordA, CubeCoord coordB)
        {
            return new CubeCoord(coordA.Q + coordB.Q, coordA.R + coordB.R, coordA.S + coordB.S);
        }

        public static CubeCoord operator -(CubeCoord coordA, CubeCoord coordB)
        {
            return new CubeCoord(coordA.Q - coordB.Q, coordA.R - coordB.R, coordA.S - coordB.S);
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
                return obj.GetHashCode();
            }
        }

        private readonly static CubeCoord[] neighborCoords = new CubeCoord[]
        {
            new CubeCoord( 0,  1, -1), // north
            new CubeCoord( 1,  0, -1), // north-east
            new CubeCoord( 1, -1,  0), // south-east
            new CubeCoord( 0, -1,  1), // south
            new CubeCoord(-1,  0,  1), // south-west
            new CubeCoord(-1 , 1,  0)  // north-west
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
                y = HexTools.hexagonSize * ((Mathf.Sqrt(3f) / 2) * coord.Q + Mathf.Sqrt(3f) * coord.R)
            };
        }
    }

}