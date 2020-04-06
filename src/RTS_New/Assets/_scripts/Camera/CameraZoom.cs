using UnityEngine;

public class CameraZoom
{
    const float MIN_ZOOM_DISTANCE = 5, MAX_ZOOM_DISTANCE = 20;
    float _minZoom, _maxZoom;
    float _zoomDelta;
    float _controllerY;

    readonly Transform controller;
    readonly Camera mCam;

    Vector3 ZoomValue => _zoomDelta * mCam.transform.forward;
    bool IsAtExtents => _controllerY <= _minZoom || _controllerY >= _maxZoom;

    public CameraZoom(Transform controllerTransform)
    {
        controller = controllerTransform;
        var oldPos = controller.position;
        _minZoom = MIN_ZOOM_DISTANCE + oldPos.y;
        _maxZoom = MAX_ZOOM_DISTANCE + oldPos.y;
        controller.position = new Vector3(oldPos.x, oldPos.y + MIN_ZOOM_DISTANCE,
            oldPos.z - MIN_ZOOM_DISTANCE);
        mCam = Camera.main;
    }

    public bool ZoomCamera()
    {
        _zoomDelta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(_zoomDelta) <= 0.0f)
            return false;
        _controllerY = controller.position.y;
        bool canZoom = _controllerY >= _minZoom && _controllerY <= _maxZoom;
        if (canZoom)
        {
            controller.position = ClampedZoomPosition();
        }

        return true;
    }

    Vector3 ClampedZoomPosition()
    {
        Vector3 clampedZoom = controller.position;
        clampedZoom += ZoomValue;
        clampedZoom.x = IsAtExtents ? controller.position.x : clampedZoom.x;
        clampedZoom.y = Mathf.Clamp(clampedZoom.y, _minZoom, _maxZoom);
        clampedZoom.z = IsAtExtents ? controller.position.z : clampedZoom.z;
        return clampedZoom;
    }
}