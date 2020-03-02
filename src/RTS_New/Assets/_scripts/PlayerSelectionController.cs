using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


/// <summary>
 ///  Handles Entity Selection for real players using mouse input
 /// </summary>
 public class PlayerSelectionController : MonoBehaviour, ISelectionController, IPointerDownHandler, IPointerUpHandler
{

    const int LEFT_MOUSE_BUTTON = 0;
    Vector3 startMousePosition, endMousePosition;

    [FormerlySerializedAs("cameraController")] [SerializeField] CameraController _cameraController;
    Camera _mCamera;

    //List of all selectable entities that share the same player
    [HideInInspector]
    public List<Entity> Selectable { get; set; } = new List<Entity>();
    //List of selected entities
    public List<Entity> Selected { get; private set; } = new List<Entity>();

    public event Action<List<Entity>> SelectionUpdated;
    
    private void Awake()
    {
        _mCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            startMousePosition = _mCamera.ScreenToViewportPoint(Input.mousePosition);
            Select(_cameraController.Hit, Input.GetKey(KeyCode.LeftControl));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            endMousePosition = _mCamera.ScreenToViewportPoint(Input.mousePosition);
            if (startMousePosition != endMousePosition)
                SelectArea(DrawRect());
        }
    }

    private void Select(RaycastHit? hitSelection, bool selectMultiple)
    {
        if (!hitSelection.HasValue)
        {
            if (!selectMultiple) DeselectAll();
 
            return;
        }
        var selected = RaycastSelection(hitSelection.Value);
        if (!selected || !Selectable.Contains(selected) || Selected.Contains(selected))
        {
            if (!selectMultiple)
                DeselectAll();
            else
            {
                selected.Deselect();
                Selected.Remove((selected));
            }
            SelectionUpdated?.Invoke(Selected);
            return;
        }

        if (!Selectable.Contains(selected))
        {
            if (selectMultiple) return;
            DeselectAll();
            SelectionUpdated?.Invoke(Selected);
        }
        else
        {
            if (IsBuilding(selected))
            {
                Debug.LogWarning("Selected Building");
                DeselectAll();
                selected.Select();
                Selected.Add(selected);
            }
            else
            {
                if (selectMultiple && !Selected.Exists(IsBuilding))
                {
                    AddToSelection(selected);
                }
                else
                {
                    DeselectAll();
                    AddToSelection(selected);
                }

            }
            SelectionUpdated?.Invoke(Selected);
        }
    }

    private void SelectArea(Rect area)
    {
        DeselectAll();
        Selectable.RemoveAll(s => s == null);
        foreach (Entity selectable in Selectable)
        {
            if (IsBuilding(selectable)) continue;
            var viewportPosition = _mCamera.WorldToViewportPoint(selectable.transform.position);
            if (area.Contains(viewportPosition, true))
            {
                selectable.Select();
                Selected.Add(selectable);
            }
        }
        SelectionUpdated?.Invoke(Selected);
    }

    private void AddToSelection(Entity selectable)
    {
        selectable.Select();
        Selected.Add(selectable);
    }

    private void DeselectAll()
    {
        if (Selected.Count == 0)
            return;
        Selected.RemoveAll(s => s == null);
        Selected.ForEach(s => s.Deselect());
        Selected.Clear();
        SelectionUpdated?.Invoke(Selected);
    }

    private Entity RaycastSelection(RaycastHit hitSelection)
    {
        GameObject objectHit = hitSelection.collider.gameObject;
        Entity selection = objectHit.GetComponentInParent<Entity>();
        return selection;
    }

    private bool IsBuilding(Entity selectable)
    {
        return selectable.GetType() == typeof(Building);
    }

    private Rect DrawRect()
    {
        float width = endMousePosition.x - startMousePosition.x;
        float height = endMousePosition.y - startMousePosition.y;
        return new Rect(startMousePosition.x, startMousePosition.y, width, height);
    }


}