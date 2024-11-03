using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeckContent))]
public class DeckContentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedObject deckContentObject = new SerializedObject(target);

        SerializedProperty fallbackProp = deckContentObject.FindProperty("fallback");
        EditorGUILayout.ObjectField(fallbackProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Traversal tiles");        
        ShowTileResourcesGrid(deckContentObject.FindProperty("traversalContent"));
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Propagator tiles");
        ShowTileResourcesGrid(deckContentObject.FindProperty("propagatorContent"));
       
        if (deckContentObject.hasModifiedProperties)
            deckContentObject.ApplyModifiedProperties();
    }

    private void ShowTileResourcesGrid(SerializedProperty property)
    {
        const int COLUMNS = 4;

        GUIStyle textCenterAlignment = new GUIStyle() { alignment = TextAnchor.MiddleCenter };
        textCenterAlignment.normal.textColor = Color.white;

        for (int i = 0; i < property.arraySize;)
        {
            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < COLUMNS && (i + j) < property.arraySize; ++j)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i + j);

                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(60));
                EditorGUILayout.PropertyField(element.FindPropertyRelative("tileResource"), GUILayout.MaxWidth(50));
                SerializedProperty amountProp = element.FindPropertyRelative("amount");
                amountProp.intValue = EditorGUILayout.IntField(amountProp.intValue, textCenterAlignment, GUILayout.MaxWidth(50));
                EditorGUILayout.EndVertical();
            }

            i += COLUMNS;

            EditorGUILayout.EndHorizontal();
        }
    }


}
