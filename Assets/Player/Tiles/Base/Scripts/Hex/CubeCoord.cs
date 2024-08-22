using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;

namespace Greenyas.Hexagon
{
    [System.Serializable]
    public class CubeCoord
    {
        [SerializeField]
        private int r, q, s;

        public int R => r;
        public int Q => q;
        public int S => s;

        public readonly static CubeCoord Origin = new(0, 0, 0);

        public CubeCoord(int q, int r, int s)
        {
            Assert.IsTrue((r + q + s) == 0, "Wrong cube coordinates. Unfulfilled (r + q + s = 0) restriction");
            this.r = r;
            this.q = q;
            this.s = s;
        }

        public CubeCoord(int q, int r) : this(q, r, -r - q)
        { }

        public CubeCoord(CubeCoord coord)
        {
            r = coord.r;
            q = coord.q;
            s = coord.s;
        }

        public static CubeCoord operator +(CubeCoord coordA, CubeCoord coordB)
        {
            coordA.r += coordB.r;
            coordA.q += coordB.q;
            coordA.s += coordB.s;

            return coordA;
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

    [CustomPropertyDrawer(typeof(CubeCoord))]
    public class CubeCoordPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var r = property.FindPropertyRelative("r");
            var q = property.FindPropertyRelative("q");
            var s = property.FindPropertyRelative("s");

            EditorGUI.LabelField(position, $"(R,Q,S) -> ({r.intValue},{q.intValue},{s.intValue})");
        }
    }

}