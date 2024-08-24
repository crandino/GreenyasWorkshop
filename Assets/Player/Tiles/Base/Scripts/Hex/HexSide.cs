using Newtonsoft.Json.Linq;
using UnityEditor;
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

        // TODO 2024-08-22
        /*
         * El paso del local a global del Side de los hexágonos no remapea bien el ángulo
         * Una vez funcione, volver al tema de conexión de los hexágonos.
         * Ver que se guardan y conectan bien los hexágonos.
         * Yo añadiría una pieza con bifurcaciones para trabajar con dos segmentos en la misma pieza
         * He eliminado un montón de mierda así que espero que el Debug de los lados del hexágono siga funcioanndo
         * 
         */

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

                //float remappedAngle = Math.Remap(transform.eulerAngles.y, -180, 180, 0, 360);
                //Debug.Log(remappedAngle);
                //int index = (int)((remappedAngle / 60) + (int)localSide);
                //return (Side)((int)Sides[index] % TOTAL_SIDES);               
            }

            //private static Quaternion[] angleRotations = new Quaternion[6]
            //{
            //    new Quaternion(0f, 0f, 0f, 1f),          // 0
            //    new Quaternion(0f, 0.5f, 0f, 0.86603f),  // 60
            //    new Quaternion(0f, 0.86603f, 0f, 0.5f),  // 120
            //    new Quaternion(0f, 1f, 0f, 0f),          // 180
            //    new Quaternion(0f, 0.86603f, 0f, -0.5f), // 240
            //    new Quaternion(0f, 0.5f, 0f, -0.86603f)  // 300
            //};

            //public static Side GetWorldSide(Side localSide, Transform transform)
            //{
            //    float maxDot = 0;
            //    int numRotations = 0;

            //    for (int i = 0; i < angleRotations.Length; i++)
            //    {
            //        float dot = Mathf.Abs(Quaternion.Dot(transform.rotation, angleRotations[i]));
            //        if (dot > maxDot)
            //        {
            //            maxDot = dot;
            //            numRotations = i;
            //        }
            //    }

            //    return (Side)(((int)localSide + numRotations) % TOTAL_SIDES);
            //}
        }

        [SerializeField]
        private Side localSide;

        //[SerializeField]
        //private Transform tileTransform;

        public Side GetWorldSide(Transform tileTransform) => Convertor.GetWorldSide(localSide, tileTransform);

        public void RotateClockwise()
        {
            localSide = GetWorldSideAfterRotStep(localSide, 1);
        }

        public void RotateCounterClockwise()
        {
            localSide = GetWorldSideAfterRotStep(localSide, -1);
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
