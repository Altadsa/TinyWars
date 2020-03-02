using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Camera MiniCam(this Camera cam)
    {
        return GameObject.FindGameObjectWithTag("minimap").GetComponent<Camera>();
    }
}
