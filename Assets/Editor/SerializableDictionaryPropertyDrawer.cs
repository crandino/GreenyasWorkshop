using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
//#if NET_4_6 || NET_STANDARD_2_0
//[CustomPropertyDrawer(typeof(SerializableHashSetBase), true)]
//#endif
public class SerializableDictionaryPropertyDrawer : PropertyDrawer
{
    protected const float IndentWidth = 15f;

    static GUIContent s_iconPlus = IconContent("Toolbar Plus", "Add entry");
    static GUIContent s_iconMinus = IconContent("Toolbar Minus", "Remove entry");
    static GUIContent s_warningIconConflict = IconContent("console.warnicon.sml", "Conflicting key, this entry will be lost");
    static GUIContent s_warningIconOther = IconContent("console.infoicon.sml", "Conflicting key");
    static GUIContent s_warningIconNull = IconContent("console.warnicon.sml", "Null key, this entry will be lost");
    static GUIStyle s_buttonStyle = GUIStyle.none;
    static GUIContent s_tempContent = new GUIContent();


    private class ConflictState
    {
        public object conflictKey = null;
        public object conflictValue = null;
        public int conflictIndex = -1;
        public int conflictOtherIndex = -1;
        public bool conflictKeyPropertyExpanded = false;
        public bool conflictValuePropertyExpanded = false;
        public float conflictLineHeight = 0f;

        public void Clear()
        {
            conflictKey = null;
            conflictValue = null;
            conflictIndex = -1;
            conflictOtherIndex = -1;
            conflictKeyPropertyExpanded = false;
            conflictValuePropertyExpanded = false;
            conflictLineHeight = 0f;
        }
    }

    struct PropertyIdentity
    {
        public PropertyIdentity(SerializedProperty property)
        {
            instance = property.serializedObject.targetObject;
            propertyPath = property.propertyPath;
        }

        public UnityEngine.Object instance;
        public string propertyPath;
    }

    static Dictionary<PropertyIdentity, ConflictState> s_conflictStateDict = new Dictionary<PropertyIdentity, ConflictState>();

    private class DictionaryAux
    {
        const string PROPERTY_NAME = "pairs";
        const string KEY_NAME = "key";
        const string VALUE_NAME = "value";

        private Action delayedCalls = null;

        private SerializedProperty dictProperty = null;

        public struct EntryPair
        {
            public SerializedProperty keyProperty;
            public SerializedProperty valueProperty;
            public int index;

            public EntryPair(SerializedProperty keyProperty, SerializedProperty valueProperty, int index)
            {
                this.keyProperty = keyProperty;
                this.valueProperty = valueProperty;
                this.index = index;
            }
        }

        private SerializedProperty this[int index]
        {
            get { return dictProperty.GetArrayElementAtIndex(index); }
        }       

        private DictionaryAux(SerializedProperty property)
        {
            dictProperty = property.FindPropertyRelative(PROPERTY_NAME);
        }

        public static DictionaryAux GetAuxiliarDictionary(SerializedProperty property)
        {
            return new DictionaryAux(property);
        }

        public SerializedProperty GetKey(int index) => this[index].FindPropertyRelative(KEY_NAME);
        public SerializedProperty GetValue(int index) => this[index].FindPropertyRelative(VALUE_NAME);

        public void AddEntry(int index) => dictProperty.InsertArrayElementAtIndex(index);
        public void RemoveEntry(int index)
        {
            var entryProperty = this[index];

            if (entryProperty.propertyType == SerializedPropertyType.ObjectReference)
                entryProperty.objectReferenceValue = null;

            dictProperty.DeleteArrayElementAtIndex(index);
        }
        public void AddEntryDelayed() => delayedCalls = () => AddEntry(dictProperty.arraySize);
        public void RemoveEntryDelayed(int index) => delayedCalls = () => RemoveEntry(index);       

        public void InvokeAndClearDelayedCalls()
        {
            delayedCalls?.Invoke();
            delayedCalls = null;   
        }

