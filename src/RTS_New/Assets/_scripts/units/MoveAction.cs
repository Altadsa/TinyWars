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
    public bool IsActionValid(GameObject targetGo, Vector3 targetPos)
    {
        StartCoroutine(Move(targetPos));
        return true;
    }

    IEnumerator Move(Vector3 targetPos)
    {
        GetComponent<UnitActions>().SetState(UnitState.MOVE);
        agent.SetDestination(targetPos);
        yield return new WaitUntil(() => agent.hasPath);
        yield return new WaitUntil(() => agent.remainingDistance < agent.stoppingDistance);
        GetComponent<UnitActions>().SetState(UnitState.IDLE);
    }
}