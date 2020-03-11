﻿using Units;
using UnityEngine;

public class Unit : Entity
{
    [SerializeField] private UnitData _data;
    public UnitData Data => _data;

    ISelectionController _selectionController;

    private Modifiers _modifiers;

    UnitActions _unitActions;
    //[SerializeField] UnitHealth health;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _unitActions = GetComponent<UnitActions>();
        Health = GetComponent<UnitHealth>();
        SetupModifiers();
    }

    private void SetupModifiers()
    {
        var entityModifiers = Player.GetUnitModifiers(_data.Type);
        UpdateModifiers(entityModifiers.EntityModifiers);
        entityModifiers.ModifiersChanged += UpdateModifiers;
    }

    public void AssignAction(GameObject targetGo, Vector3 targetPos)
    {
        _unitActions.DetermineAction(targetGo, targetPos);
    }
}