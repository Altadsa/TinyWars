using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


/// <summary>
/// Draws the selection area for the player
/// </summary>
public class UiSelectionBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const int LEFT_MOUSE_BUTTON = 0;

    [SerializeField] RectTransform _selectionBox;
    [SerializeField] CameraController _cameraController;

    private Camera _mCamera;
    Vector3 _boxStart, _boxEnd;

    public Image debug;
    
    private void Start()
    {
        _mCamera = Camera.main;
        _selectionBox.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            _boxStart = Input.mousePosition;
            StartCoroutine(DrawBox());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            StopAllCoroutines();
            _selectionBox.gameObject.SetActive(false);
        }
    }

    IEnumerator DrawBox()
    {
        if (!_selectionBox.gameObject.activeInHierarchy)
        {
            _selectionBox.gameObject.SetActive(true);
        }
        while (!Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            _boxEnd = Input.mousePosition;
            
            Vector3 centre = (_boxStart + _boxEnd) / 2;
            _boxStart.z = 0;
            float sizeX = Mathf.Abs(_boxStart.x - _boxEnd.x);
            float sizeY = Mathf.Abs(_boxStart.y - _boxEnd.y);

            _selectionBox.sizeDelta = new Vector2(sizeX, sizeY);
            _selectionBox.position = centre;       
            yield return null;
        }

    }


}