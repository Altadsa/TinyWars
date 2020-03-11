using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Requirements
{
    private Dictionary<BuildingType, bool> _playerRequirements;
    
    public Requirements()
    {
        _playerRequirements = Initialise();
    }

    public void SetRequirementMet(BuildingType requirement)
    {
        _playerRequirements[requirement] = true;
        Debug.LogFormat("Requirement Met: {0}", requirement);
    }
    
    public bool RequirementsMet(BuildingType[] requirements)
    {
        foreach (var buildingType in requirements)
        {
            if (!_playerRequirements[buildingType])
                return false;
        }

        return true;
    }
    
    private Dictionary<BuildingType, bool> Initialise()
    {
        var _initialRequirements = new Dictionary<BuildingType, bool>();
        using (var sr = new StreamReader("Assets/Resources/requirements.txt"))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var keyVal = line.Split('=');
                var key = (BuildingType)Enum.Parse(typeof(BuildingType),keyVal[0]);
                _initialRequirements[key] = Boolean.Parse(keyVal[1]);
            }
        }

        return _initialRequirements;
    }
}