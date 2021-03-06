﻿using System;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour
{
    // Current Health
    protected float _currentHealth = 0;
    
    // Max Health
    protected float _maxHealth = 100;

    protected int _armourValue = 0;
    
    public event Action<float,float> HealthChanged;
    public event Action EntityDestroyed;
    
    public float CurrentHealth => _currentHealth;

    public bool HealthFull => _currentHealth >= _maxHealth;

    protected void UpdateHealth()
    {
        HealthChanged?.Invoke(_currentHealth,_maxHealth);   
    }
    
    protected void CreateHealthUi()
    {
        GetComponent<UiEntityHealthBar>().Init();
        UpdateHealth();
    }

    protected virtual void CheckHealth()
    {
        if (_currentHealth > 0) return;
        EntityDestroyed?.Invoke();
    }
    
    public abstract void TakeDamage(float dmg);
    
}
