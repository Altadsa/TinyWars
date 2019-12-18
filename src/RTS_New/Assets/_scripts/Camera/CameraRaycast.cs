using UnityEngine;

public class CameraRaycast
{
    Camera _mCam;
    Ray _mouseRay;
    RaycastHit _hit;

    public CameraRaycast()
    {
        _mCam = Camera.main;
    }

    public RaycastHit? RaycastForHit()
    {
        _mouseRay = _mCam.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(_mouseRay, out _hit);
        if (hasHit)
        {
            return _hit;
        }

        return null;
    }
}