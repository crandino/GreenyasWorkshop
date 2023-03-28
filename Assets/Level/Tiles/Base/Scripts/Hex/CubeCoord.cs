using System.Collections.Generic;
using UnityEngine.Assertions;

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
                return 0;
            }

        }
    }

}