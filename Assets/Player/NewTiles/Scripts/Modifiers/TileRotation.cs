using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEngine;

public class TileRotation : TileModifier
{
    // Rotation
    private float initialRotationAngle = 0f;
    private float rotationAngle = 0f;

    private float progress = 1f;
    private readonly float inverseTotalTime = 1f;

    private bool InProgress => progress < 1f;

    public TileRotation(Tile tile, float rotationTime = 0.0f) : base(tile)
    { 
        inverseTotalTime = 1f / rotationTime;
    }

    public void RotateClockwise()
    {
        if (InProgress) return;

        Start();
        rotationAngle = +HexTools.ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        if (InProgress) return;

        Start();
        rotationAngle = -HexTools.ROTATION_ANGLE;
    }

    protected override void OnStart()
    {
        progress = 0f;
        initialRotationAngle = Current.transform.rotation.eulerAngles.y;
    }

    protected override void OnFinish()
    {
        float finalAngle = initialRotationAngle + rotationAngle;
        Current.transform.localEulerAngles = new Vector3(Current.transform.localEulerAngles.x, finalAngle, Current.transform.localEulerAngles.z);
    }

    protected override bool OnUpdate()
    {
        progress = progress + Time.deltaTime * inverseTotalTime;

        float lerpAngle = Mathf.LerpAngle(initialRotationAngle, initialRotationAngle + rotationAngle, progress);
        Current.transform.localEulerAngles = new Vector3(Current.transform.localEulerAngles.x, lerpAngle, Current.transform.localEulerAngles.z);

        return InProgress;
    }
}
