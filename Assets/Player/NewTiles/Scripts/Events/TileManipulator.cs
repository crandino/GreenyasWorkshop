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

        public CubeCoord Coord => position.Coord;

        public void Initialize(Tile tile)
        {
            input = Game.Instance.GetSystem<InputManager>();

            rotation = new TileRotation(tile, 0.3f);
            position = new TilePosition(tile);
        }

        public void PickUp()
        {
            trigger.enabled = false;

            input.OnAxis.OnPositiveDelta += rotation.RotateClockwise;
            input.OnAxis.OnNegativeDelta += rotation.RotateCounterClockwise;

            position.AllowMovement();
        }

        public void Release()
        {
            trigger.enabled = true;

            input.OnAxis.OnPositiveDelta -= rotation.RotateClockwise;
            input.OnAxis.OnNegativeDelta -= rotation.RotateCounterClockwise;

            position.RestrictMovement();
        }
    }
}