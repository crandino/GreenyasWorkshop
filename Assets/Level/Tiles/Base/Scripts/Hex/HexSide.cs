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
            private static Quaternion[] angleRotations = new Quaternion[6]
            {
                new Quaternion(0f, 0f, 0f, 1f),          // 0
                new Quaternion(0f, 0.5f, 0f, 0.86603f),  // 60
                new Quaternion(0f, 0.86603f, 0f, 0.5f),  // 120
                new Quaternion(0f, 1f, 0f, 0f),          // 180
                new Quaternion(0f, 0.86603f, 0f, -0.5f), // 240
                new Quaternion(0f, 0.5f, 0f, -0.86603f)  // 300
            };

            public static Side GetWorldSide(Side localSide, Transform transform)
            {
                float maxDot = 0;
                int numRotations = 0;

                for (int i = 0; i < angleRotations.Length; i++)
                {
                    float dot = Mathf.Abs(Quaternion.Dot(transform.rotation, angleRotations[i]));
                    if (dot > maxDot)
                    {
                        maxDot = dot;
                        numRotations = i;
                    }
                }

                return (Side)(((int)localSide + numRotations) % TOTAL_SIDES);
            }
        }

        [SerializeField]
        private Side initialLocalSide;

        [SerializeField]
        private Transform tileTransform;

        public Side WorldSide => Convertor.GetWorldSide(initialLocalSide, tileTransform);

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
