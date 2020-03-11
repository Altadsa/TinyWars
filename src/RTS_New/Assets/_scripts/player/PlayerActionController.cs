using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : IActionController
{
    public void AssignUnitActions(List<Entity> selected, RaycastHit actionTarget)
    {
        bool isSelectionValid = selected[0].GetType() == typeof(Unit);
        if (!isSelectionValid) return;

        var targetGo = actionTarget.collider.gameObject;
        var targetPos = actionTarget.point;
        
        foreach (var selectable in selected)
        {
            Unit selectedUnit = selectable as Unit;
            selectedUnit.AssignAction(targetGo, targetPos);
        }
    }
}