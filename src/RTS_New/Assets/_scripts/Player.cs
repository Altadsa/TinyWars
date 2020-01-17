using System;
using System.Collections.Generic;
using UnityEngine;
using Units;

[System.Serializable]
public class Player : IUpgradeable
{
    public Color Color { get; private set; }
    public bool IsAi { get; private set; }
    
    
    public Dictionary<Modifier, float> Modifiers { get; set; }

    public event Action<Dictionary<Modifier, float>> OnModifiersChanged;

    public Player(Color color, bool isAi)
    {
        Color = color;
        IsAi = isAi;
        Modifiers = startModifiers;
    }

    public void UpdateModifiers()
    {
        OnModifiersChanged?.Invoke(Modifiers);
    }
    
    

    readonly Dictionary<Modifier, float> startModifiers = new Dictionary<Modifier, float>()
    {
        {Modifier.MeleeDamage, 1f},
        {Modifier.MeleeArmour, 1f},
        {Modifier.AttackSpeed, 1f},
        {Modifier.MoraleMax, 0.25f},
        {Modifier.MoraleDecay, 1f}
    };
}

public class PlayerModifiers
{
    private Dictionary<UnitType, UnitModifiers> _pModifiers;
    
    
    
}

public class UnitModifiers
{
    private Dictionary<Modifier, float> _uModifiers;
}