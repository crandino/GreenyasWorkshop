using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset styleSheet;

    private Toggle quickRotToogle;

    [MenuItem("Window/Greenyas/Tile Placer")]
    private static void OpenTilePlacerWindow()
    {
        GetWindow<TilePlacer>();
    }

    private struct Label
    {
        public TilePrefabLabel label;
        private DragAndDropManipulator manipulator;

        public Label(TilePrefabLabel prefabPathlabel)
        {
            label = prefabPathlabel;
            manipulator = new DragAndDropManipulator(label);
        }

        public void AddManipulator()
        {
            label.AddManipulator(manipulator);
        }

        public void RemoveManipulator()
        {
            label.RemoveManipulator(manipulator);
        }
    }

    private Label[] labels;

    private void OnEnable()
    {
        if (styleSheet != null)
        {
            styleSheet.CloneTree(rootVisualElement);

            labels = rootVisualElement.Query<TilePrefabLabel>().ForEach(prefabPathLabel => new Label(prefabPathLabel)).ToArray();

            foreach (var label in labels)
            {
                label.AddManipulator();
            }

            quickRotToogle = rootVisualElement.Query<Toggle>();
            quickRotToogle.RegisterValueChangedCallback(SwitchInput);
        }
    }

    private void OnDisable()
    {
        foreach (var label in labels)
        {
            label.RemoveManipulator();
        }

        quickRotToogle.UnregisterValueChangedCallback(SwitchInput);
        UnregisterCallbacksOnScene();
    }

    private void RotateTile(MouseDownEvent evt)
    {
        GameObject tile = Selection.activeGameObject;

        if (!tile || !tile.GetComponent<Tile>())
            return;

        if (evt.shiftKey || evt.ctrlKey)
        {
            evt.StopPropagation();

            if (evt.shiftKey)
                RotateTile(tile.GetComponent<Tile>(), true);
            else if (evt.ctrlKey)
                RotateTile(tile.GetComponent<Tile>(), false);
        }
    }

    private void RotateTile(Tile tile, bool clockWise)
    {
        //if (Application.isPlaying)
        //{
        //    tile.DisconnectTile();
        //    tile.EditorRotate(clockWise ? HexTools.ROTATION_ANGLE : -HexTools.ROTATION_ANGLE);
        //    tile.ConnectTile();
        //}
        //else
        //    tile.EditorRotate(clockWise ? HexTools.ROTATION_ANGLE : -HexTools.ROTATION_ANGLE);
    }

    private void SwitchInput(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
            RegisterCallbacksOnScene();
        else
            UnregisterCallbacksOnScene();
    }

    private void RegisterCallbacksOnScene()
    {
        GetWindow<SceneView>().rootVisualElement.RegisterCallback<MouseDownEvent>(RotateTile, TrickleDown.TrickleDown);
    }

    private void UnregisterCallbacksOnScene()
    {
        GetWindow<SceneView>().rootVisualElement.UnregisterCallback<MouseDownEvent>(RotateTile);
    }
}
