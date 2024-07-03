using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer
{
    private static VisualElement SceneView => GetWindow(typeof(SceneView)).rootVisualElement;

    // Añadir todos los movimientos posibles
    // Añadir un estado donde aún no se coloca la pieza
    // Añadir algún tipo de indicador en el icono que advierte que el proceso está en marcha
    // Con el botón Intro se coloca
    // Con el botón Esc se cancela todo el proceso
    // Evitar colocar piezas una encima de otras de alguna manera

    private class TilePlacerManipulator : Manipulator
    {
        private readonly Tile tilePrefab = null;
        private Tile instantiatedTile = null;

        private readonly static Color manipulationActiveColor = new Color(1f, 0.984f, 0f, 0.5f);
        private readonly static Color manipulationInactiveColor = Color.black;

        public TilePlacerManipulator(TilePrefabOption tileOption)
        {
            target = tileOption;
            target.focusable = true;
            tilePrefab = tileOption.TilePrefab;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(StartPlacement);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(StartPlacement);
        }

        private void StartPlacement(PointerDownEvent _)
        {
            if (instantiatedTile != null)
                return;

            instantiatedTile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            target.RegisterCallback<KeyDownEvent>(ManipulateTile);
            target.style.backgroundColor = manipulationActiveColor;
        }

        public void CancelPlacement()
        {
            if (instantiatedTile != null)
                DestroyImmediate(instantiatedTile.gameObject);

            FinishPlacement();
        }

        private void AcceptPlacement()
        {
            instantiatedTile.SetOnGrid();
            FinishPlacement();
        }

        private void FinishPlacement()
        {
            target.style.backgroundColor = manipulationInactiveColor;
            target.UnregisterCallback<KeyDownEvent>(ManipulateTile);
            instantiatedTile = null;
        }

        private void ManipulateTile(KeyDownEvent evt)
        {
            evt.StopImmediatePropagation();

            switch (evt.keyCode)
            {
                case KeyCode.Return:
                    AcceptPlacement();
                    break;               
                case KeyCode.Escape: 
                    CancelPlacement(); 
                    break;
                case KeyCode.A: 
                    instantiatedTile.MoveLeft();
                    break;
                case KeyCode.D:
                    instantiatedTile.MoveRight();
                    break;
                case KeyCode.E:
                    instantiatedTile.transform.Rotate(Vector3.up, +HexTools.ROTATION_ANGLE);
                    break;           
                case KeyCode.Q:
                    instantiatedTile.transform.Rotate(Vector3.up, -HexTools.ROTATION_ANGLE);
                    break;
                case KeyCode.S: 
                    instantiatedTile.MoveDown();
                    break;
                case KeyCode.W: 
                    instantiatedTile.MoveUp();
                    break;
                
                default:
                    break;
            }
        }      
    }
}