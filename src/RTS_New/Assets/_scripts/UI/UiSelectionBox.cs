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
    Vector3 _boxStart, _boxEnd;

    public Image debug;
    
    private void Start()
    {
        _selectionBox.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            _boxStart = eventData.position;
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

            float sizeX = Mathf.Abs(_boxStart.x - _boxEnd.x);
            float sizeY = Mathf.Abs(_boxStart.y - _boxEnd.y);
            
            var centre = (_boxStart + _boxEnd) / 2;
//            var centreX = _boxEnd.x > _boxStart.x ? sizeX : -sizeX;
//            var centreY = _boxEnd.y > _boxStart.y ? sizeY : -sizeY;
//            var centre = _boxStart + new Vector3(centreX / 2, centreY / 2, 0);
            _boxStart.z = 0;
            
            _selectionBox.sizeDelta = new Vector2(sizeX, sizeY);
            _selectionBox.position = centre;       
            yield return new WaitForEndOfFrame();
        }

    }


}