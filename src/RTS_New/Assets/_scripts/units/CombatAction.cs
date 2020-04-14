using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CombatAction : UnitAction
{
    
    private float _baseDamage;
    private float _damage;
    private float _speed;
    
    public int Priority { get; } = 1;

    protected override void Start()
    {
        base.Start();
        _baseDamage = _unit.Data.Damage;
        _damage = _unit.GetModifierValue(Modifier.Damage);
        _speed = _unit.GetModifierValue(Modifier.AttackSpeed);
        _unit.ModifiersUpdated += UpdateModifiers;
    }

    private void UpdateModifiers()
    {
        _damage = _unit.GetModifierValue(Modifier.Damage);
        _speed = _unit.GetModifierValue(Modifier.AttackSpeed);
    }

    public override bool IsActionValid(GameObject targetGo, Vector3 targetPos)
    {
        if (!targetGo) return false;
        var entity = targetGo.GetComponent<Entity>();
        if (!entity || entity.IsAllied(_unit.Player)) return false;
        Debug.Log("Entering Combat");
        var entityHealth = entity.Health;
        StartCoroutine(TargetEntity(entityHealth, entity));
        return true;
    }

    private void TargetDestroyed()
    {
        _unitActions.SetState(UnitState.IDLE);
        StopAllCoroutines();
    }
    
    IEnumerator TargetEntity(EntityHealth health, Entity entity)
    {
        health.EntityDestroyed += TargetDestroyed;
        var tarPos = entity.transform.position;
        _unitActions.SetState(UnitState.MOVE);
        _agent.SetDestination(tarPos);
        yield return new WaitUntil(() => _agent.hasPath);
        yield return new WaitUntil(() => !_agent.hasPath);
        _unitActions.SetState(UnitState.ACT);
        while (health)
        {
            health.TakeDamage(_baseDamage*_damage);
            yield return new WaitForSeconds(_speed);
        }
    }
}
