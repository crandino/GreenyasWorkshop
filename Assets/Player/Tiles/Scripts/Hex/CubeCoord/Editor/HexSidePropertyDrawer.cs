using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HexSide))]
public class HexSidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowHexSideCarousel(position, property);
    }

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
