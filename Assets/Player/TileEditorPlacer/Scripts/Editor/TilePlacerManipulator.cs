using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer
{
    //private static VisualElement SceneView => GetWindow(typeof(SceneView)).rootVisualElement;

    // Añadir un estado donde aún no se coloca la pieza
    // Evitar colocar piezas una encima de otras de alguna manera

    private class TilePlacerManipulator : Manipulator
    {
        private readonly Tile tilePrefab = null;
        private Tile instantiatedTile = null;

        private TilePosition tilePos;
        private TileRotation tileRot;

        private readonly static Color manipulationActiveColor = new(1f, 0.984f, 0f, 0.5f);
        private readonly static Color manipulationInactiveColor = new(.5f, .5f, .5f, 1f);

        public TilePlacerManipulator(TilePrefabOption tileOption)
        {
            target = tileOption;
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
            tilePos = new TilePosition(instantiatedTile, TilePosition.PositionMode.HOVER);
            tileRot = new TileRotation(instantiatedTile);
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
            tilePos.AttachToGrid();
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
                    tilePos.MoveLeft();
                    break;
                case KeyCode.D:
                    tilePos.MoveRight();
                    break;
                case KeyCode.E:
                    tileRot.RotateClockwise();
                    break;           
                case KeyCode.Q:
                    tileRot.RotateCounterClockwise();
                    break;
                case KeyCode.S:
                    tilePos.MoveDown();
                    break;
                case KeyCode.W:
                    tilePos.MoveUp();
                    break;
                
                default:
                    break;
            }
        }      
    }
}