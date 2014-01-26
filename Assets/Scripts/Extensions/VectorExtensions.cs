using UnityEngine;
using System.Collections;

public static class VectorExtensions
{
    public static Vector2 ToXZ(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }
}