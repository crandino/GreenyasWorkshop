using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryCustomDrawer : PropertyDrawer
{

    private readonly static string keysSerializedName = "keys";
    private readonly static string valuesSerializedName = "values";

    private SerializedKeyValuePair[] serializedPairs;

    private struct SerializedKeyValuePair
    {
        public SerializedProperty key, value;

        public SerializedKeyValuePair(SerializedProperty key, SerializedProperty value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (BuildSerializedKeyValuePairs(property).Length + 1) * EditorGUIUtility.singleLineHeight;
    }

    private SerializedKeyValuePair[] BuildSerializedKeyValuePairs(SerializedProperty property)
    {
        SerializedProperty serializedKeysProperty = property.FindPropertyRelative(keysSerializedName);
        SerializedProperty serializedValuesProperty = property.FindPropertyRelative(valuesSerializedName);

        SerializedKeyValuePair[] serializedPairs = new SerializedKeyValuePair[serializedKeysProperty.arraySize];

        for (int i = 0; i < serializedKeysProperty.arraySize; ++i)
        {
            serializedPairs[i]= new SerializedKeyValuePair(serializedKeysProperty.GetArrayElementAtIndex(i),
                                                           serializedValuesProperty.GetArrayElementAtIndex(i));
        }

        return serializedPairs;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;

        //if (GUI.Button(position, "Clear"))
        //{
        //    IDictionary dict = property.boxedValue as IDictionary;
        //    dict.Clear();
        //    property.serializedObject.ApplyModifiedProperties();
        //}

        position.y += EditorGUIUtility.singleLineHeight;
        
        serializedPairs = BuildSerializedKeyValuePairs(property);

        float halfWidth = EditorGUIUtility.currentViewWidth * 0.5f;
        position.width = halfWidth;

        foreach (SerializedKeyValuePair pair in serializedPairs)
        {
            position.x = 0;
            EditorGUI.PropertyField(position, pair.key, GUIContent.none);
            position.x += halfWidth;
            EditorGUI.PropertyField(position, pair.value, GUIContent.none);
            position.y += EditorGUIUtility.singleLineHeight;
        }
    }

    private void DrawControls(Rect position)
    {
        GUIContent icon = EditorGUIUtility.IconContent("Toolbar Plus", "Add entry");
        GUI.Button(position, icon);
        icon = EditorGUIUtility.IconContent("Toolbar Minus", "Remove entry");
        GUI.Button(position, icon);
        GUIStyle buttonStyle = GUIStyle.none;
        //buttonStyle.CalcSize(icon)

        
        //GUI.Button(position, )
    }

    //public override void OnInspectorGUI()
    //{
    //    Debug.Log("Drawing custom inspector");
    //    EditorGUILayout.LabelField("Hola");

    //    foreach (SerializedKeyValuePair pair in serializedPairs)
    //    {
    //        EditorGUILayout.BeginHorizontal();

    //        EditorGUILayout.ObjectField(pair.key);
    //        EditorGUILayout.ObjectField(pair.value);

    //        EditorGUILayout.EndHorizontal();
    //    }
    //}
}
