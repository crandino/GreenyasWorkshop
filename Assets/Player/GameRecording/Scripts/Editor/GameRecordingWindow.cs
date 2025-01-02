using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameRecordingWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset styleSheet;

    [MenuItem("Window/Greenyas/Game Recording")]
    private static void OpenTilePlacerWindow()
    {
        GetWindow<GameRecordingWindow>();
    }  

    private void OnEnable()
    {
        if (styleSheet != null)
        {
            styleSheet.CloneTree(rootVisualElement);           
        }
    }   

    private void OnDisable()
    {
        
    }
}
