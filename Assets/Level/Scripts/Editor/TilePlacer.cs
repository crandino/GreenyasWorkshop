using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset styleSheet;

    [MenuItem("Window/Greenyas/Tile Placer")]
    private static void OpenTilePlacerWindow()
    {
        GetWindow<TilePlacer>();

        

    }

    private struct Label
    {
        public PrefabPathLabel label;
        private DragAndDropManipulator manipulator;

        public Label(PrefabPathLabel prefabPathlabel)
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

            labels = rootVisualElement.Query<PrefabPathLabel>().ForEach(prefabPathLabel => new Label(prefabPathLabel)).ToArray();

            foreach (var label in labels)
            {
                label.AddManipulator();
            }
        }
    }

    private void OnDisable()
    {
        foreach (var label in labels)
        {
            label.RemoveManipulator();
        }
    }
}
