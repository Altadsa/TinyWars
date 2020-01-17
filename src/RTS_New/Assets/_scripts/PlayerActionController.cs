using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : IActionController
{
    public void AssignUnitActions(List<ISelectable> selected, RaycastHit acttionTarget)
    {
        bool isSelectionValid = selected[0].GetType() == typeof(Unit);
        if (!isSelectionValid) return;

        foreach (var selectable in selected)
        {
            Unit selectedUnit = selectable as Unit;
            selectedUnit.AssignAction(acttionTarget);
        }
    }
}