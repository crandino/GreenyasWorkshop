using Greenyas.Hexagon;
using Greenyas.Input;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileManipulator : TileEvent
    {
        [SerializeField]
        private Collider trigger;

        // Rotation
        private float targetRotationAngle = 0f;
        private EventTimer rotationTimer = null;

        // Position
        private Vector3 verticalOffset = Vector3.up * 0.25f;

        private InputManager input = null;

        public override void Initialize()
        {
            enabled = false;

            input = Game.Instance.GetSystem<InputManager>();
            targetRotationAngle = transform.rotation.eulerAngles.y;

            const float EVENT_TIME = 0.3f;
            rotationTimer = new EventTimer(EVENT_TIME, RestrictRotation, OnRotation, AllowRotation);
        }

        public override void OnPickUp(Tile.Data data)
        {
            trigger.enabled = false;
            enabled = true;

            AllowRotation();
        }

        public override void OnRelease(Tile.Data data)
        {
            trigger.enabled = true;
            enabled = false;

            RestrictRotation();

            data.SetOnGrid();
        }

        private void RotateClockwise()
        {
            rotationTimer.Start();
            targetRotationAngle += HexTools.ROTATION_ANGLE;
        }

        private void RotateCounterClockwise()
        {
            rotationTimer.Start();
            targetRotationAngle -= HexTools.ROTATION_ANGLE;
        }

#if UNITY_EDITOR
        public void EditorRotate(float angle)
        {
            targetRotationAngle += angle;
            transform.Rotate(Vector3.up, angle);
        }
#endif

        //public void FindNearCubeCoordAndPlace()
        //{
        //    CubeCoord coord = HexTools.GetNearestCubeCoord(transform.position);
        //    transform.position = HexTools.GetCartesianWorldPos(coord);
        //}

        private void AllowRotation()
        {
            input.OnAxis.OnPositiveDelta += RotateClockwise;
            input.OnAxis.OnNegativeDelta += RotateCounterClockwise;
        }

        private void RestrictRotation()
        {
            input.OnAxis.OnPositiveDelta -= RotateClockwise;
            input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;
        }

        private void OnRotation(float progress)
        {
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationAngle, progress);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
        }

        private void Update()
        {
            rotationTimer.Step();
            if (TileRaycast.CursorRaycastToBoard(out Vector3 boardCursorPos))
                transform.position = boardCursorPos + verticalOffset;
        }
    }

}