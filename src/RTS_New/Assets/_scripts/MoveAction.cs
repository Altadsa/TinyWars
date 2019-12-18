using UnityEngine;
using UnityEngine.AI;

public class MoveAction : MonoBehaviour, IUnitAction
{
    [SerializeField] private NavMeshAgent agent;

    public int Priority { get; } = 3;
    public bool IsActionValid(RaycastHit actionTarget)
    {
        return agent.SetDestination(actionTarget.point);
    }
}