using Greenyas.Hexagon;
using Hexalinks.Tile;
using System;
using UnityEngine;

public class TileRotation : TileModifier
{
    // Rotation
    private float initialRotationAngle = 0f;
    private float targetRotationAngle = 0f;

    private readonly Action onStartRotation, onFinishRotation;

    private float progress = 0f;
    private readonly float inverseTotalTime = 1f;

    public TileRotation(Tile tile, float rotationTime = 0.5f, Action onStartRotation = null, Action onFinishRotation = null) : base(tile)
    { 
        inverseTotalTime = 1f / rotationTime;

        this.onStartRotation = onStartRotation;
        this.onFinishRotation = onFinishRotation;
    }

    public void RotateClockwise()
    {
        Start();
        targetRotationAngle = initialRotationAngle + HexTools.ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        Start();
        targetRotationAngle = initialRotationAngle - HexTools.ROTATION_ANGLE;
    }

    protected override void OnStart()
    {
        progress = 0f;
        initialRotationAngle = targetRotationAngle = Current.transform.rotation.eulerAngles.y;
        onStartRotation();
    }

    protected override void OnFinish()
    {
        progress = 0f;
        onFinishRotation();
    }
    protected override void OnUpdate()
    {
        progress = Mathf.Clamp01(progress + Time.deltaTime * inverseTotalTime);

        float angle = Mathf.LerpAngle(initialRotationAngle, targetRotationAngle, progress);
        Current.transform.localEulerAngles = new Vector3(Current.transform.localEulerAngles.x, angle, Current.transform.localEulerAngles.z);

        if (progress == 1f)
            Finish();
    }
}
