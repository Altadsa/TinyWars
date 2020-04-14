using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class UnitActions : MonoBehaviour
{

    public event Action<UnitState> StateUpdated;
    
    private UnitAction[] _actions;

    private void Awake()
    {
        _actions = GetComponents<UnitAction>();
    }

    public void DetermineAction(GameObject targetGo, Vector3 targetPos)
    {
        foreach (var unitAction in _actions)
        {
            unitAction.StopAction();
            unitAction.IsActionValid(targetGo, targetPos);
            //if (unitAction.IsActionValid(targetGo, targetPos)) return;
        }
    }

    public void SetState(UnitState state)
    {
        StateUpdated?.Invoke(state);
    }
}

public abstract class UnitAction : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Unit _unit;
    protected UnitActions _unitActions;
    
    public abstract bool IsActionValid(GameObject targetGo, Vector3 targetPos);

    public void StopAction()
    {
        StopAllCoroutines();
    }

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<Unit>();
        _unitActions = GetComponent<UnitActions>();
    }
}