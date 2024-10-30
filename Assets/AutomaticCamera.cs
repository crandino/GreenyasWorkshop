using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assertions.Must;

//[ExecuteInEditMode]
public class AutomaticCamera : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    public Camera Cam => cam;

    [SerializeField]
    private Transform up, bottom, left, right;

    [SerializeField]
    private GameObject reference;

    [SerializeField]
    private Vector3 lToCam, uToCam;

    [SerializeField]
    AnimationCurve curve;

    public AnimationCurve Curve => curve;

    [SerializeField]
    private Vector3 localTargetCamPosition;
    private Vector3 velocity = Vector3.zero;

    //public float left = -0.2F;
    //public float right = 0.2F;
    //public float top = 0.2F;
    //public float bottom = -0.2F;

    CameraFrustum frustum;

    [ContextMenu("Calculate")]
    private void CalculateFrustumVectors()
    {
        Vector3[] corners = new Vector3[4];
        cam.CalculateFrustumCorners(new Rect(1f, 0f, 1f, 1f), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, corners);

        Vector3 l = 0.5f * (corners[0] + corners[1]);
        lToCam = cam.transform.position - cam.transform.TransformPoint(l);

        Vector3 u = 0.5f * (corners[1] + corners[2]);
        uToCam = cam.transform.position - cam.transform.TransformPoint(u);

        localTargetCamPosition = transform.position;
    }

    private void Awake()
    {
        frustum = new(this, left, right, up, bottom);
    }

    void LateUpdate()
    {

        //EditorApplication.update += 
        //Debug.Log(cam.projectionMatrix);

        Vector3 newPositionCamera = cam.transform.localPosition;

        Vector3 centerHorizontal = (right.position + left.position) * 0.5f;
        Vector3 centerVertical = (up.position + bottom.position) * 0.5f;

        frustum.UpdateFrustum();

        //Debug.Log($"Is {(planes[1].GetSide(right.position) ? "insdie" : "outside")}");

        //if (frustum.GetSide(CameraFrustum.FrustumSide.Right, right.position))
        //{
        //    Debug.Log("Inside");
        //}
        //else
        //{
        //    Debug.Log("Outside");
        //    //float distance = frustum.planes[1].GetDistanceToPoint(cam.transform.InverseTransformPoint(right.position));
        //    //cam.transform.localPosition = cam.transform.localPosition + ( distance * frustum.planes[1].normal);

        //}

        //Vector3 camCorrectionTranslation = Vector3.zero;

        //Vector3 forwardBackwardOnlyVector = Vector3.Project(frustum.GetTranslationCorrection(), -cam.transform.forward);

        Vector3 targetPosition = transform.position;

        //localTargetCamPosition.z += cam.transform.InverseTransformDirection(frustum.GetTranslationCorrection()).z * Time.deltaTime;
        //targetPosition.x = cam.transform.InverseTransformPoint(centerHorizontal).x;
        targetPosition += cam.transform.InverseTransformPoint(centerVertical).y * cam.transform.up;
        targetPosition += cam.transform.InverseTransformPoint(centerHorizontal).x * cam.transform.right;
        float zCorrection = frustum.GetTranslationCorrection().z;
        targetPosition += zCorrection * cam.transform.forward;
        //targetPosition.x = transform.localPosition.x;

        //if (!frustum.AnyLimitOverPassed)
        //{
        //    localTargetCamPosition.z += frustum.GetAllTranslationCorrection().z * Time.deltaTime;
        //}

        //targetPosition = cam.transform.TransformPoint(-targetPosition);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, Time.deltaTime * 2f);

        //cam.transform.localPosition += frustum.GetTranslationCorrection();
        //cam.transform.LookAt((centerHorizontal + centerVertical) * 0.5f);

        //frustum.DrawCorrection();    

        reference.transform.position = (centerHorizontal + centerVertical) * 0.5f;
    }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }


}

