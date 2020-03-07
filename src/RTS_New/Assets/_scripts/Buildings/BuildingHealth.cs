using System;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    // Current Health
    private float _currentHealth = 0;
    
    // Max Health
    private float _maxHealth = 100;

    private int _armourValue = 0;
    
    [Tooltip("Initialise the building with Max Health?")]
    [SerializeField] bool _initialise = false;
    public event Action<float,float> HealthChanged;
    public event Action EntityDestroyed;

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
        HealthChanged?.Invoke(_currentHealth,_maxHealth);
    }

    private void SetMaxHealth()
    {
        _maxHealth = GetComponent<Building>().GetModifierValue(Modifier.Health);
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth,_maxHealth);
    }

    public void TakeDamage(float dmg)
    {
        var rDmg = dmg * (1 - 0.01f * _armourValue);
        _currentHealth = Mathf.Clamp(_currentHealth-rDmg, 0f, _maxHealth);
        HealthChanged?.Invoke(_currentHealth,_maxHealth);
        
        if (Math.Abs(_currentHealth - Mathf.Epsilon) > 0) return;
        
        EntityDestroyed?.Invoke();
        Destroy(gameObject, 1f);
    }

    public void Repair(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
        HealthChanged?.Invoke(_currentHealth,_maxHealth);
    }
    
}
