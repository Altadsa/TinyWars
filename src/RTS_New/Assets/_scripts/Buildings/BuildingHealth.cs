using System;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    [Range(0,MAX_H)]
    [SerializeField]private float _cHealth = 0;
    const float MAX_H = 100;

    private int _armourValue = 0;
    
    public event Action<float,float> HealthChanged;
    public event Action EntityDestroyed;
    
    void Awake()
    {
        _cHealth = 0.25f * MAX_H;
    }

    public void TakeDamage(float dmg)
    {
        var rDmg = dmg * (1 - 0.01f * _armourValue);
        _cHealth = Mathf.Clamp(_cHealth-rDmg, 0f, MAX_H);
        if (_cHealth == Mathf.Epsilon)
        {
            EntityDestroyed?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
    
}
