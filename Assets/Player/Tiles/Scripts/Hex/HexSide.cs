using HexaLinks.Tile;
using UnityEngine;

namespace Greenyas.Hexagon
{
    [System.Serializable]
    public class HexSide
    {
        public const int TOTAL_SIDES = 6;

        public enum Side
        {
            North = 0,
            NorthEast,
            SouthEast,
            South,
            SouthWest,
            NorthWest
        }

        [SerializeField]
        private Side localSide;
        public Side GetWorldSide(TileSegment segment) => Convertor.GetWorldSide(localSide, segment.transform);

        public static class Convertor
        {
            public static Side GetWorldSide(Side localSide, Transform transform)
            {
                int rotationSteps = (int)transform.eulerAngles.y / 60;
                return GetWorldSideAfterRotStep(localSide, rotationSteps);
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
        }
    }
}
