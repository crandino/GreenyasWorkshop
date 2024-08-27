using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CubeCoord))]
public class CubeCoordPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var q = property.FindPropertyRelative("q");
        var r = property.FindPropertyRelative("r");
        var s = property.FindPropertyRelative("s");

        EditorGUI.LabelField(position, $"(Q,R,S) -> ({q.intValue},{r.intValue},{s.intValue})");
    }
}