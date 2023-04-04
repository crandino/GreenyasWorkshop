using UnityEngine;

namespace Greenyas.VectorExtensions.Swizzle
{
    // TODO: Maybe this class is very obtuse
    public static class VectorSwizzleExtensions
    {
        public static Vector3 SwizzleX_Y(this Vector2 v, float yValue = 0) => new Vector3(v.x, yValue, v.y);
        public static Vector2 SwizzleXZ(this Vector3 v, float yValue = 0) => new Vector2(v.x, v.z);
    }
}
