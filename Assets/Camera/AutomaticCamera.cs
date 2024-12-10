using HexaLinks.Tile;
using System;
using System.Linq;
using UnityEngine;

public class AutomaticCamera : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private Vector2 centerOffset;

    [SerializeField, Tooltip("Left, right, bottom, up")]
    private Vector4 frustumLimitsOffset;

    public Camera Camera => camera;
    public Vector2 CenterOffset => centerOffset;
    public Vector4 FrustumLimitsOffset => frustumLimitsOffset;


    private Vector3 targetCameraPosition;
    private Vector3 velocity = Vector3.zero;

    private CameraFrustum frustum;

    private void Start()
    {
        Game.Instance.GetSystem<HexMap>().OnGridChanged += UpdateLimits;

        TileFinder.GridLimits gridLimits = TileFinder.GetLimits();
        frustum = new(this, gridLimits);
    }

    private void UpdateLimits()
    {
        frustum.UpdateLimits(TileFinder.GetLimits());
    }

    void LateUpdate()
    {
        targetCameraPosition = transform.position;

        targetCameraPosition += camera.transform.InverseTransformPoint(frustum.VertLimitCenter).y * camera.transform.up;
        targetCameraPosition += camera.transform.InverseTransformPoint(frustum.HorzLimitCenter).x * camera.transform.right;
        targetCameraPosition += frustum.GetTranslationCorrection().z * camera.transform.forward;

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetCameraPosition, ref velocity, Time.deltaTime);
    }  
}

[System.Serializable]
public class CameraFrustum
{
    private readonly PlaneLimit[] limits; // left, right, bottom, up
    private readonly AutomaticCamera autoCamera;

    public Vector3 HorzLimitCenter => ((limits[0].worldLimitPosition + limits[1].worldLimitPosition) * 0.5f) +
                                      (-autoCamera.CenterOffset.x * autoCamera.Camera.transform.right);
    public Vector3 VertLimitCenter => ((limits[2].worldLimitPosition + limits[3].worldLimitPosition) * 0.5f) +
                                      (-autoCamera.CenterOffset.y * autoCamera.Camera.transform.up);

    public CameraFrustum(AutomaticCamera camera, TileFinder.GridLimits gridlimits)
    {
        autoCamera = camera;

        // Left, right, bottom, up, near, far
        Plane[] worldPlanes = GeometryUtility.CalculateFrustumPlanes(camera.Camera);

        //UpdateLimits(gridlimits);

        limits = new PlaneLimit[] {
            new(worldPlanes[0], autoCamera.Camera.transform, gridlimits.limits[0], autoCamera.FrustumLimitsOffset[0]),
            new(worldPlanes[1], autoCamera.Camera.transform, gridlimits.limits[1], autoCamera.FrustumLimitsOffset[1]),
            new(worldPlanes[2], autoCamera.Camera.transform, gridlimits.limits[2], autoCamera.FrustumLimitsOffset[2]),
            new(worldPlanes[3], autoCamera.Camera.transform, gridlimits.limits[3], autoCamera.FrustumLimitsOffset[3])
            };
    }

    public void UpdateLimits(TileFinder.GridLimits gridlimits)
    {
        limits[0].worldLimitPosition = gridlimits.limits[0];
        limits[1].worldLimitPosition = gridlimits.limits[1];
        limits[2].worldLimitPosition = gridlimits.limits[2];
        limits[3].worldLimitPosition = gridlimits.limits[3];
    }

    private struct PlaneLimit
    {
        private readonly Plane localPlane;
        public Vector3 worldLimitPosition;
        public readonly float sideThreshold;
        private readonly Transform camTransform;

        private readonly float DistanceToLocalPlane => localPlane.GetDistanceToPoint(camTransform.InverseTransformPoint(worldLimitPosition));

        public PlaneLimit(Plane worldPlane, Transform camTransform, Vector3 worldLimit, float threshold = 0f)
        {
            this.camTransform = camTransform;
            localPlane = new Plane(camTransform.InverseTransformVector(worldPlane.normal), worldPlane.distance + (Vector3.Dot(worldPlane.normal, camTransform.position)));
            worldLimitPosition = worldLimit;
            sideThreshold = threshold;
        }

        public readonly bool GetSide()
        {
            return DistanceToLocalPlane > sideThreshold;
        }

        public Vector3 GetCorrection()
        {
            float distanceCorrected = DistanceToLocalPlane - sideThreshold;
            Vector3 correction = distanceCorrected * camTransform.TransformDirection(localPlane.normal);
            Debug.DrawLine(worldLimitPosition, worldLimitPosition + correction, Color.yellow);

            return correction;
        }
    }

    public bool AnyLimitOverPassed => GetOverpassedLimits().Length > 0;

    private PlaneLimit[] GetOverpassedLimits()
    {
        return limits.Where(l => !l.GetSide()).ToArray();
    }

    public Vector3 GetTranslationCorrection()
    {
        Vector3 corr = Vector3.zero;       

        if (AnyLimitOverPassed)
            Array.ForEach(GetOverpassedLimits(), l => corr += l.GetCorrection());
        else
            Array.ForEach(limits, l => corr += l.GetCorrection());

        return corr;
    }
}