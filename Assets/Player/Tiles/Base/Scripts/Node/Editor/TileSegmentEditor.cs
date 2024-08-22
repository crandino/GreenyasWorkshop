using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSegment), true)]
public class TileSegmentEditor : Editor
{
    public struct PathProperties
    {
        public SerializedProperty meshRenderer;
        public SerializedProperty emissionPathIndex;
        
        public NodeProperties[] nodes;

        public struct NodeProperties
        {
            public SerializedProperty hexSide;
            public SerializedProperty initialLocalSide;
            public SerializedProperty worldDebugPos;

            public NodeProperties(SerializedProperty node)
            {
                hexSide = node.FindPropertyRelative("hexSide");
                initialLocalSide = hexSide.FindPropertyRelative("initialLocalSide");
                worldDebugPos = node.FindPropertyRelative("localDebugPosition");

                Transform transform = ((TileSegment)(node.serializedObject.targetObject)).transform;
                SerializedProperty transformProperty = node.FindPropertyRelative("tileTransform");
                transformProperty.objectReferenceValue = transform;
                transformProperty = hexSide.FindPropertyRelative("tileTransform");
                transformProperty.objectReferenceValue = transform;
            }
        }

        public PathProperties(SerializedObject @object)
        {
            meshRenderer = @object.FindProperty("meshRenderer");
            emissionPathIndex = @object.FindProperty("emissionPathIndex");

            SerializedProperty nodesArray = @object.FindProperty("nodes");
            nodes = new NodeProperties[nodesArray.arraySize];

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new NodeProperties(nodesArray.GetArrayElementAtIndex(i));
            }            
        }
    }

    private PathProperties properties;
    
    private void OnEnable()
    {
        properties = new PathProperties(serializedObject);
        for (int i = 0; i < properties.nodes.Length; i++)
        {
            UpdateNode(ref properties.nodes[i]);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.ObjectField(properties.meshRenderer);
        //properties.emissionPathIndex.intValue = EditorGUILayout.IntField("Path Emission ID", properties.emissionPathIndex.intValue);

        ++EditorGUI.indentLevel;
        for (int i = 0; i < properties.nodes.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Node {i + 1}");

            ++EditorGUI.indentLevel;
            ShowHexSideCarousel(ref properties.nodes[i]);
            --EditorGUI.indentLevel;

            EditorGUILayout.EndHorizontal();

        }
        --EditorGUI.indentLevel;

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    private void ShowHexSideCarousel(ref PathProperties.NodeProperties nodeSerialized)
    {
        if (GUILayout.Button("<"))
            PreviousOrientation(ref nodeSerialized);
        GUILayout.Label(((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex).ToString());
        if (GUILayout.Button(">"))
            NextOrientation(ref nodeSerialized);
    }

    private void PreviousOrientation(ref PathProperties.NodeProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, -1);
        UpdateNode(ref nodeSerialized);
    }

    private void NextOrientation(ref PathProperties.NodeProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, 1);
        UpdateNode(ref nodeSerialized);
    }

    private void UpdateNode(ref PathProperties.NodeProperties nodeSerialized)
    {
        Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex);
        Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
        nodeSerialized.worldDebugPos.vector3Value = localPosition * HexTools.hexagonSize;
    }
}

