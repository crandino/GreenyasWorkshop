using HexaLinks.Tile;
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
            //private readonly static Side[] Sides = new Side[]
            //{
            //    Side.North,
            //    Side.NorthEast,
            //    Side.SouthEast,
            //    Side.South,
            //    Side.SouthWest,
            //    Side.NorthWest
            //};

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

    [CustomPropertyDrawer(typeof(HexSide))]
    public class CubeCoordPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowHexSideCarousel(position, property);
        }

        //public override void OnInspectorGUI()
        //{
        //    ShowHexSideCarousel()
        //    serializedObject.Update();

        //    //EditorGUILayout.ObjectField(properties.meshRenderer);

        //    ++EditorGUI.indentLevel;
        //    for (int i = 0; i < properties.gates.Length; i++)
        //    {
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField($"Gate {i + 1}");

        //        ++EditorGUI.indentLevel;
        //        ShowHexSideCarousel(ref properties.gates[i]);
        //        --EditorGUI.indentLevel;

        //        EditorGUILayout.EndHorizontal();
        //    }
        //    --EditorGUI.indentLevel;

        //    if (serializedObject.hasModifiedProperties)
        //        serializedObject.ApplyModifiedProperties();
        //}

        private void ShowHexSideCarousel(Rect position, SerializedProperty property)
        {
            SerializedProperty localSide = property.FindPropertyRelative("localSide");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
                PreviousOrientation(localSide);
            EditorGUILayout.LabelField(localSide.enumDisplayNames[localSide.enumValueIndex]);
            if (GUILayout.Button(">"))
                NextOrientation(localSide);
            EditorGUILayout.EndHorizontal();

        }

        private void PreviousOrientation(SerializedProperty property)
        {
            property.enumValueIndex = (int)HexSide.Convertor.GetWorldSideAfterRotStep((HexSide.Side)property.enumValueIndex, -1);
        }

        private void NextOrientation(SerializedProperty property)
        {
            property.enumValueIndex = (int)HexSide.Convertor.GetWorldSideAfterRotStep((HexSide.Side)property.enumValueIndex, 1);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0f;
        }
    }
}
