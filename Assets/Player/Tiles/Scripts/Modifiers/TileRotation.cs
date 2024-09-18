using Greenyas.Hexagon;
using Greenyas.Input;
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

    private static InputManager input;

    public TileRotation(Tile tile, float rotationTime = 0.0f) : base(tile)
    { 
        inverseTotalTime = 1f / rotationTime;
        input = Game.Instance.GetSystem<InputManager>();
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

#if UNITY_EDITOR
    public
#else
    private
#endif      
    void RotateClockwise()
    {
        if (InProgress) return;

        Start();
        rotationAngle = +HexTools.ROTATION_ANGLE;
    }

#if UNITY_EDITOR
    public
#else
    private
#endif 
    void RotateCounterClockwise()
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
