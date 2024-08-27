using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryCustomDrawer : PropertyDrawer
{
    private class SerializationHelper
    {
        public struct DictionaryAction
        {
            public enum Type
            {
                Add, Remove, Clear
            }

            public Type type;
            public int index;
        }

        private readonly Queue<DictionaryAction> actionQueue = new();

        public void AddAction(DictionaryAction.Type actionType, int arrayIndex = 0)
        {
            actionQueue.Enqueue(new DictionaryAction()
            {
                type = actionType,
                index = arrayIndex
            });
        }

        public void PerformActions()
        {
            while(actionQueue.Count != 0)
            {
                DictionaryAction action = actionQueue.Dequeue();

                switch(action.type)
                {
                    case (DictionaryAction.Type.Add):
                        {
                            keys.InsertArrayElementAtIndex(keys.arraySize);
                            values.InsertArrayElementAtIndex(values.arraySize);
                        }
                        break;
                    case (DictionaryAction.Type.Remove):
                        {
                            keys.DeleteArrayElementAtIndex(action.index);
                            values.DeleteArrayElementAtIndex(action.index);
                        }
                        break;
                    case (DictionaryAction.Type.Clear):
                        {
                            keys.ClearArray();
                            values.ClearArray();
                        }
                        break;
                }
            }
        }

        private readonly static string keysSerializedName = "keys";
        private readonly static string valuesSerializedName = "values";
        private readonly static string invalidPairsSerializedName = "invalidKeyValuePairs";
        private readonly static string invalidKeyArrayIndexSerializedName = "keyArrayIndex";

        private SerializedProperty keys, values;
        private readonly List<int> invalidKeys = new List<int>();

        public int KeyCount => keys.arraySize;

        public bool IsIndexInvalid(int index) => invalidKeys.Contains(index);

        public void Rebuild(SerializedProperty serializableDictProperty)
        {
            keys = serializableDictProperty.FindPropertyRelative(keysSerializedName);
            values = serializableDictProperty.FindPropertyRelative(valuesSerializedName);

            invalidKeys.Clear();

            SerializedProperty invalidKeyList = serializableDictProperty.FindPropertyRelative(invalidPairsSerializedName);
            for (int i = 0; i < invalidKeyList.arraySize; ++i)
                invalidKeys.Add(invalidKeyList.GetArrayElementAtIndex(i).FindPropertyRelative(invalidKeyArrayIndexSerializedName).intValue);
        }

        public (SerializedProperty, SerializedProperty) GetKeyPairSerializedEntry(int index) => (keys.GetArrayElementAtIndex(index), values.GetArrayElementAtIndex(index));
    }

    private SerializationHelper helper = new SerializationHelper();

    private readonly static GUIContent plusIcon = EditorGUIUtility.IconContent("Toolbar Plus", "Add entry");
    private readonly static GUIContent removeIcon = EditorGUIUtility.IconContent("Toolbar Minus", "Remove entry");
    private readonly static GUIContent trashIcon = EditorGUIUtility.IconContent("TreeEditor.Trash", "Clear");

    private readonly static GUILayoutOption buttonWidth = GUILayout.MaxWidth(25);
    private readonly static GUIStyle buttonStyle = new GUIStyle() { alignment = TextAnchor.MiddleRight };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        helper.Rebuild(property);

        DrawHeader(label);

        Color defaultBackgroundColor = GUI.backgroundColor;            
        EditorGUI.indentLevel++;

        for(int i = 0; i < helper.KeyCount; ++i)
        {
            (SerializedProperty, SerializedProperty) pair = helper.GetKeyPairSerializedEntry(i);

            GUI.backgroundColor = helper.IsIndexInvalid(i) ? Color.red : defaultBackgroundColor;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(pair.Item1, GUIContent.none, true);

            if (GUILayout.Button(removeIcon, buttonStyle, buttonWidth))
                helper.AddAction(SerializationHelper.DictionaryAction.Type.Remove, i);

            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(pair.Item2, GUIContent.none, true);
        }

        EditorGUI.indentLevel--;

        helper.PerformActions();
    }

    private void DrawHeader(GUIContent label)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);

        if (GUILayout.Button(plusIcon, buttonStyle, buttonWidth))
            helper.AddAction(SerializationHelper.DictionaryAction.Type.Add);

        if (GUILayout.Button(trashIcon, buttonStyle, buttonWidth))
            helper.AddAction(SerializationHelper.DictionaryAction.Type.Clear);

        GUILayout.EndHorizontal();
    }
}
