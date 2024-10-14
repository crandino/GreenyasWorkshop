using Hexalinks.Tile;
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

    private readonly struct Option
    {
        private readonly TilePrefabOption tile;
        private readonly TilePrefabOptionManipulator manipulator;

        public Option(TilePrefabOption tileOption)
        {
            tile = tileOption;
            manipulator = new TilePrefabOptionManipulator(tile);
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

        RegisterCallbackOnScene();
    }

    private void RegisterCallbackOnScene()
    {
        Selection.selectionChanged += ChangeTileSelection;
    }

    private void UnregisterCallbackOnScene()
    {
        Selection.selectionChanged -= ChangeTileSelection;
    }

    private void ChangeTileSelection()
    {
        Tile tile = null;
        if (Selection.activeGameObject != null)
        {
            tile = Selection.activeGameObject.GetComponent<Tile>();

            if (tile != null && TileEditorManipulator.IsAvailable(tile))
                TileEditorManipulator.Set(tile, GetWindow<SceneView>().rootVisualElement, TilePosition.PositionMode.GRID);
            else
                TileEditorManipulator.Unset();
        }
    }


    private void OnDisable()
    {
        foreach (var label in options)
            label.RemoveManipulator();

        UnregisterCallbackOnScene();
    }
}
