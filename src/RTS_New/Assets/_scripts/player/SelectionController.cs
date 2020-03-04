using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionController : MonoBehaviour, ISelectionController
{
    // List of selectable entities
    public List<Entity> Selectable { get; protected set; } = new List<Entity>();
    //List of selected entities
    public List<Entity> Selected { get; protected set; } = new List<Entity>();
    
    public void AddSelected(Entity entity)
    {
        Selectable.Add(entity);
        if (entity is Building)
        {
            var building = entity as Building;
            building.BuildingDataUpdated += UpdateSelection;
        }
    }

    public void RemoveSelected(Entity entity)
    {
        if (Selected.Contains(entity))
        {
            Selected.Remove(entity);
        }
        Selectable.Remove(entity);
        UpdateSelection();
    }

    protected abstract void UpdateSelection();
}