using System;
using System.Collections.Generic;
using System.IO;
using Units;
using UnityEngine;

public class PlayerModifiers
{
    private Dictionary<UnitType, UnitModifiers> _pModifiers;

    public PlayerModifiers()
    {
        _pModifiers = Initialise();
    }
    
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

    private Dictionary<UnitType, UnitModifiers> Initialise()
    {
        var initialValues = new Dictionary<UnitType, UnitModifiers>();
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
                    initialValues[unitKey] = new UnitModifiers(newModifiers);
                    newModifiers.Clear();
                }
                newModifiers[modKey] = modVal;
                lastType = unitKey;
            }
            initialValues[unitKey] = new UnitModifiers(newModifiers);
        }

        return initialValues;
    }

}