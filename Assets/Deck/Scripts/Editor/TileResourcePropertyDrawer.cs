using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TileResource))]
public class TileResourcePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(property.objectReferenceValue != null)
        {
            SerializedObject tileResourceObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty iconProperty = tileResourceObject.FindProperty("icon");
            Texture2D texture = AssetPreview.GetAssetPreview(iconProperty.objectReferenceValue);
            EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y, 50f, 50f), texture);
        }
        else
            EditorGUI.ObjectField(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.objectReferenceValue != null ? 50f : EditorGUIUtility.singleLineHeight;
    }
}