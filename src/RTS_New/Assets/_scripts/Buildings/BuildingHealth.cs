using System;
using UnityEngine;

public class BuildingHealth : EntityHealth
{
    
    [Tooltip("Initialise the building with Max Health?")]
    [SerializeField] bool _initialise = false;

    public float CurrentHealth => _currentHealth;

    public bool HealthFull => _currentHealth >= _maxHealth;
    
    void Start()
    {
        _maxHealth = GetComponent<Building>().GetModifierValue(Modifier.Health);
        GetComponent<Building>().ModifiersUpdated += SetMaxHealth;
        if (!_initialise)
            _currentHealth = 0.25f * _maxHealth;
        else
            _currentHealth = _maxHealth;
        UpdateHealth();
    }

    private void SetMaxHealth()
    {
        _maxHealth = GetComponent<Building>().GetModifierValue(Modifier.Health);
        _currentHealth = _maxHealth;
        UpdateHealth();
    }

    public override void TakeDamage(float dmg)
    {
        var rDmg = dmg * (1 - 0.01f * _armourValue);
        _currentHealth = Mathf.Clamp(_currentHealth-rDmg, 0f, _maxHealth);
        UpdateHealth();
        CheckHealth();
    }

    public void Repair(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
        UpdateHealth();
    }
    
}
