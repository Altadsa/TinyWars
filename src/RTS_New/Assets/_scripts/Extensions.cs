using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector3[] VieportBounds(this Camera camera)
    {
        var viewportBounds = new Vector3[4];
        viewportBounds[0] = camera.ViewportToWorldPoint(Vector3.zero);
        viewportBounds[1] = camera.ViewportToWorldPoint(Vector3.up);
        viewportBounds[2] = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        viewportBounds[3] = camera.ViewportToWorldPoint(Vector3.right);
        return viewportBounds;
    }

    public static Vector3 SwapYZ(this Vector3 vector3)
    {
        var tmp = vector3.y;
        vector3.y = vector3.z;
        vector3.z = tmp;
        return vector3;
    }
}
