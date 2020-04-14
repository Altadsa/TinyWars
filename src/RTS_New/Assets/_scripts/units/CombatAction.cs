using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CombatAction : UnitAction
{
    
    private float _baseDamage;
    private float _damage;
    private float _speed;
    private float _range;
    
    public int Priority { get; } = 1;

    protected override void Start()
    {
        base.Start();
        _baseDamage = _unit.Data.Damage;
        _damage = _unit.GetModifierValue(Modifier.Damage);
        _speed = _unit.GetModifierValue(Modifier.AttackSpeed);
        _range = _unit.GetModifierValue(Modifier.Range);
        _unit.ModifiersUpdated += UpdateModifiers;
    }

    private void UpdateModifiers()
    {
        _damage = _unit.GetModifierValue(Modifier.Damage);
        _speed = _unit.GetModifierValue(Modifier.AttackSpeed);
        _range = _unit.GetModifierValue(Modifier.Range);
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
        yield return StartCoroutine(MoveToPosition(tarPos));
        _unitActions.SetState(UnitState.ACT);
        while (health)
        {
            var dmg = _baseDamage * _damage;
            health.TakeDamage(Random.Range(dmg/2, dmg));
            yield return new WaitForSeconds(_speed);
        }
    }
    
}
