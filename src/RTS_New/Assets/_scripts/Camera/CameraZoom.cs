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
        _minZoom = MIN_ZOOM_DISTANCE + controller.position.y;
        _maxZoom = MAX_ZOOM_DISTANCE + controller.position.y;
        controller.position = new Vector3(controller.position.x, controller.position.y + MIN_ZOOM_DISTANCE,
            controller.position.z - MIN_ZOOM_DISTANCE);
        mCam = Camera.main;
    }

    public void ZoomCamera()
    {
        _zoomDelta = Input.mouseScrollDelta.y;
        _controllerY = controller.position.y;
        bool canZoom = _controllerY >= _minZoom && _controllerY <= _maxZoom;
        if (canZoom)
        {
            controller.position = ClampedZoomPosition();
        }
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