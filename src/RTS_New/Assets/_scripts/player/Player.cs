using System;
using UnityEngine;
using Units;

[System.Serializable]
public class Player
{
    public Color Color { get; private set; }
    
    public Material EntityMaterial { get; private set; }
    public bool IsAi { get; private set; }

    public event Action<ResourceData,int> ResourcesUpdated;
    
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

    public ResourceData PlayerResources => _playerResources.Data;
    public int MaxFood => _playerResources.MaxFood;
    public Player(Color color, Material entityMaterial, bool isAi)
    {
        Color = color;
        EntityMaterial = entityMaterial;
        IsAi = isAi;
        _playerModifiers = new PlayerModifiers();
        _playerResources = new PlayerResources();
        _playerRequirements = new Requirements();
    }

    public bool CanAfford(ResourceData data)
    {
        return _playerResources.CanAffordCost(data);
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

    public void DeductResources(ResourceData queueableData)
    {
        _playerResources.DeductResourceCost(queueableData);
        ResourcesUpdated?.Invoke(_playerResources.Data, MaxFood);
    }

    public void RefundResources(ResourceData data)
    {
        _playerResources.RefundResourceCost(data);
        ResourcesUpdated?.Invoke(_playerResources.Data, MaxFood);
    }

    public void ChangeMaxFood(int amount)
    {
        _playerResources.ChangeFoodCapacity(amount);
        ResourcesUpdated?.Invoke(_playerResources.Data, MaxFood);
    }

    public void ChangeFoodUsage(int amount)
    {
        _playerResources.ChangeFoodUsage(amount);
        ResourcesUpdated?.Invoke(_playerResources.Data, MaxFood);
    }
    
}

public class PlayerResources
{
    private int _gold;
    private int _lumber;
    private int _iron;
    private int _food;

    private int _maxFood;
    
    public PlayerResources()
    {
        _gold = 100;
        _lumber = 100;
        _iron = 100;
        _food = 0;
        _maxFood = 24;
    }

    public ResourceData Data => new ResourceData(_gold,_lumber,_iron,_food);

    public int MaxFood => _maxFood;
    
    public bool CanAffordCost(ResourceData cost)
    {
        var enoughGold = _gold >= cost.Gold;
        var enoughLumber = _lumber >= cost.Lumber;
        var enoughIron = _iron >= cost.Iron;
        var enoughFood = _food + cost.Food <= _maxFood;
        return enoughGold && enoughLumber && enoughIron && enoughFood;
    }

    public void DeductResourceCost(ResourceData cost)
    {
        _gold -= cost.Gold;
        _lumber -= cost.Lumber;
        _iron -= cost.Iron;
        _food += cost.Food;
    }

    public void RefundResourceCost(ResourceData cost)
    {
        _gold += cost.Gold;
        _lumber += cost.Lumber;
        _iron += cost.Iron;
        _food -= cost.Food;
    }

    public void ChangeFoodCapacity(int change)
    {
        _maxFood += change;
    }

    public void ChangeFoodUsage(int change)
    {
        _food += change;
    }
    
}

public struct ResourceData
{
    
    public int Gold { get; private set; }
    public int Lumber { get; private set; }
    public int Iron { get; private set; }
    public int Food { get; private set; }
    
    public ResourceData(int g, int l, int i, int f)
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