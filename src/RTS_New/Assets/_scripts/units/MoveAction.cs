using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic Unit movement Script
/// </summary>
public class MoveAction : MonoBehaviour, IUnitAction
{
    [SerializeField] private NavMeshAgent agent;

    public int Priority { get; } = 3;
    public bool IsActionValid(RaycastHit actionTarget)
    {
        StartCoroutine(Move(actionTarget.point));
        return true;
    }

    IEnumerator Move(Vector3 position)
    {
        GetComponent<UnitActions>().SetState(UnitState.MOVE);
        agent.SetDestination(position);
        yield return new WaitUntil(() => agent.remainingDistance < agent.stoppingDistance);
        GetComponent<UnitActions>().SetState(UnitState.IDLE);
    }
}