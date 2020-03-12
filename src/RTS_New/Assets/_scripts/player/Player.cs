using UnityEngine;
using Units;

[System.Serializable]
public class Player
{
    public Color Color { get; private set; }
    
    public Material EntityMaterial { get; private set; }
    public bool IsAi { get; private set; }

    private PlayerModifiers _playerModifiers;
    private PlayerResources _playerResources;
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
        _playerResources = new PlayerResources();
        _playerRequirements = new Requirements();
    }

    public bool CanAfford(ResourceCost cost)
    {
        return _playerResources.CanAffordCost(cost);
    }
    
    /// <summary>
    /// Called by the Unit upgrades to change the value of a given modifier.
    /// </summary>
    /// <param name="type">The Unit receiving the new modifier</param>
    /// <param name="modifier">The Modifier to be changed.</param>
    /// <param name="value">The new values of the Modifier</param>
    public void ChangeModifier(UnitType type, Modifier modifier, float value)
    {
        _playerModifiers.SetModifier(type, modifier, value);
    }

    public bool RequirementsMet(BuildingType[] requirements)
    {
        return _playerRequirements.RequirementsMet(requirements);
    }

    public void SetRequirementMet(BuildingType buildingType)
    {
        _playerRequirements.SetRequirementMet(buildingType);
    }
}

public class PlayerResources
{
    private int _gold;
    private int _lumber;
    private int _iron;
    private int _food;
    
    public PlayerResources()
    {
        _gold = 100;
        _lumber = 100;
        _iron = 100;
        _food = 100;
    }

    public bool CanAffordCost(ResourceCost cost)
    {
        var enoughGold = _gold >= cost.Gold;
        var enoughLumber = _lumber >= cost.Lumber;
        var enoughIron = _iron >= cost.Iron;
        var enoughFood = _food >= cost.Lumber;
        return enoughGold && enoughLumber && enoughIron && enoughFood;
    }

    public void DeductResourceCost(ResourceCost cost)
    {
        _gold -= cost.Gold;
        _lumber -= cost.Lumber;
        _iron -= cost.Iron;
        _food += cost.Food;
    }
    
}

public struct ResourceCost
{
    
    public int Gold { get; private set; }
    public int Lumber { get; private set; }
    public int Iron { get; private set; }
    public int Food { get; private set; }
    
    public ResourceCost(int g, int l, int i, int f)
    {
        Gold = g;
        Lumber = l;
        Iron = i;
        Food = f;
    }

    public override string ToString()
    {
        return $"\n\tGold: {Gold}\n\tLumber: {Lumber}\n\tIron: {Iron}\n\tFood: {Food}";
    }
}

public enum ResourceType
{
    Gold,
    Lumber,
    Iron,
    Food
}