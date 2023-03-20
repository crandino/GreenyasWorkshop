using Greenyas.VectorExtensions.Swizzle;
using Greenyass.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    // Position
    float targetHeight = 0f;

    // Rotation
    const float ROTATION_ANGLE = 360f / 6f;
    const float ROTATION_TIME = 1f;

    private float targetAngle = 0f;
    private float currentRotationTime = 0f;

    private bool IsRotating => currentRotationTime < ROTATION_TIME;

    private static Tile selectedTile = null;
    public static bool IsTileSelected => selectedTile != null;

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

        input.OnAxis.OnPositiveDelta += RotateClockwise;
        input.OnAxis.OnNegativeDelta += RotateCounterClockwise;

        selectedTile = this;
    }

    public void Release()
    {
        targetHeight = 0.0f;

        input.OnAxis.OnPositiveDelta -= RotateClockwise;
        input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;

        hex = Hex.GetNearestHex(transform.position);
        //Vector3 releasedPos = HexMap.GetNearestHexWorldPosFrom(transform.position).SwizzleX_Y(targetHeight);

        //Vector3 releasedPos = TilePlaceHelper.GetNearestPossiblePoint(transform.position);
        UpdatePosition(hex.Get3DCartesianWorldPos(), 0f);

        selectedTile = null;
    }

    public void UpdatePosition(Vector3 position, float height)
    {
        transform.position = new Vector3(position.x, height, position.z);
    }

    private void Update()
    {
        // Rotation
        currentRotationTime += Time.deltaTime;
        float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, currentRotationTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
    }
}
