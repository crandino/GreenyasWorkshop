using Hexalinks.Tile;
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
            NorthEast = 1,
            SouthEast = 2,
            South = 3,
            SouthWest = 4,
            NorthWest = 5
        }        

        private static class Convertor
        {
            private readonly static Side[] Sides = new Side[]
            {
                Side.North,
                Side.NorthEast,
                Side.SouthEast,
                Side.South,
                Side.SouthWest,
                Side.NorthWest
            };

            public static Side GetWorldSide(Side localSide, Transform transform)
            {
                int rotationSteps = (int)transform.eulerAngles.y / 60;
                return GetWorldSideAfterRotStep(localSide, rotationSteps);
            }      
        }

        [SerializeField]
        private Side localSide;

        public Side GetWorldSide(TileSegment segment) => Convertor.GetWorldSide(localSide, segment.transform);

        //public void RotateClockwise()
        //{
        //    localSide = GetWorldSideAfterRotStep(localSide, 1);
        //}

        //public void RotateCounterClockwise()
        //{
        //    localSide = GetWorldSideAfterRotStep(localSide, -1);
        //}

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
