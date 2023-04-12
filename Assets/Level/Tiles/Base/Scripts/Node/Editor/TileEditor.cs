using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    private Properties props;

    private void OnEnable()
    {
        props = new Properties(serializedObject);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.ObjectField(props.trigger);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Paths");
        props.pathsArray.ChangeArraySizeAndInitialize<TilePath>(EditorGUILayout.DelayedIntField(props.pathsArray.arraySize));
        props = new Properties(serializedObject); // TODO: Super guarrada maxima para no desincronizar el propio objeto serializado y mi estructura
        EditorGUILayout.EndHorizontal();

        ++EditorGUI.indentLevel;
        for (int i = 0; i < props.pathsArray.arraySize; i++)
        {
            EditorGUILayout.LabelField($"Path {i + 1}");
            ++EditorGUI.indentLevel;

            for (int j = 0; j < props.paths[i].nodes.Length; j++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Node {j + 1}");

                ++EditorGUI.indentLevel;

                ShowHexSideCarousel(ref props.paths[i].nodes[j]);

                EditorGUILayout.EndHorizontal();

                --EditorGUI.indentLevel;
            }

            --EditorGUI.indentLevel;
        }
        --EditorGUI.indentLevel;

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    private struct Properties
    {
        public SerializedProperty trigger;        
        public SerializedProperty pathsArray;

        public PathProperties[] paths;

        public struct PathProperties
        {
            public SerializedProperty path;
            public NodeProperties[] nodes;

            public struct NodeProperties
            {
                public SerializedProperty node;

                public SerializedProperty hexSide;
                public SerializedProperty initialLocalSide;

                // Debug visualization
                public SerializedProperty tileTransform;
                public SerializedProperty worldDebugPos;
            }
        }       

        public Properties(SerializedObject @object)
        {
            trigger = @object.FindProperty("trigger");
            pathsArray = @object.FindProperty("paths");

            paths = new PathProperties[pathsArray.arraySize];

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i].path = pathsArray.GetArrayElementAtIndex(i);
                paths[i].nodes = new PathProperties.NodeProperties[2];
                SerializedProperty nodes = paths[i].path.FindPropertyRelative("nodes");

                for (int j = 0; j < paths[i].nodes.Length; ++j)
                {
                    paths[i].nodes[j].node = nodes.GetArrayElementAtIndex(j);

                    paths[i].nodes[j].hexSide = paths[i].nodes[j].node.FindPropertyRelative("hexSide");
                    paths[i].nodes[j].initialLocalSide = paths[i].nodes[j].hexSide.FindPropertyRelative("initialLocalSide");

                    paths[i].nodes[j].tileTransform = paths[i].nodes[j].node.FindPropertyRelative("tileTransform");
                    Tile tile = ((Tile)@object.targetObject);
                    paths[i].nodes[j].tileTransform.objectReferenceValue = tile.transform;
                    paths[i].nodes[j].worldDebugPos = paths[i].nodes[j].node.FindPropertyRelative("localDebugPosition");
                }
            }
        }
    }

    private void ShowHexSideCarousel(ref Properties.PathProperties.NodeProperties nodeSerialized)
    {
        if (GUILayout.Button("<"))
            PreviousOrientation(ref nodeSerialized);
        GUILayout.Label(((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex).ToString());
        if (GUILayout.Button(">"))
            NextOrientation(ref nodeSerialized);
    }

    private void PreviousOrientation(ref Properties.PathProperties.NodeProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, -1);
        UpdateNode(ref nodeSerialized);
    }

    private void NextOrientation(ref Properties.PathProperties.NodeProperties nodeSerialized)
    {
        nodeSerialized.initialLocalSide.enumValueIndex = (int)HexSide.GetWorldSideAfterRotStep((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex, 1);
        UpdateNode(ref nodeSerialized);
    }

    private void UpdateNode(ref Properties.PathProperties.NodeProperties nodeSerialized)
    {
        Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn((HexSide.Side)nodeSerialized.initialLocalSide.enumValueIndex);
        Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
        nodeSerialized.worldDebugPos.vector3Value = localPosition * HexTools.hexagonSize;
    }
}