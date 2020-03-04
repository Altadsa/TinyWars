using System;
using System.Collections.Generic;
using System.IO;

public class Requirements
{
    private Dictionary<BuildingType, bool> _playerRequirements;
    
    public Requirements()
    {
        _playerRequirements = Initialise();
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