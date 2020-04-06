using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiMinimap : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Transform _player;
    private Camera _mCamera;
    private Camera _miniCam;
    private Vector3 _mapStart, _mapDim;

    private RenderTexture _mapTexture;
    
    private void Start()
    {
        _mCamera = Camera.main;
        _miniCam = GameObject.FindGameObjectWithTag("minimap").GetComponent<Camera>();
        FindObjectOfType<CameraController>().CameraMoved += UpdateCameraPosition;
        // Set the dimensions and start of the minimap
        SetMapValues();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var diffX = eventData.position.x - _mapStart.x;
        var diffY = eventData.position.y - _mapStart.y;
        var viewpointPos = new Vector2(diffX / _mapDim.x, diffY / _mapDim.y);
        _player.transform.position = GetMapWorldCoords(viewpointPos);
    }

    private void SetMapValues()
    {
        var rectTransform = GetComponentInChildren<RawImage>().rectTransform;
        var rectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(rectCorners);
        _mapStart = rectCorners[0];
        _mapDim = rectCorners[2] - rectCorners[0];
    }
    
    // It can be assumed that this will always return a value since the 
    // Camera will only show the game world.
    private Vector3 GetMapWorldCoords(Vector3 viewpointPos)
    {
        Ray newRay = _miniCam.ViewportPointToRay(viewpointPos);
        RaycastHit hit;
        Physics.Raycast(newRay, out hit);
        return hit.point;
    }

    private List<Vector3> _viewportCoords;
    
    private void UpdateCameraPosition()
    {
        _viewportCoords = new List<Vector3>();
        var cameraBounds = ViewportBounds(_mCamera);
        foreach (var cameraBound in cameraBounds)
        {
            RaycastHit hit;
            Physics.Raycast(cameraBound, out hit);
            _viewportCoords.Add(hit.point);
        }
    }
    
    private Ray[] ViewportBounds(Camera camera)
    {
        var viewportBounds = new Ray[4];
        viewportBounds[0] = camera.ViewportPointToRay(Vector3.zero);
        viewportBounds[1] = camera.ViewportPointToRay(Vector3.up);
        viewportBounds[2] = camera.ViewportPointToRay(new Vector3(1, 1, 0));
        viewportBounds[3] = camera.ViewportPointToRay(Vector3.right);
        return viewportBounds;
    }

    private void OnDrawGizmos()
    {
        if (_viewportCoords == null) return;
        Gizmos.color = Color.blue;
        foreach (var viewportCoord in _viewportCoords)
        {
            Gizmos.DrawSphere(viewportCoord, 1);
        }
    }
}