[System.Serializable]
public class CameraFrustum
{
    private PlaneLimit[] limits;
    private PlaneLimit left, right, bottom, up;

    private class PlaneLimit
    {
        private readonly Plane plane;
        private readonly Camera cam;
        private readonly Transform limitPos;
        private float weight;

        public PlaneLimit(Plane worldPlane, Camera camera, AnimationCurve curve, Transform limit)
        {
            plane = worldPlane; // new Plane(camera.transform.InverseTransformVector(worldPlane.normal), worldPlane.distance + (Vector3.Dot(worldPlane.normal, camera.transform.position)));
            cam = camera;
            limitPos = limit;

            weight = 0f;
        }

        public PlaneLimit(PlaneLimit limit, Plane newPlane)
        {
            plane = newPlane;
            cam = limit.cam;
            limitPos = limit.limitPos;

            weight = limit.weight;
        }


        public bool GetSide()
        {
            const float planeOffset = 0.0f;
            float distance = plane.GetDistanceToPoint(limitPos.position);
            return distance > planeOffset;
        }

        public Vector3 GetCorrection()
        {
            float distance = plane.GetDistanceToPoint(limitPos.position);
            //distance = Mathf.Clamp(distance, float.MinValue, 0f);

            Debug.Log($"{limitPos.name}: distance {distance}");

            //distance = Math.Remap(distance, 0.5f, 1.5f, 0f, 1f);

            Debug.DrawLine(limitPos.position, limitPos.position + ((distance /*+ weight*/) * plane.normal), Color.red);

            return distance * plane.normal;
        }

        //public void DrawCorrection() => Debug.DrawLine(limitPos.position, limitPos.position + GetCorrection(), Color.red);
    }


    private Camera cam;

    public CameraFrustum(AutomaticCamera autoCamera, Transform leftLimit, Transform rightLimit, Transform upLimit, Transform bottomLimit)
    {
        cam = autoCamera.Cam;

        // Left, right, bottom, up, near, far
        Plane[] worldPlanes = GeometryUtility.CalculateFrustumPlanes(autoCamera.Cam);

        left = new(worldPlanes[0], autoCamera.Cam, autoCamera.Curve, leftLimit);
        right = new(worldPlanes[1], autoCamera.Cam, autoCamera.Curve, rightLimit);
        bottom = new(worldPlanes[2], autoCamera.Cam, autoCamera.Curve, bottomLimit);
        up = new(worldPlanes[3], autoCamera.Cam, autoCamera.Curve, upLimit);

        limits = new PlaneLimit[] { left, right, bottom, up };
        //near = WorldToLocalPlane(worldPlanes[4]);
        //far = WorldToLocalPlane(worldPlanes[5]);

        //planes = new Plane[] { left, right, bottom, up, near, far };
    }

    public void UpdateFrustum()
    {
        Plane[] worldPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

        left = new(left, worldPlanes[0]);
        right = new(right, worldPlanes[1]);
        bottom = new(bottom, worldPlanes[2]);
        up = new(up, worldPlanes[3]);

        limits = new PlaneLimit[] { left, right, bottom, up };
    }

    public bool AnyLimitOverPassed => GetOverpassedLimits().Length > 0;

    private PlaneLimit[] GetOverpassedLimits()
    {
        return limits.Where(l => !l.GetSide()).ToArray();
    }

    public Vector3 GetTranslationCorrection()
    {
        Vector3 corr = Vector3.zero;
        //Vector3 corrAll = Vector3.zero;

        // Array.ForEach(GetOverpassedLimits(), l => corrOutside += l.GetCorrection());
        //Array.ForEach(limits, l => corrAll += l.GetCorrection());

        if (AnyLimitOverPassed)
            Array.ForEach(GetOverpassedLimits(), l => corr += l.GetCorrection());
        else
            Array.ForEach(limits, l => corr += l.GetCorrection());

        return corr;
    }
}