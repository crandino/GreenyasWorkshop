using UnityEditor;
using UnityEngine;
using Greenyas.Hexagon;
using UnityEngine.SocialPlatforms.Impl;

[CustomEditor(typeof(Node),true)]
public class NodeEditor : Editor
{
    private SerializedProperty localSide;
    private SerializedProperty tile;

    private void OnEnable()
    {
        SerializedProperty hexSide = serializedObject.FindProperty("hexSide");
        localSide = hexSide.FindPropertyRelative("initialLocalSide");
        tile = hexSide.FindPropertyRelative("parentTile");
        SetLocalPosition((HexSide.Side)localSide.enumValueIndex);
    }

    //private void OnSceneGUI()
    //{
    //    Node node = target as Node;
    //    Handles.ArrowHandleCap(0, node.transform.position, node.transform.rotation, 0.5f, EventType.Repaint);
    //}

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Hexagon Size Information");
        EditorGUI.indentLevel++;
        EditorGUILayout.ObjectField(tile);

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth));
        EditorGUILayout.LabelField("Local Orientation:");

        if (GUILayout.Button("<")) 
            PreviousOrientation();

        GUILayout.Label(((HexSide.Side)localSide.enumValueIndex).ToString());

        if (GUILayout.Button(">")) 
            NextOrientation();

        EditorGUILayout.EndHorizontal();

        //if (Application.isPlaying)
        //{
        //    EditorGUILayout.LabelField("World Orientation:");
        //    SerializedProperty hexSide = serializedObject.FindProperty("hexSide");
        //    HexSide side = hexSide.objectReferenceValue as System.Object as HexSide;
        //    GUILayout.Label(side.WorldSide.ToString());
        //}

        EditorGUI.indentLevel--;

        EditorGUILayout.ObjectField(serializedObject.FindProperty("inNode"));

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    private void PreviousOrientation()
    {
        localSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)localSide.enumValueIndex, -1);
        SetLocalPosition((HexSide.Side)localSide.enumValueIndex);
    }

    private void NextOrientation()
    {
        localSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)localSide.enumValueIndex, 1);
        SetLocalPosition((HexSide.Side)localSide.enumValueIndex);
    }

    private void SetLocalPosition(HexSide.Side localSide)
    {
        Vector2 localDir = HexSide.GetVectorToNeighborHexOn(localSide);
        Vector3 localPosition = new Vector3(localDir.x, 0f, localDir.y);
        localPosition *= 0.5f;
        ((Node)target).transform.localPosition = localPosition;
    }
}
