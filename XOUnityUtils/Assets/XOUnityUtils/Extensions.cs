using UnityEngine;
using System.Collections;

namespace XOUnityUtils
{
    public static class Extensions
    {
        public static void SetX(this Vector3 p, float x) { p = new Vector3(x, p.y, p.z); }
        public static void SetY(this Vector3 p, float y) { p = new Vector3(p.x, y, p.z); }
        public static void SetZ(this Vector3 p, float z) { p = new Vector3(p.x, p.y, z); }

        public static void SetPositionX(this Transform t, float x) { var p = t.position; t.position = new Vector3(x, p.y, p.z); }
        public static void SetPositionY(this Transform t, float y) { var p = t.position; t.position = new Vector3(p.x, y, p.z); }
        public static void SetPositionZ(this Transform t, float z) { var p = t.position; t.position = new Vector3(p.x, p.y, z); }
    }
}
