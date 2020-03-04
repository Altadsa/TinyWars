using System;
using System.Collections.Generic;
using UnityEngine;
using Units;

[System.Serializable]
public class Player
{
    public Color Color { get; private set; }
    
    public Material EntityMaterial { get; private set; }
    public bool IsAi { get; private set; }

    private PlayerModifiers _playerModifiers;

    private Requirements _playerRequirements;
    
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
        _playerModifiers = new PlayerModifiers();
        _playerRequirements = new Requirements();
    }

    public void ChangeModifier(UnitType type, Modifier modifier, float value)
    {
        _playerModifiers.SetUnitModifier(type, modifier, value);
    }
}

public class UnitModifiers
{
    private Dictionary<Modifier, float> _uModifiers;

    public UnitModifiers(Dictionary<Modifier, float> newModifiers)
    {
        _uModifiers = newModifiers;
    }
    
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
        Debug.LogFormat("Before setting new modifier {0}: {1}", modifier, _uModifiers[modifier]);
        _uModifiers[modifier] = value;
        Debug.LogFormat("After setting new modifier {0}: {1}", modifier, _uModifiers[modifier]);
        ModifiersChanged?.Invoke();
    }
}