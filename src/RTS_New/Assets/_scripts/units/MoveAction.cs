using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic Unit movement Script
/// </summary>
public class MoveAction : UnitAction
{
    [SerializeField] private NavMeshAgent agent;

    public int Priority { get; } = 3;
    public override bool IsActionValid(GameObject targetGo, Vector3 targetPos)
    {
        StartCoroutine(Move(targetPos));
        return true;
    }

    IEnumerator Move(Vector3 targetPos)
    {
        _unitActions.SetState(UnitState.MOVE);
        agent.SetDestination(targetPos);
        yield return new WaitUntil(() => agent.hasPath);
        yield return new WaitUntil(() => agent.remainingDistance < agent.stoppingDistance);
        _unitActions.SetState(UnitState.IDLE);
    }
}