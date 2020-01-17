using System.Collections.Generic;
using UnityEngine;


 /// <summary>
 ///  Handles Entity Selection for real players using mouse input
 /// </summary>
 public class PlayerSelectionController : MonoBehaviour, ISelectionController
{

    const int LEFT_MOUSE_BUTTON = 0;
    Vector3 startMousePosition, endMousePosition;

    [SerializeField] CameraController cameraController;
    Camera mainCamera;

    //List of all selectable entities that share the same player
    [HideInInspector]
    public List<ISelectable> Selectable { get; set; } = new List<ISelectable>();
    //List of selected entities
    public List<ISelectable> Selected { get; private set; } = new List<ISelectable>();

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            startMousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            Select(cameraController.Hit, Input.GetKey(KeyCode.LeftControl));
        }
        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            endMousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
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
        if (selected == null || !Selectable.Contains(selected) || Selected.Contains(selected))
        {
            if (!selectMultiple)
                DeselectAll();
            else
            {
                selected.Deselect();
                Selected.Remove((selected));
            }
            return;
        }

        if (!Selectable.Contains(selected))
        {
            if (selectMultiple) return;
            DeselectAll();
        }
        else
        {
            if (IsBuilding(selected))
            {
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
        }
    }

    private void SelectArea(Rect area)
    {
        DeselectAll();
        Selectable.RemoveAll(s => s == null);
        foreach (ISelectable selectable in Selectable)
        {
            if (IsBuilding(selectable)) continue;
            var viewportPosition = mainCamera.WorldToViewportPoint(selectable.Transform.position);
            if (area.Contains(viewportPosition, true))
            {
                selectable.Select();
                Selected.Add(selectable);
            }
        }
    }

    private void AddToSelection(ISelectable selectable)
    {
        selectable.Select();
        Selected.Add(selectable);
    }

    private void DeselectAll()
    {
        Selected.RemoveAll(s => s == null);
        Selected.ForEach(s => s.Deselect());
        Selected.Clear();
    }

    private ISelectable RaycastSelection(RaycastHit hitSelection)
    {
        GameObject objectHit = hitSelection.collider.gameObject;
        ISelectable selection = objectHit.GetComponentInParent<ISelectable>();
        return selection;
    }

    private bool IsBuilding(ISelectable selectable)
    {
        //return selectable.GetType() == typeof(Building);
        return false;
    }

    private Rect DrawRect()
    {
        float width = endMousePosition.x - startMousePosition.x;
        float height = endMousePosition.y - startMousePosition.y;
        return new Rect(startMousePosition.x, startMousePosition.y, width, height);
    }

}