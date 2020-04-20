using UnityEngine;

public class UnitHealth : EntityHealth
{

    private Unit _unit;
    private UnitActions _unitActions;
    
    void Start()
    {
        _unit = GetComponent<Unit>();
        _unitActions = GetComponent<UnitActions>();
        
        _maxHealth = GetComponent<Unit>().Data.Health;
        _armourValue = (int)GetComponent<Unit>().GetModifierValue(Modifier.Armour);
        _unit.ModifiersUpdated += UpdateModifiers;
        _currentHealth = _maxHealth;
        UpdateHealth();
        CreateHealthUi();
    }

    // Due to engine execution order, we have to initialise the unit health ui after creating unit health.
    protected override void CheckHealth()
    {
        base.CheckHealth();
        if (_currentHealth > 0)
        {
            _unitActions.SetState(UnitState.DMG);
            return;
        }
        _unitActions.SetState(UnitState.DIE);
        //Destroy(gameObject,2);
    }

    private void UpdateModifiers()
    {
        _armourValue = (int)_unit.GetModifierValue(Modifier.Armour);
    }

    public override void TakeDamage(float dmg)
    {
        var rDmg = dmg * (1 - 0.01f * _armourValue);
        _currentHealth = Mathf.Clamp(_currentHealth-rDmg, 0, _maxHealth);
        UpdateHealth();
        CheckHealth();   
    }
}
