using System;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers
{
    private Dictionary<Modifier, float> _modifiers;

    // Idea is to invoke event, and entities that use this modifier will be notified such that they can keep their own data up-to-date.
    public event Action<Dictionary<Modifier, float>> ModifiersChanged;
    
    public Modifiers(Dictionary<Modifier, float> newModifiers)
    {
        _modifiers = newModifiers;
    }

    public Dictionary<Modifier, float> EntityModifiers => _modifiers;
    
    public float GetModifier(Modifier modifierKey)
    {
        try
        {
            return _modifiers[modifierKey];
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    


    public void SetModifier(Modifier modifier, float value)
    {
        Debug.LogFormat("Setting new modifier {0} from {1}to {2}", modifier, _modifiers[modifier], value);
        _modifiers[modifier] = value;
        ModifiersChanged?.Invoke(_modifiers);
    }
}