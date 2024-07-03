using Hexalinks.Tile;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset styleSheet;

    //private Toggle quickRotToogle;

    [MenuItem("Window/Greenyas/Tile Placer")]
    private static void OpenTilePlacerWindow()
    {
        GetWindow<TilePlacer>();
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

    //private void RotateTile(MouseDownEvent evt)
    //{
    //    GameObject tile = Selection.activeGameObject;

    //    if (!tile || !tile.GetComponent<Tile>())
    //        return;

    //    if (evt.shiftKey || evt.ctrlKey)
    //    {
    //        evt.StopPropagation();

    //        if (evt.shiftKey)
    //            RotateTile(tile.GetComponent<Tile>(), true);
    //        else if (evt.ctrlKey)
    //            RotateTile(tile.GetComponent<Tile>(), false);
    //    }
    //}

    //private void RotateTile(Tile tile, bool clockWise)
    //{
    //    //if (Application.isPlaying)
    //    //{
    //    //    tile.DisconnectTile();
    //    //    tile.EditorRotate(clockWise ? HexTools.ROTATION_ANGLE : -HexTools.ROTATION_ANGLE);
    //    //    tile.ConnectTile();
    //    //}
    //    //else
    //    //    tile.EditorRotate(clockWise ? HexTools.ROTATION_ANGLE : -HexTools.ROTATION_ANGLE);
    //}

    //private void SwitchInput(ChangeEvent<bool> evt)
    //{
    //    if (evt.newValue)
    //        RegisterCallbacksOnScene();
    //    else
    //        UnregisterCallbacksOnScene();
    //}

    //private void RegisterCallbacksOnScene()
    //{
    //    SceneView.RegisterCallback<MouseDownEvent>(RotateTile, TrickleDown.TrickleDown);
    //}

    //private void UnregisterCallbacksOnScene()
    //{
    //    SceneView.UnregisterCallback<MouseDownEvent>(RotateTile);
    //}
}
