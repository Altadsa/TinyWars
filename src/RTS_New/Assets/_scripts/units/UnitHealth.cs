using UnityEngine;

public class UnitHealth : EntityHealth
{
    
    void Start()
    {
        _maxHealth = GetComponent<Unit>().Data.Health;
        _armourValue = (int)GetComponent<Unit>().GetModifierValue(Modifier.Armour);
        GetComponent<Unit>().ModifiersUpdated += UpdateModifiers;
        _currentHealth = _maxHealth;
        UpdateHealth();
        CreateHealthUi();
    }

    // Due to engine execution order, we have to initialise the unit health ui after creating unit health.
    protected override void CheckHealth()
    {
        base.CheckHealth();
        GetComponent<UnitActions>().SetState(UnitState.DIE);
        Destroy(gameObject,1);
    }

    private void UpdateModifiers()
    {
        _armourValue = (int)GetComponent<Unit>().GetModifierValue(Modifier.Armour);
    }

    public override void TakeDamage(float dmg)
    {
        GetComponent<UnitActions>().SetState(UnitState.DMG);
        var rDmg = dmg * (1 - 0.01f * _armourValue);
        _currentHealth = Mathf.Clamp(_currentHealth-rDmg, 0, _maxHealth);
        UpdateHealth();
        CheckHealth();   
    }
}
