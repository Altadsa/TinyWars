﻿using UnityEngine;

public class UnitHealth : EntityHealth
{
    
    void Start()
    {
        _maxHealth = GetComponent<Unit>().Data.Health;
        _armourValue = (int)GetComponent<Unit>().GetModifierValue(Modifier.Armour);
        GetComponent<Unit>().ModifiersUpdated += UpdateModifiers;
        _currentHealth = _maxHealth;
        UpdateHealth();
    }

    private void UpdateModifiers()
    {
        _armourValue = (int)GetComponent<Unit>().GetModifierValue(Modifier.Armour);
    }

    public override void TakeDamage(float dmg)
    {
        var rDmg = dmg * (1 - 0.01f * _armourValue);
        _currentHealth = Mathf.Clamp(_currentHealth-rDmg, 0f, _maxHealth);
        UpdateHealth();
        CheckHealth();   
    }
}
