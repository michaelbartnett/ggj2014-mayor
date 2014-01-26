using UnityEngine;
using System.Collections;

public static class VectorExtensions
{
    public static Vector2 ToXZ(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }

    public static Vector2 Perpendicular(this Vector2 vec2)
    {
        return new Vector2(-vec2.y, vec2.x);
    }

    public static Vector3 ToX0Z(this Vector2 vec2)
    {
        return new Vector3(vec2.x, 0f, vec2.y);
    }
}