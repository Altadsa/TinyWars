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

    public void SetUnitModifier(UnitType type, Modifier modifier, float value)
    {
        _pModifiers[type].SetModifier(modifier, value);
    }
    
    
}