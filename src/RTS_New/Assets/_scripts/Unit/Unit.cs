using Units;
using UnityEngine;

public class Unit : Entity
{
    [SerializeField] private UnitType _type;
    
    ISelectionController selectionController;

    private UnitModifiers _modifiers;

    [SerializeField] UnitActions unitActions;
    //[SerializeField] UnitHealth health;
    [SerializeField] GameObject selectionIndicator;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        SetupModifiers();

    }

    private void SetupModifiers()
    {
        _modifiers = FindObjectOfType<PlayerController>().Player.GetUnitModifiers(_type);
        _modifiers.ModifiersChanged += SetModifiers;
        SetModifiers();
    }

    public float _dmg = 0;
    public float _amr = 0;

    private void SetModifiers()
    {
        _dmg = _modifiers.GetModifier(Modifier.Damage);
        _amr = _modifiers.GetModifier(Modifier.Armour);
    }
    
    public override void Select()
    {
        Debug.Log($"Selected by {Player}");
        selectionIndicator.SetActive(true);
    }

    public override void Deselect()
    {
        Debug.Log($"Deselected by {Player}");
        selectionIndicator.SetActive(false);
    }

    public void AssignAction(RaycastHit actionTarget)
    {
        unitActions.DetermineAction(actionTarget);
    }
}