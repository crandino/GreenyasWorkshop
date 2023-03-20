using Greenyas.VectorExtensions.Swizzle;
using Greenyass.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Collider trigger;

    // Position
    float targetHeight = 0f;

    // Rotation
    const float ROTATION_ANGLE = 360f / 6f;
    const float ROTATION_TIME = 1f;

    private float targetAngle = 0f;
    private float currentRotationTime = 0f;

    private bool IsRotating => currentRotationTime < ROTATION_TIME;

    //private static Tile selectedTile = null;
    //public static bool IsTileSelected => selectedTile != null;

    private InputManager input = null;
    private Hex hex = null;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        hex = Hex.GetNearestHex(transform.position);
    }

    public void RotateClockwise()
    {
        currentRotationTime = 0f;
        targetAngle += ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        currentRotationTime = 0f;
        targetAngle -= ROTATION_ANGLE;
    }

    public void PickUp()
    {
        targetHeight = 0.25f;

        trigger.enabled = false;

        input.OnAxis.OnPositiveDelta += RotateClockwise;
        input.OnAxis.OnNegativeDelta += RotateCounterClockwise;
    }

    public void Release()
    {
        targetHeight = 0.0f;

        trigger.enabled = true;

        input.OnAxis.OnPositiveDelta -= RotateClockwise;
        input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;

        hex = Hex.GetNearestHex(transform.position);
        UpdatePosition(hex.Get3DCartesianWorldPos());
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Update()
    {
        // Rotation
        currentRotationTime += Time.deltaTime;
        float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, currentRotationTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
    }
}
