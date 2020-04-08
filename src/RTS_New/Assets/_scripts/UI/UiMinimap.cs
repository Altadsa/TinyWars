using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiMinimap : MonoBehaviour, IPointerDownHandler
{
    public Texture2D DebugTex;
    
    [SerializeField] Transform _player;
    [SerializeField] private Canvas _overlay;
    [SerializeField] private Image _viewport;
    private Camera _mCamera;
    private Camera _miniCam;
    private Vector3 _mapStart, _mapDim;

    public RenderTexture _mapTexture;

    private Rect _mapRec;
    
    private void Start()
    {
        _mCamera = Camera.main;
        _miniCam = GameObject.FindGameObjectWithTag("minimap").GetComponent<Camera>();
        _overlay.worldCamera = _miniCam;
        
        //_mapTexture = (RenderTexture) GetComponentInChildren<RawImage>().texture;
        _mapRec = GetComponentInChildren<RawImage>().gameObject.GetComponent<RectTransform>().rect;
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

    private List<Vector3> _viewportCoords = new List<Vector3>();
    private List<Vector3> _worldCoors = new List<Vector3>();
    
    private void UpdateCameraPosition()
    {
        _worldCoors.Clear();
        // first we update the cameras viewport world bounds
        _viewportCoords.Clear();
        var cameraBounds = ViewportBounds(_mCamera);
        foreach (var cameraBound in cameraBounds)
        {
            RaycastHit hit;
            Physics.Raycast(cameraBound, out hit);
            _worldCoors.Add(hit.point);
            var viewpointCoord = _miniCam.WorldToViewportPoint(hit.point);
            _viewportCoords.Add(viewpointCoord);
        }

        var upperWidth = Mathf.Abs(_worldCoors[1].x - _worldCoors[2].x);
        var height = Mathf.Abs(_worldCoors[1].z - _worldCoors[0].z);
        var lowerWidth = Mathf.Abs(_worldCoors[0].x - _worldCoors[3].x);
        
        _viewport.transform.position = _worldCoors[0] + new Vector3(lowerWidth/2,0,0);
        _viewport.rectTransform.sizeDelta = new Vector2(upperWidth, height*2) * 10;
        
//        Debug.LogFormat("Upper W: {0} \n Lower W: {1} \n Height: {2} \n", upperWidth, lowerWidth, Math.Round(height, 2));
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
        if (_worldCoors == null) return;
        Gizmos.color = Color.red;
        foreach (var viewportCoord in _worldCoors)
        {
            Gizmos.DrawSphere(viewportCoord, 3);
        }
    }
}