        public IEnumerable<EntryPair> EnumerateEntries(int startIndex = 0)
        {
            if (dictProperty.arraySize > startIndex)
            {
                for (int i = startIndex; i < dictProperty.arraySize; i++)
                    yield return new EntryPair(GetKey(i), GetValue(i), i);
            }
        }
    }

    private DictionaryAux dictAux = null;   

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        dictAux ??= DictionaryAux.GetAuxiliarDictionary(property);

        label = EditorGUI.BeginProperty(position, label, property);       

        ConflictState conflictState = GetConflictState(property);

        if (conflictState.conflictIndex != -1)
        {
            dictAux.AddEntry(conflictState.conflictIndex);
            SerializedProperty keyProperty = dictAux.GetKey(conflictState.conflictIndex);
            SetPropertyValue(keyProperty, conflictState.conflictKey);
            keyProperty.isExpanded = conflictState.conflictKeyPropertyExpanded;

            var valueProperty = dictAux.GetValue(conflictState.conflictIndex);

            SetPropertyValue(valueProperty, conflictState.conflictValue);
            valueProperty.isExpanded = conflictState.conflictValuePropertyExpanded;
        }

        var buttonWidth = s_buttonStyle.CalcSize(s_iconPlus).x;

        var labelPosition = position;
        labelPosition.height = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded)
            labelPosition.xMax -= s_buttonStyle.CalcSize(s_iconPlus).x;

        EditorGUI.PropertyField(labelPosition, property, label, false);

        if (property.isExpanded)
        {
            var buttonPosition = position;
            buttonPosition.xMin = buttonPosition.xMax - buttonWidth;
            buttonPosition.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginDisabledGroup(conflictState.conflictIndex != -1);
            if (GUI.Button(buttonPosition, s_iconPlus, s_buttonStyle))
                dictAux.AddEntryDelayed();
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel++;
            var linePosition = position;
            linePosition.y += EditorGUIUtility.singleLineHeight;
            linePosition.xMax -= buttonWidth;

            foreach (DictionaryAux.EntryPair entry in dictAux.EnumerateEntries())
            {
                var keyProperty = entry.keyProperty;
                var valueProperty = entry.valueProperty;
                int i = entry.index;

                float lineHeight = DrawKeyValueLine(keyProperty, valueProperty, linePosition, i);

                buttonPosition = linePosition;
                buttonPosition.x = linePosition.xMax;
                buttonPosition.height = EditorGUIUtility.singleLineHeight;

                if (GUI.Button(buttonPosition, s_iconMinus, s_buttonStyle))
                    dictAux.RemoveEntryDelayed(i);

                if (i == conflictState.conflictIndex && conflictState.conflictOtherIndex == -1)
                {
                    var iconPosition = linePosition;
                    iconPosition.size = s_buttonStyle.CalcSize(s_warningIconNull);
                    GUI.Label(iconPosition, s_warningIconNull);
                }
                else if (i == conflictState.conflictIndex)
                {
                    var iconPosition = linePosition;
                    iconPosition.size = s_buttonStyle.CalcSize(s_warningIconConflict);
                    GUI.Label(iconPosition, s_warningIconConflict);
                }
                else if (i == conflictState.conflictOtherIndex)
                {
                    var iconPosition = linePosition;
                    iconPosition.size = s_buttonStyle.CalcSize(s_warningIconOther);
                    GUI.Label(iconPosition, s_warningIconOther);
                }


                linePosition.y += lineHeight;
            }

            EditorGUI.indentLevel--;
        }

        dictAux.InvokeAndClearDelayedCalls();       

        conflictState.Clear();

        foreach (var entry1 in dictAux.EnumerateEntries())
        {
            var keyProperty1 = entry1.keyProperty;
            int i = entry1.index;
            object keyProperty1Value = GetPropertyValue(keyProperty1);

            if (keyProperty1Value == null)
            {
                var valueProperty1 = entry1.valueProperty;
                SaveProperty(keyProperty1, valueProperty1, i, -1, conflictState);
                dictAux.RemoveEntry(i);

                break;
            }


            foreach (var entry2 in dictAux.EnumerateEntries(i + 1))
            {
                var keyProperty2 = entry2.keyProperty;
                int j = entry2.index;
                object keyProperty2Value = GetPropertyValue(keyProperty2);

                if (ComparePropertyValues(keyProperty1Value, keyProperty2Value))
                {
                    var valueProperty2 = entry2.valueProperty;
                    SaveProperty(keyProperty2, valueProperty2, j, i, conflictState);                   
                    dictAux.RemoveEntry(j);

                    break;
                    //goto breakLoops;
                }
            }
        }
    //breakLoops:

        EditorGUI.EndProperty();
    }

    static float DrawKeyValueLine(SerializedProperty keyProperty, SerializedProperty valueProperty, Rect linePosition, int index)
    {
        bool keyCanBeExpanded = CanPropertyBeExpanded(keyProperty);

        if (valueProperty != null)
        {
            bool valueCanBeExpanded = CanPropertyBeExpanded(valueProperty);

            if (!keyCanBeExpanded && valueCanBeExpanded)
            {
                return DrawKeyValueLineExpand(keyProperty, valueProperty, linePosition);
            }
            else
            {
                var keyLabel = keyCanBeExpanded ? ("Key " + index.ToString()) : "";
                var valueLabel = valueCanBeExpanded ? ("Value " + index.ToString()) : "";
                return DrawKeyValueLineSimple(keyProperty, valueProperty, keyLabel, valueLabel, linePosition);
            }
        }
        else
        {
            if (!keyCanBeExpanded)
            {
                return DrawKeyLine(keyProperty, linePosition, null);
            }
            else
            {
                var keyLabel = string.Format("{0} {1}", ObjectNames.NicifyVariableName(keyProperty.type), index);
                return DrawKeyLine(keyProperty, linePosition, keyLabel);
            }
        }
    }

    static float DrawKeyValueLineSimple(SerializedProperty keyProperty, SerializedProperty valueProperty, string keyLabel, string valueLabel, Rect linePosition)
    {
        float labelWidth = EditorGUIUtility.labelWidth;
        float labelWidthRelative = labelWidth / linePosition.width;

        float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var keyPosition = linePosition;
        keyPosition.height = keyPropertyHeight;
        keyPosition.width = labelWidth - IndentWidth;
        EditorGUIUtility.labelWidth = keyPosition.width * labelWidthRelative;
        EditorGUI.PropertyField(keyPosition, keyProperty, TempContent(keyLabel), true);

        float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
        var valuePosition = linePosition;
        valuePosition.height = valuePropertyHeight;
        valuePosition.xMin += labelWidth;
        EditorGUIUtility.labelWidth = valuePosition.width * labelWidthRelative;
        EditorGUI.indentLevel--;
        EditorGUI.PropertyField(valuePosition, valueProperty, TempContent(valueLabel), true);
        EditorGUI.indentLevel++;

        EditorGUIUtility.labelWidth = labelWidth;

        return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
    }

    static float DrawKeyValueLineExpand(SerializedProperty keyProperty, SerializedProperty valueProperty, Rect linePosition)
    {
        float labelWidth = EditorGUIUtility.labelWidth;

        float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var keyPosition = linePosition;
        keyPosition.height = keyPropertyHeight;
        keyPosition.width = labelWidth - IndentWidth;
        EditorGUI.PropertyField(keyPosition, keyProperty, GUIContent.none, true);

        float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
        var valuePosition = linePosition;
        valuePosition.height = valuePropertyHeight;
        EditorGUI.PropertyField(valuePosition, valueProperty, GUIContent.none, true);

        EditorGUIUtility.labelWidth = labelWidth;

        return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
    }

    static float DrawKeyLine(SerializedProperty keyProperty, Rect linePosition, string keyLabel)
    {
        float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var keyPosition = linePosition;
        keyPosition.height = keyPropertyHeight;
        keyPosition.width = linePosition.width;

        var keyLabelContent = keyLabel != null ? TempContent(keyLabel) : GUIContent.none;
        EditorGUI.PropertyField(keyPosition, keyProperty, keyLabelContent, true);

        return keyPropertyHeight;
    }

    static bool CanPropertyBeExpanded(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Generic:
            case SerializedPropertyType.Vector4:
            case SerializedPropertyType.Quaternion:
                return true;
            default:
                return false;
        }
    }

    static void SaveProperty(SerializedProperty keyProperty, SerializedProperty valueProperty, int index, int otherIndex, ConflictState conflictState)
    {
        conflictState.conflictKey = GetPropertyValue(keyProperty);
        conflictState.conflictValue = valueProperty != null ? GetPropertyValue(valueProperty) : null;
        float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        float valuePropertyHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : 0f;
        float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
        conflictState.conflictLineHeight = lineHeight;
        conflictState.conflictIndex = index;
        conflictState.conflictOtherIndex = otherIndex;
        conflictState.conflictKeyPropertyExpanded = keyProperty.isExpanded;
        conflictState.conflictValuePropertyExpanded = valueProperty != null ? valueProperty.isExpanded : false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        dictAux ??= DictionaryAux.GetAuxiliarDictionary(property);

        float propertyHeight = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded)
        {
            foreach (var entry in dictAux.EnumerateEntries())
            {
                var keyProperty = entry.keyProperty;
                var valueProperty = entry.valueProperty;
                float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
                float valuePropertyHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : 0f;
                float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
                propertyHeight += lineHeight;
            }

            ConflictState conflictState = GetConflictState(property);

            if (conflictState.conflictIndex != -1)
            {
                propertyHeight += conflictState.conflictLineHeight;
            }
        }

        return propertyHeight;
    }

    static ConflictState GetConflictState(SerializedProperty property)
    {
        ConflictState conflictState;
        PropertyIdentity propId = new PropertyIdentity(property);
        if (!s_conflictStateDict.TryGetValue(propId, out conflictState))
        {
            conflictState = new ConflictState();
            s_conflictStateDict.Add(propId, conflictState);
        }
        return conflictState;
    }

    static Dictionary<SerializedPropertyType, PropertyInfo> s_serializedPropertyValueAccessorsDict;

    static SerializableDictionaryPropertyDrawer()
    {
        Dictionary<SerializedPropertyType, string> serializedPropertyValueAccessorsNameDict = new Dictionary<SerializedPropertyType, string>() {
            { SerializedPropertyType.Integer, "intValue" },
            { SerializedPropertyType.Boolean, "boolValue" },
            { SerializedPropertyType.Float, "floatValue" },
            { SerializedPropertyType.String, "stringValue" },
            { SerializedPropertyType.Color, "colorValue" },
            { SerializedPropertyType.ObjectReference, "objectReferenceValue" },
            { SerializedPropertyType.LayerMask, "intValue" },
            { SerializedPropertyType.Enum, "intValue" },
            { SerializedPropertyType.Vector2, "vector2Value" },
            { SerializedPropertyType.Vector3, "vector3Value" },
            { SerializedPropertyType.Vector4, "vector4Value" },
            { SerializedPropertyType.Rect, "rectValue" },
            { SerializedPropertyType.ArraySize, "intValue" },
            { SerializedPropertyType.Character, "intValue" },
            { SerializedPropertyType.AnimationCurve, "animationCurveValue" },
            { SerializedPropertyType.Bounds, "boundsValue" },
            { SerializedPropertyType.Quaternion, "quaternionValue" },
        };
        Type serializedPropertyType = typeof(SerializedProperty);

        s_serializedPropertyValueAccessorsDict = new Dictionary<SerializedPropertyType, PropertyInfo>();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

        foreach (var kvp in serializedPropertyValueAccessorsNameDict)
        {
            PropertyInfo propertyInfo = serializedPropertyType.GetProperty(kvp.Value, flags);
            s_serializedPropertyValueAccessorsDict.Add(kvp.Key, propertyInfo);
        }
    }

    static GUIContent IconContent(string name, string tooltip)
    {
        var builtinIcon = EditorGUIUtility.IconContent(name);
        return new GUIContent(builtinIcon.image, tooltip);
    }

    static GUIContent TempContent(string text)
    {
        s_tempContent.text = text;
        return s_tempContent;
    }    

    public static object GetPropertyValue(SerializedProperty p)
    {
        PropertyInfo propertyInfo;
        if (s_serializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out propertyInfo))
        {
            return propertyInfo.GetValue(p, null);
        }
        else
        {
            if (p.isArray)
                return GetPropertyValueArray(p);
            else
                return GetPropertyValueGeneric(p);
        }
    }

    static void SetPropertyValue(SerializedProperty p, object v)
    {
        PropertyInfo propertyInfo;
        if (s_serializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out propertyInfo))
        {
            propertyInfo.SetValue(p, v, null);
        }
        else
        {
            if (p.isArray)
                SetPropertyValueArray(p, v);
            else
                SetPropertyValueGeneric(p, v);
        }
    }

    static object GetPropertyValueArray(SerializedProperty property)
    {
        object[] array = new object[property.arraySize];
        for (int i = 0; i < property.arraySize; i++)
        {
            SerializedProperty item = property.GetArrayElementAtIndex(i);
            array[i] = GetPropertyValue(item);
        }
        return array;
    }

    static object GetPropertyValueGeneric(SerializedProperty property)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        var iterator = property.Copy();
        if (iterator.Next(true))
        {
            var end = property.GetEndProperty();
            do
            {
                string name = iterator.name;
                object value = GetPropertyValue(iterator);
                dict.Add(name, value);
            } while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);
        }
        return dict;
    }

    static void SetPropertyValueArray(SerializedProperty property, object v)
    {
        object[] array = (object[])v;
        property.arraySize = array.Length;
        for (int i = 0; i < property.arraySize; i++)
        {
            SerializedProperty item = property.GetArrayElementAtIndex(i);
            SetPropertyValue(item, array[i]);
        }
    }

    static void SetPropertyValueGeneric(SerializedProperty property, object v)
    {
        Dictionary<string, object> dict = (Dictionary<string, object>)v;
        var iterator = property.Copy();
        if (iterator.Next(true))
        {
            var end = property.GetEndProperty();
            do
            {
                string name = iterator.name;
                SetPropertyValue(iterator, dict[name]);
            } while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);
        }
    }

    static bool ComparePropertyValues(object value1, object value2)
    {
        if (value1 is Dictionary<string, object> && value2 is Dictionary<string, object>)
        {
            var dict1 = (Dictionary<string, object>)value1;
            var dict2 = (Dictionary<string, object>)value2;
            return CompareDictionaries(dict1, dict2);
        }
        else
        {
            return object.Equals(value1, value2);
        }
    }

    static bool CompareDictionaries(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
    {
        if (dict1.Count != dict2.Count)
            return false;

        foreach (var kvp1 in dict1)
        {
            var key1 = kvp1.Key;
            object value1 = kvp1.Value;

            object value2;
            if (!dict2.TryGetValue(key1, out value2))
                return false;

            if (!ComparePropertyValues(value1, value2))
                return false;
        }

        return true;
    }   

    //static IEnumerable<EnumerationEntry> EnumerateEntries(SerializedProperty keyArrayProperty, SerializedProperty valueArrayProperty, int startIndex = 0)
    //{
    //    if (keyArrayProperty.arraySize > startIndex)
    //    {
    //        int index = startIndex;
    //        var keyProperty = keyArrayProperty.GetArrayElementAtIndex(startIndex);
    //        var valueProperty = valueArrayProperty != null ? valueArrayProperty.GetArrayElementAtIndex(startIndex) : null;
    //        var endProperty = keyArrayProperty.GetEndProperty();

    //        do
    //        {
    //            yield return new EnumerationEntry(keyProperty, valueProperty, index);
    //            index++;
    //        } while (keyProperty.Next(false)
    //            && (valueProperty != null ? valueProperty.Next(false) : true)
    //            && !SerializedProperty.EqualContents(keyProperty, endProperty));
    //    }
    //}

    
}