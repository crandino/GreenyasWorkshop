using Greenyas.Hexagon;
using Greenyas.Input;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hexalinks.Tile
{
    public class TileManipulator : TileEvent
    {
        [SerializeField]
        private Collider trigger;

        // Rotation
        private float initialRotationAngle = 0f;
        private float targetRotationAngle = 0f;
        private EventTimer rotationTimer = null;

        // Position
        private Vector3 verticalOffset = Vector3.up * 0.25f;

        private InputManager input = null;

        public override void Initialize()
        {
            enabled = false;

            input = Game.Instance.GetSystem<InputManager>();

            const float EVENT_TIME = 0.3f;
            rotationTimer = new EventTimer(EVENT_TIME, StartingRotation, OnRotation, FinishingRotation);
        }

        public override void OnPickUp(Tile.Data data)
        {
            trigger.enabled = false;
            enabled = true;

            initialRotationAngle = targetRotationAngle = transform.rotation.eulerAngles.y;

            AllowRotation();
        }

        public override void OnRelease(Tile.Data data)
        {
            trigger.enabled = true;
            enabled = false;

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetRotationAngle, transform.localEulerAngles.z);

            RestrictRotation();

            data.SetOnGrid();
        }

        private void StartingRotation()
        {
            initialRotationAngle = targetRotationAngle = transform.rotation.eulerAngles.y;
            RestrictRotation();
        }

        private void FinishingRotation()
        {
            AllowRotation();
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
            float angle = Mathf.LerpAngle(initialRotationAngle, targetRotationAngle, progress);
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