using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TilePlacerWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset styleSheet;

    [MenuItem("Window/Greenyas/Tile Placer")]
    private static void OpenTilePlacerWindow()
    {
        GetWindow<TilePlacerWindow>();
    }

    private struct Option
    {
        public TilePrefabOption tile;
        private readonly TilePlacerManipulator manipulator;

        public Option(TilePrefabOption tileOption)
        {
            tile = tileOption;
            manipulator = new TilePlacerManipulator(tile);
        }

        public readonly void AddManipulator()
        {
            tile.AddManipulator(manipulator);
        }

        public readonly void RemoveManipulator()
        {
            tile.RemoveManipulator(manipulator);
        }
    }

    private Option[] options;

    private void OnEnable()
    {
        if (styleSheet != null)
        {
            styleSheet.CloneTree(rootVisualElement);

            options = rootVisualElement.Query<TilePrefabOption>().ForEach(prefabPathLabel => new Option(prefabPathLabel)).ToArray();

            foreach (var label in options)
            {
                label.AddManipulator();
            }
        }
    }

    private void OnDisable()
    {
        foreach (var label in options)
            label.RemoveManipulator();
    }
}
