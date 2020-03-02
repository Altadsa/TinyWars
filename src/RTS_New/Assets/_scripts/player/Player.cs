using System;
using System.Collections.Generic;
using UnityEngine;
using Units;

[System.Serializable]
public class Player : IUpgradeable
{
    public Color Color { get; private set; }
    
    public Material EntityMaterial { get; private set; }
    public bool IsAi { get; private set; }

    private PlayerModifiers _playerModifiers = new PlayerModifiers();
    
    public UnitModifiers GetUnitModifiers(UnitType unitType)
    {
        return _playerModifiers.FetchModifiers(unitType);
    }
    
    public Dictionary<Modifier, float> Modifiers { get; set; }

    public event Action<Dictionary<Modifier, float>> OnModifiersChanged;

    public Player(Color color, Material entityMaterial, bool isAi)
    {
        Color = color;
        EntityMaterial = entityMaterial;
        IsAi = isAi;
        Modifiers = startModifiers;
        _playerModifiers = new PlayerModifiers();
    }

    public void UpdateModifiers()
    {
        OnModifiersChanged?.Invoke(Modifiers);
    }

    public void ChangeModifier(UnitType type, Modifier modifier, float value)
    {
        _playerModifiers.SetUnitModifier(type, modifier, value);
    }

    readonly Dictionary<Modifier, float> startModifiers = new Dictionary<Modifier, float>()
    {
        {Modifier.Damage, 1f},
        {Modifier.Armour, 1f},
        {Modifier.AttackSpeed, 1f},
        {Modifier.MoraleMax, 0.25f},
        {Modifier.MoraleDecay, 1f}
    };
}

public class UnitModifiers
{
    private Dictionary<Modifier, float> _uModifiers = new Dictionary<Modifier, float>()
    {
        {Modifier.Damage, 1},
        {Modifier.Armour, 0}
    };

    public float GetModifier(Modifier modifierKey)
    {
        try
        {
            return _uModifiers[modifierKey];
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public event Action ModifiersChanged;

    public void SetModifier(Modifier modifier, float value)
    {
        _uModifiers[modifier] = value;
        ModifiersChanged?.Invoke();
    }
}