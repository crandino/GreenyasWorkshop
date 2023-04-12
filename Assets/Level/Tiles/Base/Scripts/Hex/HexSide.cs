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

        [SerializeField]
        private Side initialLocalSide;

        public Side WorldSide => initialLocalSide;

        public void RotateClockwise()
        {
            initialLocalSide = GetWorldSideAfterRotStep(initialLocalSide, 1);
        }

        public void RotateCounterClockwise()
        {
            initialLocalSide = GetWorldSideAfterRotStep(initialLocalSide, -1);
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
