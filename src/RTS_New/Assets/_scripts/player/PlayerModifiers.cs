using System.Collections.Generic;
using Units;
using UnityEngine;

public class PlayerModifiers
{
    private Dictionary<UnitType, UnitModifiers> _pModifiers = new Dictionary<UnitType, UnitModifiers>()
    {
        {UnitType.PEASANT, new UnitModifiers()}, 
        {UnitType.ARCHER, new UnitModifiers()} 
    };

    /// <summary>
    /// Retrives the modfiers for the given unit
    /// </summary>
    /// <param name="unitType">The key to identify the modifiers to return</param>
    /// <returns></returns>
    public UnitModifiers FetchModifiers(UnitType unitType)
    {
        try
        {
            //Debug.Log("Successfully retrieved Modifier Data for " + unitType);
            return _pModifiers[unitType];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError("No such Unit Type exists in Modifiers: " + unitType);
            throw;
        }
    }

    /// <summary>
    /// Sets a new value for a given units modifier
    /// </summary>
    /// <param name="type"></param>
    /// <param name="modifier"></param>
    /// <param name="value"></param>
    public void SetUnitModifier(UnitType type, Modifier modifier, float value)
    {
        _pModifiers[type].SetModifier(modifier, value);
    }
    
    
}