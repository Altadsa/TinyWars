using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitActions : MonoBehaviour
{

    public event Action<UnitState> StateUpdated;
    
    private List<IUnitAction> actions;

    private void Start()
    {
        actions = GetComponents<IUnitAction>().ToList();
    }

    public void DetermineAction(RaycastHit actionTarget)
    {
        foreach (var unitAction in actions)
        {
            if (unitAction.IsActionValid(actionTarget)) return;
        }
    }

    public void SetState(UnitState state)
    {
        StateUpdated?.Invoke(state);
    }
}