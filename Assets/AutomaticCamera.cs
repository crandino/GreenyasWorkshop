using System;
using System.Linq;
using UnityEngine;

public class AutomaticCamera : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private Transform up, bottom, left, right;

    private Vector3 targetCameraPosition;
    private Vector3 velocity = Vector3.zero;

    private CameraFrustum frustum;

    private void Awake()
    {
        TileFinder.GridLimits gridLimits = TileFinder.GetLimits();

        up = gridLimits.north;
        bottom = gridLimits.south;
        left = gridLimits.west;
        right = gridLimits.east;

        frustum = new(camera, left, right, up, bottom);
    }

    void LateUpdate()
    {
        Vector3 centerHorizontal = (right.position + left.position) * 0.5f;
        Vector3 centerVertical = (up.position + bottom.position) * 0.5f;

        targetCameraPosition = transform.position;

        targetCameraPosition += camera.transform.InverseTransformPoint(centerVertical).y * camera.transform.up;
        targetCameraPosition += camera.transform.InverseTransformPoint(centerHorizontal).x * camera.transform.right;
        targetCameraPosition += frustum.GetTranslationCorrection().z * camera.transform.forward;

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetCameraPosition, ref velocity, Time.deltaTime);
    }  
}

[System.Serializable]
public class CameraFrustum
{
    private PlaneLimit[] limits; // left, right, bottom, up

    private static Camera Camera { set; get; }

    public CameraFrustum(Camera camera, Transform leftLimit, Transform rightLimit, Transform upLimit, Transform bottomLimit)
    {
        CameraFrustum.Camera = camera;

        // Left, right, bottom, up, near, far
        Plane[] worldPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        
        limits = new PlaneLimit[] {
            new(worldPlanes[0], leftLimit, 1.5f),
            new(worldPlanes[1], rightLimit, 0.5f),
            new(worldPlanes[2], bottomLimit, 0.5f),
            new(worldPlanes[3], upLimit, 0.5f)
            };
    }

    private struct PlaneLimit
    {
        private readonly Plane localPlane;
        private readonly float sideThreshold;
        public Transform LimitPos { private set; get; }

        private readonly float DistanceToLocalPlane => localPlane.GetDistanceToPoint(Camera.transform.InverseTransformPoint(LimitPos.position));

        public PlaneLimit(Plane worldPlane, Transform limit, float threshold = 0f)
        {
            localPlane = new Plane(Camera.transform.InverseTransformVector(worldPlane.normal), worldPlane.distance + (Vector3.Dot(worldPlane.normal, Camera.transform.position)));
            LimitPos = limit;
            sideThreshold = threshold;
        }

        public readonly bool GetSide()
        {
            return DistanceToLocalPlane > sideThreshold;
        }

        public Vector3 GetCorrection()
        {
            float distance = DistanceToLocalPlane - sideThreshold;
            Vector3 correction = Camera.transform.TransformDirection(distance * localPlane.normal);
            Debug.DrawLine(LimitPos.position, LimitPos.position + correction, Color.red);

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