using System;
using System.Linq;
using Units;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField] private UnitType _type;
    
    ISelectionController selectionController;

    private UnitModifiers _modifiers;

    [SerializeField] UnitActions unitActions;
    //[SerializeField] UnitHealth health;
    [SerializeField] GameObject selectionIndicator;
    public Player Player { get; private set; }
    public Transform Transform => transform;

    public void Initialize(Player player)
    {
        Player = player;
        selectionIndicator.SetActive(false);
        FindObjectsOfType<PlayerController>()
            .FirstOrDefault(c => c.Player == Player)
            ?.SelectionController.Selectable.Add(this);
        SetupModifiers();
        //health.Initialize(player);
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
    
    public void Select()
    {
        Debug.Log($"Selected by {Player}");
        selectionIndicator.SetActive(true);
    }

    public void Deselect()
    {
        Debug.Log($"Deselected by {Player}");
        selectionIndicator.SetActive(false);
    }

    public void AssignAction(RaycastHit actionTarget)
    {
        unitActions.DetermineAction(actionTarget);
    }
}