using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


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

    private void Start()
    {
        _mCamera = Camera.main;
        _selectionBox.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            _boxStart = eventData.position;
            StartCoroutine(DrawBox());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            StopAllCoroutines();
            _selectionBox.gameObject.SetActive(false);
        }
    }

    private void OnMouseDrag()
    {
        if (!_selectionBox.gameObject.activeInHierarchy)
        {
            _selectionBox.gameObject.SetActive(true);
        }

        _boxEnd = Input.mousePosition;

        Vector3 rectStart = Camera.main.WorldToScreenPoint(_boxStart);
        Vector3 centre = (rectStart + _boxEnd) / 2;
        rectStart.z = 0;
        float sizeX = Mathf.Abs(rectStart.x - _boxEnd.x);
        float sizeY = Mathf.Abs(rectStart.y - _boxEnd.y);

        _selectionBox.sizeDelta = new Vector2(sizeX, sizeY);
        _selectionBox.position = centre;
    }

    IEnumerator DrawBox()
    {
        while (!Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            if (!_selectionBox.gameObject.activeInHierarchy)
            {
                _selectionBox.gameObject.SetActive(true);
            }

            _boxEnd = Input.mousePosition;

            Vector3 rectStart = Camera.main.WorldToScreenPoint(_boxStart);
            Vector3 centre = (rectStart + _boxEnd) / 2;
            rectStart.z = 0;
            float sizeX = Mathf.Abs(rectStart.x - _boxEnd.x);
            float sizeY = Mathf.Abs(rectStart.y - _boxEnd.y);

            _selectionBox.sizeDelta = new Vector2(sizeX, sizeY);
            _selectionBox.position = centre;          
            yield return new WaitForEndOfFrame();
        }

    }


}