using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitActions : MonoBehaviour
{

    public event Action<UnitState> StateUpdated;
    
    private IUnitAction[] _actions;

    private void Awake()
    {
        _actions = GetComponents<IUnitAction>();
    }

    public void DetermineAction(GameObject targetGo, Vector3 targetPos)
    {
        foreach (var unitAction in _actions)
        {
            if (unitAction.IsActionValid(targetGo, targetPos)) return;
        }
    }

    public void SetState(UnitState state)
    {
        StateUpdated?.Invoke(state);
    }
}