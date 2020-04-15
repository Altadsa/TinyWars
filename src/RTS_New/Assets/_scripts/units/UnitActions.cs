using System;
using System.Collections;
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
        var hasValidAction = false;
        foreach (var unitAction in _actions)
        {
            unitAction.StopAction();
            if (!hasValidAction)
                hasValidAction = unitAction.IsActionValid(targetGo, targetPos);
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

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<Unit>();
        _unitActions = GetComponent<UnitActions>();
    }

    protected virtual void Start()
    {

    }
    
    protected IEnumerator MoveToPosition(Vector3 position, float stopDistance)
    {
        _unitActions.SetState(UnitState.MOVE);
        _agent.isStopped = false;
        _agent.SetDestination(position);
        yield return new WaitUntil(() => _agent.hasPath);
        yield return new WaitUntil(() => _agent.remainingDistance <= stopDistance);
        _agent.isStopped = true;
    }
}