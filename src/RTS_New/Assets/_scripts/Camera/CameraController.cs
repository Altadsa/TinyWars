using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public RaycastHit? Hit => _cameraRaycast.RaycastForHit();

    IInput _input;
    CameraRaycast _cameraRaycast;
    CameraMovement _cameraMovement;
    CameraZoom _cameraZoom;

    void Awake()
    {
        _input = new AxisInput();
        _cameraRaycast = new CameraRaycast();
        _cameraMovement = new CameraMovement(transform, _input);
        _cameraZoom = new CameraZoom(transform);
    }

    void Update()
    {
        _cameraMovement.MoveCamera();
        _cameraZoom.ZoomCamera();
    }
    
}