using Greenyas.Hexagon;
using HexaLinks.Tile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSegment))]
public class TileSegmentEditor : Editor
{
    public struct SegmentProperties
    {
        public GateProperties[] gates;

        public struct GateProperties
        {
            public SerializedProperty hexSide;
            public SerializedProperty initialLocalSide;

            public GateProperties(SerializedProperty gate)
            {
                hexSide = gate.FindPropertyRelative("hexSide");
                initialLocalSide = hexSide.FindPropertyRelative("localSide");
            }
        }

        public SegmentProperties(SerializedObject @object)
        {
            SerializedProperty gatesArray = @object.FindProperty("gates");
            gates = new GateProperties[gatesArray.arraySize];

            for (int i = 0; i < gates.Length; i++)
            {
                gates[i] = new GateProperties(gatesArray.GetArrayElementAtIndex(i));
            }            
        }
    }

    private SegmentProperties properties;
    
    private void OnEnable()
    {
        properties = new SegmentProperties(serializedObject);
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.ObjectField(properties.meshRenderer);

        ++EditorGUI.indentLevel;
        for (int i = 0; i < properties.gates.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Gate {i + 1}");

            ++EditorGUI.indentLevel;
            ShowHexSideCarousel(ref properties.gates[i]);
            --EditorGUI.indentLevel;

            EditorGUILayout.EndHorizontal();
        }
        --EditorGUI.indentLevel;

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    private void ShowHexSideCarousel(ref SegmentProperties.GateProperties nodeSerialized)
    {
        if (GUILayout.Button("<"))
            PreviousOrientation(ref nodeSerialized);
        GUILayout.Label(nodeSerialized.initialLocalSide.enumDisplayNames[nodeSerialized.initialLocalSide.enumValueIndex]);
        if (GUILayout.Button(">"))
            NextOrientation(ref nodeSerialized);
    }

    private void PreviousOrientation(ref SegmentProperties.GateProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.Convertor.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, -1);
    }

    private void NextOrientation(ref SegmentProperties.GateProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.Convertor.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, 1);
    }
}

