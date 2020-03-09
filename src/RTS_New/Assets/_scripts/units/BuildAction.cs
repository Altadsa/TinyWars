using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BuildAction : MonoBehaviour, IUnitAction
{
    [SerializeField] private NavMeshAgent _agent;

    private Unit _unit;
    
    private float _efficiency = 25;
    private float _speed;
    
    public int Priority { get; } = 1;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _speed = _unit.GetModifierValue(Modifier.BuildSpeed);
        _unit.ModifiersUpdated += UpdateModifiers;
    }

    private void UpdateModifiers()
    {
        _speed = _unit.GetModifierValue(Modifier.BuildSpeed);
    }

    public bool IsActionValid(RaycastHit actionTarget)
    {
        
        StopAllCoroutines();
        var go = actionTarget.collider.gameObject;
        var building = go.GetComponent<Building>();
        var allied = building.IsAllied(_unit.Player);
        if (!building || !allied) return false;
        StartCoroutine(Build(building));
        return true;

    }

    IEnumerator Build(Building building)
    {
        var dst = building.transform.position;
        _agent.SetDestination(dst);
        yield return new WaitUntil(() => _agent.hasPath);
        GetComponent<UnitActions>().SetState(UnitState.MOVE);
        yield return new WaitUntil(() => _agent.remainingDistance < _agent.stoppingDistance);
        var buildingHealth = building.Health as BuildingHealth;
        while (!buildingHealth.HealthFull)
        {
            GetComponent<UnitActions>().SetState(UnitState.ACT);
            buildingHealth.Repair(_efficiency);
            yield return new WaitForSeconds(_speed);
        }
        GetComponent<UnitActions>().SetState(UnitState.IDLE);
    }
    
}