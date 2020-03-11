using System;
using System.Collections.Generic;
using System.IO;
using Units;
using UnityEngine;

public class PlayerModifiers
{
    private Dictionary<UnitType, Modifiers> _uModifiers;
    private Dictionary<BuildingType, Modifiers> _bModifiers;
    
    public PlayerModifiers()
    {
        _uModifiers = InitialiseUnitModifiers();
        _bModifiers = InitialiseBuildingModifiers();
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
            Debug.LogErrorFormat("{0}: No such Unit exists, or rather, I can't find it's modifiers.", unitType);
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
    public void SetModifier(UnitType type, Modifier modifier, float value)
    {
        _uModifiers[type].SetModifier(modifier, value);
    }

    public void SetModifier(BuildingType type, Modifier modifier, float value)
    {
        _bModifiers[type].SetModifier(modifier,value);
    }

    private Dictionary<UnitType, Modifiers> InitialiseUnitModifiers()
    {
        var initialValues = new Dictionary<UnitType, Modifiers>();
        using (var sr = new StreamReader("Assets/Resources/unit_modifiers.txt"))
        {
            string line = sr.ReadLine();
            Dictionary<Modifier,float> newModifiers = new Dictionary<Modifier, float>();
            var unitModVal = line.Split('=');
            var unitKey = (UnitType) Enum.Parse(typeof(UnitType), unitModVal[0]);
            var modKey = (Modifier) Enum.Parse(typeof(Modifier), unitModVal[1]);
            var modVal = float.Parse(unitModVal[2]);
            newModifiers[modKey] = modVal;
            UnitType lastKey = unitKey;
            while ((line = sr.ReadLine()) != null)
            {
                unitModVal = line.Split('=');
                unitKey = (UnitType) Enum.Parse(typeof(UnitType), unitModVal[0]);
                modKey = (Modifier) Enum.Parse(typeof(Modifier), unitModVal[1]);
                modVal = float.Parse(unitModVal[2]);
                if (unitKey != lastKey)
                {
                    initialValues[lastKey] = new Modifiers(newModifiers);
                    newModifiers.Clear();
                }
                newModifiers[modKey] = modVal;
                lastKey = unitKey;
            }
            initialValues[unitKey] = new Modifiers(newModifiers);
        }

        return initialValues;
    }
    
    private Dictionary<BuildingType, Modifiers> InitialiseBuildingModifiers()
    {
        var initialValues = new Dictionary<BuildingType, Modifiers>();
        using (var sr = new StreamReader("Assets/Resources/building_modifiers.txt"))
        {

            // Initialise new modifiers
            Dictionary<Modifier,float> newModifiers = new Dictionary<Modifier, float>();
            // Read first line and get Entity Type, Modifier Type and value
            string line = sr.ReadLine();          
            var buildingModVal = line.Split('=');
            var buildingKey = (BuildingType) Enum.Parse(typeof(BuildingType), buildingModVal[0]);
            var modKey = (Modifier) Enum.Parse(typeof(Modifier), buildingModVal[1]);
            var modVal = float.Parse(buildingModVal[2]);
            
            newModifiers[modKey] = modVal;
            BuildingType lastKey = buildingKey;
            while ((line = sr.ReadLine()) != null)
            {
                buildingModVal = line.Split('=');
                buildingKey = (BuildingType) Enum.Parse(typeof(BuildingType), buildingModVal[0]);
                modKey = (Modifier) Enum.Parse(typeof(Modifier), buildingModVal[1]);
                modVal = float.Parse(buildingModVal[2]);
                if (buildingKey != lastKey)
                {
                    initialValues[lastKey] = new Modifiers(newModifiers);
                    newModifiers.Clear();
                }
                newModifiers[modKey] = modVal;
                lastKey = buildingKey;
            }
            initialValues[buildingKey] = new Modifiers(newModifiers);
        }

        return initialValues;
    }

}