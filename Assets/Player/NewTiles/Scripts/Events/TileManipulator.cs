using Greenyas.Hexagon;
using Greenyas.Input;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileManipulator : MonoBehaviour
    {
        [SerializeField]
        private Collider trigger;

        private TileRotation rotation;
        private TilePosition position;

        private InputManager input = null;

        public void Initialize()
        {
            input = Game.Instance.GetSystem<InputManager>();

            Tile tile = gameObject.GetComponent<Tile>();

            rotation = new TileRotation(tile, 0.3f, RestrictRotation, AllowRotation);
            position = new TilePosition(tile);
        }

        public void PickUp()
        {
            trigger.enabled = false;

            AllowRotation();
            position.AllowMovement();
        }

        public void Release()
        {
            trigger.enabled = true;

            RestrictRotation();
            position.RestrictMovement();
        }

        private void AllowRotation()
        {
            input.OnAxis.OnPositiveDelta += rotation.RotateClockwise;
            input.OnAxis.OnNegativeDelta += rotation.RotateCounterClockwise;
        }

        private void RestrictRotation()
        {
            input.OnAxis.OnPositiveDelta -= rotation.RotateClockwise;
            input.OnAxis.OnNegativeDelta -= rotation.RotateCounterClockwise;
        }

        public CubeCoord SetOnGrid()
        {
            CubeCoord coord = HexTools.GetNearestCubeCoord(transform.position);
            transform.position = HexTools.GetGridCartesianWorldPos(coord);
            return coord;
        }
    }
}