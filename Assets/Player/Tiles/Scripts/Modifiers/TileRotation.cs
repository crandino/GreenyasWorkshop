using Greenyas.Hexagon;
using Greenyas.Input;
using UnityEngine;

public class TileRotation : TileModifier
{
    // Rotation
    private float initialRotationAngle = 0f;
    private float rotationAngle = 0f;

    private float progress = 1f;
    private float inverseTotalTime = 1f;

    private bool InProgress => progress < 1f;

    private static InputManager input;

    protected TileRotation(TileCoordinates coordinates) : base(coordinates)
    { }

    public TileRotation(TileCoordinates coordinates, float rotationTime) : this(coordinates)
    {
        SetTotalTime(rotationTime);
        input = Game.Instance.GetSystem<InputManager>();
    }

    protected void SetTotalTime(float rotationTotalTime)
    {
        inverseTotalTime = 1f / rotationTotalTime;
    }

    public void AllowRotation()
    {
        input.OnAxis.OnPositiveDelta += RotateClockwise;
        input.OnAxis.OnNegativeDelta += RotateCounterClockwise;
    }

    public void RestrictRotation()
    {
        input.OnAxis.OnPositiveDelta -= RotateClockwise;
        input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;
    }
  
    private void RotateClockwise()
    {
        if (InProgress) return;

        Start();
        rotationAngle = +HexTools.ROTATION_ANGLE;
    }

    private void RotateCounterClockwise()
    {
        if (InProgress) return;

        Start();
        rotationAngle = -HexTools.ROTATION_ANGLE;
    }

    protected override void OnStart()
    {
        progress = 0f;
        initialRotationAngle = Coordinates.RotationAngle;
    }

    protected override void OnFinish()
    {
        float finalAngle = initialRotationAngle + rotationAngle;
        Coordinates.RotationAngle = finalAngle;
    }

    protected override bool OnUpdate()
    {
        progress = progress + Time.deltaTime * inverseTotalTime;

        float lerpAngle = Mathf.LerpAngle(initialRotationAngle, initialRotationAngle + rotationAngle, progress);
        Coordinates.RotationAngle = lerpAngle;

        return InProgress;
    }
}
