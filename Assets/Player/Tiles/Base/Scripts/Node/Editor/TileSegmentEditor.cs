using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSegment))]
public class TileSegmentEditor : Editor
{
    public struct SegmentProperties
    {
        public SerializedProperty meshRenderer;
        
        public GateProperties[] gates;

        public struct GateProperties
        {
            public SerializedProperty hexSide;
            public SerializedProperty initialLocalSide;
            //public SerializedProperty connections;
            //public bool connectionsFoldout;

            public GateProperties(SerializedProperty gate)
            {
                hexSide = gate.FindPropertyRelative("node").FindPropertyRelative("hexSide");
                initialLocalSide = hexSide.FindPropertyRelative("localSide");
                //connections = gate.FindPropertyRelative("connections");
                //connectionsFoldout = false;
            }
        }

        public SegmentProperties(SerializedObject @object)
        {
            meshRenderer = @object.FindProperty("meshRenderer");

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

        EditorGUILayout.ObjectField(properties.meshRenderer);

        ++EditorGUI.indentLevel;
        for (int i = 0; i < properties.gates.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Node {i + 1}");

            ++EditorGUI.indentLevel;
            ShowHexSideCarousel(ref properties.gates[i]);
            --EditorGUI.indentLevel;

            EditorGUILayout.EndHorizontal();

            //properties.gates[i].connectionsFoldout = EditorGUILayout.Foldout(properties.gates[i].connectionsFoldout, "Connections");
            //if(properties.gates[i].connectionsFoldout )
            //    EditorGUILayout.ObjectField(properties.gates[i].connections);

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
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, -1);
        //UpdateNode(ref nodeSerialized);
    }

    private void NextOrientation(ref SegmentProperties.GateProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, 1);
        //UpdateNode(ref nodeSerialized);
    }

    //private void UpdateNode(ref SegmentProperties.GateProperties nodeSerialized)
    //{
    //    Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex);
    //    Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
    //    nodeSerialized.worldDebugPos.vector3Value = localPosition * HexTools.hexagonSize;
    //}
}

