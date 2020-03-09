using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitCombat : MonoBehaviour, IUnitAction
{
    [SerializeField] private NavMeshAgent _agent;

    private Unit _unit;

    private float _damage;
    private float _speed;
    
    public int Priority { get; } = 1;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _damage = _unit.GetModifierValue(Modifier.Damage);
        _speed = _unit.GetModifierValue(Modifier.AttackSpeed);
        _unit.ModifiersUpdated += UpdateModifiers;
    }

    private void UpdateModifiers()
    {
        _damage = _unit.GetModifierValue(Modifier.Damage);
        _speed = _unit.GetModifierValue(Modifier.AttackSpeed);
    }

    public bool IsActionValid(RaycastHit actionTarget)
    {
        StopAllCoroutines();
        var gameObj = actionTarget.collider.gameObject;
        var entity = gameObj.GetComponent<Entity>();
        if (!entity || entity.IsAllied(_unit.Player)) return false;
        Debug.Log("Entering Combat");
        var entityHealth = entity.Health;
        StartCoroutine(TargetEntity(entityHealth, entity));
        return true;
    }

    private void TargetDestroyed()
    {
        StopAllCoroutines();
        GetComponent<UnitActions>().SetState(UnitState.IDLE);
    }
    
    IEnumerator TargetEntity(EntityHealth health, Entity entity)
    {
        health.EntityDestroyed += TargetDestroyed;
        var tarPos = entity.transform.position;
        _agent.SetDestination(tarPos);
        yield return new WaitUntil(() => _agent.hasPath);
        GetComponent<UnitActions>().SetState(UnitState.MOVE);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
        while (true)
        {
            GetComponent<UnitActions>().SetState(UnitState.ACT);
            health.TakeDamage(_damage);
            yield return new WaitForSeconds(_speed);
        }
    }
}
