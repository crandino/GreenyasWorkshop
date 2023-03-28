using UnityEngine;

namespace Greenyas.Hexagon
{
    [System.Serializable]
    public class HexSide
    {
        private readonly static CubeCoord[] sideNeighborCoords = new CubeCoord[]
        {
        new CubeCoord( 0,  1, -1), // north
        new CubeCoord( 1,  0, -1), // north-east
        new CubeCoord( 1, -1,  0), // south-east
        new CubeCoord( 0, -1,  1), // south
        new CubeCoord(-1,  0,  1), // south-west
        new CubeCoord(-1 , 1,  0)  // north-west
        };

        private const int TOTAL_SIDES = 6;

        public enum Side
        {
            North = 0,
            NorthEast = 1,
            SouthEast = 2,
            South = 3,
            SouthWest = 4,
            NorthWest = 5
        }

        [SerializeField]
        private Side initialLocalSide;

        [SerializeField]
        private Tile parentTile;

        public Side WorldSide => GetWorldSideAfterRotStep(initialLocalSide, parentTile.AccumulatedRotationAngle / Tile.ROTATION_ANGLE);
        public Side OppositeWorldSide => GetWorldSideAfterRotStep(initialLocalSide, (parentTile.AccumulatedRotationAngle + 180) / Tile.ROTATION_ANGLE);

        public CubeCoord GetNeighborCoordOnWorldSide()
        {
            return sideNeighborCoords[(int)WorldSide] + parentTile.HexCoord;
        }

        public static Side GetWorldSideAfterRotStep(Side localSide, int rotationSteps = 0)
        {
            if (rotationSteps > 0)
                return GetWorldSideAfterPositiveRotStep(localSide, rotationSteps);
            else if (rotationSteps < 0)
                return GetWorldSideAfterNegativeRotStep(localSide, rotationSteps);
            else
                return localSide;
        }

        private static Side GetWorldSideAfterPositiveRotStep(Side localSide, int rotationSteps)
        {
            rotationSteps %= TOTAL_SIDES;
            localSide += rotationSteps;
            return (Side)((int)localSide % TOTAL_SIDES);
        }

        private static Side GetWorldSideAfterNegativeRotStep(Side localSide, int rotationSteps)
        {
            rotationSteps = -rotationSteps % TOTAL_SIDES;
            localSide -= rotationSteps;
            if (localSide < Side.North)
                return (Side)TOTAL_SIDES - rotationSteps;
            return localSide;
        }

        public static Vector2 GetVectorToNeighborHexOn(Side side)
        {
            CubeCoord coord = sideNeighborCoords[(int)side];

            return new Vector2()
            {
                x = HexTools.hexagonSize * (3f / 2) * coord.Q,
                y = HexTools.hexagonSize * ((Mathf.Sqrt(3f) / 2) * coord.Q + Mathf.Sqrt(3f) * coord.R)
            };
        }
    }
}
