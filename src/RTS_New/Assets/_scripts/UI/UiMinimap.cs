using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiMinimap : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] Transform _player;
    [SerializeField] private Canvas _overlay;
    [SerializeField] private Image _viewport;
    [SerializeField] private Camera _mCamera;
    
    private Camera _mapCam;
    private Vector3 _mapStart, _mapDim;
    
    
    private List<Vector3> _vpWorldCords = new List<Vector3>();
    
    private void Start()
    {
        _mCamera = Camera.main;
        _mapCam = GameObject.FindGameObjectWithTag("minimap").GetComponent<Camera>();
        _overlay.worldCamera = _mapCam;
        
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
        Ray newRay = _mapCam.ViewportPointToRay(viewpointPos);
        RaycastHit hit;
        Physics.Raycast(newRay, out hit);
        return hit.point;
    }
    
    
    
    private void UpdateCameraPosition()
    {
        _vpWorldCords.Clear();
        // first we update the cameras viewport world bounds
        var cameraBounds = ViewportBounds(_mCamera);
        foreach (var cameraBound in cameraBounds)
        {
            RaycastHit hit;
            Physics.Raycast(cameraBound, out hit);
            _vpWorldCords.Add(hit.point);
        }

        var upperWidth = Mathf.Abs(_vpWorldCords[1].x - _vpWorldCords[2].x);
        var height = Mathf.Abs(_vpWorldCords[1].z - _vpWorldCords[0].z);
        var lowerWidth = Mathf.Abs(_vpWorldCords[0].x - _vpWorldCords[3].x);
        
        // set the new position of the viewport image
        var newVpPos = _vpWorldCords[0];
        newVpPos.x += lowerWidth / 2;
        _viewport.transform.position = newVpPos;
        
        // set the size of the viewport image
        _viewport.rectTransform.sizeDelta = new Vector2(upperWidth, height*1.6f) * 10;
        
    }

    /// <summary>
    /// Returns rays originating from the viewport corners of the Camera
    /// </summary>
    /// <param name="camera">The Camera to get viewport coords from.</param>
    /// <returns></returns>
    private Ray[] ViewportBounds(Camera camera)
    {
        var viewportBounds = new Ray[4];
        viewportBounds[0] = camera.ViewportPointToRay(Vector3.zero);
        viewportBounds[1] = camera.ViewportPointToRay(Vector3.up);
        viewportBounds[2] = camera.ViewportPointToRay(new Vector3(1, 1, 0));
        viewportBounds[3] = camera.ViewportPointToRay(Vector3.right);
        return viewportBounds;
    }
    
}
