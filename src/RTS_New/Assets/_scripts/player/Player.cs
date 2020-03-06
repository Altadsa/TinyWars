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
    
    public Modifiers GetUnitModifiers(UnitType unitType)
    {
        return _playerModifiers.FetchModifiers(unitType);
    }

    public Modifiers GetBuildingModifiers(BuildingType buildingType)
    {
        return _playerModifiers.FetchModifiers(buildingType);
    }
    
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
        _playerModifiers.SetModifier(type, modifier, value);
    }
}