using System;
using System.Collections.Generic;
using System.IO;
using Units;
using UnityEngine;

public class PlayerModifiers
{
    private Dictionary<UnitType, Modifiers> _uModifiers;
    private Dictionary<BuildingType, Modifiers> _bModifiers = new Dictionary<BuildingType, Modifiers>();
    
    public PlayerModifiers()
    {
        _uModifiers = Initialise();
    }
    
    /// <summary>
    /// Retrieves the modifiers for the given unit.
    /// </summary>
    /// <param name="unitType">The key to identify the modifiers to return</param>
    /// <returns></returns>
    public Modifiers FetchModifiers(UnitType unitType)
    {
        try
        {
            return _uModifiers[unitType];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError("No such Unit Type exists in Modifiers: " + unitType);
            throw;
        }
    }

    /// <summary>
    /// Retrieves the modifiers for the given building.
    /// </summary>
    /// <param name="buildingType">The Building whose modifiers we want to retrieve.</param>
    /// <returns></returns>
    public Modifiers FetchModifiers(BuildingType buildingType)
    {
        try
        {
            return _bModifiers[buildingType];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogErrorFormat("{0}: No such Building exists, or rather, I can't find it's modifiers.", buildingType);
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
        _uModifiers[type].SetModifier(modifier, value);
    }

    private Dictionary<UnitType, Modifiers> Initialise()
    {
        var initialValues = new Dictionary<UnitType, Modifiers>();
        using (var sr = new StreamReader("Assets/Resources/modifiers.txt"))
        {
            string line = sr.ReadLine();
            Dictionary<Modifier,float> newModifiers = new Dictionary<Modifier, float>();
            var unitModVal = line.Split('=');
            var unitKey = (UnitType) Enum.Parse(typeof(UnitType), unitModVal[0]);
            var modKey = (Modifier) Enum.Parse(typeof(Modifier), unitModVal[1]);
            var modVal = float.Parse(unitModVal[2]);
            newModifiers[modKey] = modVal;
            UnitType lastType = unitKey;
            while ((line = sr.ReadLine()) != null)
            {
                unitModVal = line.Split('=');
                unitKey = (UnitType) Enum.Parse(typeof(UnitType), unitModVal[0]);
                modKey = (Modifier) Enum.Parse(typeof(Modifier), unitModVal[1]);
                modVal = float.Parse(unitModVal[2]);
                if (unitKey != lastType)
                {
                    initialValues[unitKey] = new Modifiers(newModifiers);
                    newModifiers.Clear();
                }
                newModifiers[modKey] = modVal;
                lastType = unitKey;
            }
            initialValues[unitKey] = new Modifiers(newModifiers);
        }

        return initialValues;
    }

}