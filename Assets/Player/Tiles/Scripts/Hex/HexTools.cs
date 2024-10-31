using UnityEngine;

namespace Greenyas.Hexagon
{
    public static class HexTools
	{
        public static readonly float hexagonSize = 0.5f;
        public const int ROTATION_ANGLE = 60;

        public static Vector3 GetHexCenterCartesianWorldPos(Vector3 worldPos)
        {
            return GetGridCartesianWorldPos(CubeCoord.GetNearestCubeCoord(worldPos));
        }

        public static Vector3 GetGridCartesianWorldPos(CubeCoord hexCoord)
        {
            float x = hexagonSize * (3f / 2) * hexCoord.Q;
            float y = hexagonSize * ((Mathf.Sqrt(3f) / 2) * hexCoord.Q + Mathf.Sqrt(3f) * hexCoord.R);
            return new Vector3(x, 0f, -y);
        }

        public static float Dot(CubeCoord hexCoord1, CubeCoord hexCoord2)
        {
            return Vector3.Dot(GetGridCartesianWorldPos(hexCoord1).normalized, GetGridCartesianWorldPos(hexCoord2).normalized);
        }


    } 
}
