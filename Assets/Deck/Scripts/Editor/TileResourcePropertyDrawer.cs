using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[CustomPropertyDrawer(typeof(TileResource))]
public class TileResourcePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedObject tileResourceObject = new SerializedObject(property.objectReferenceValue);
        
        SerializedProperty iconProperty = tileResourceObject.FindProperty("icon");
        Texture2D texture = AssetPreview.GetAssetPreview(iconProperty.objectReferenceValue);
        EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y,50,50), texture);
        //GUILayout.Label(texture);
        //EditorGUILayout.ObjectField(iconProperty, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 50f;
    }
}