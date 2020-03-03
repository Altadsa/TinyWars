﻿using Units;
using UnityEngine;

public class Unit : Entity
{
    [SerializeField] private UnitData _data;
    public UnitData Data => _data;
    
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
        _modifiers = FindObjectOfType<PlayerController>().Player.GetUnitModifiers(_data.Type);
        _modifiers.ModifiersChanged += SetModifiers;
        SetModifiers();
    }
    
    private void SetModifiers()
    {
        //TODO Setup modifiers
    }

    public void AssignAction(RaycastHit actionTarget)
    {
        unitActions.DetermineAction(actionTarget);
    }
}