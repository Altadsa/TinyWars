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

    public void AddToResources(ResourceType type, int amount)
    {
        _playerResources.AddToResources(type, amount);
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
 
    public ResourceData GetDiscrepancy(ResourceData data)
    {
        var goldDiscrepancy = data.Gold - PlayerResources.Gold;
        var lumberDiscrepancy = data.Lumber - PlayerResources.Lumber;
        var ironDiscrepancy = data.Iron - PlayerResources.Iron;
        var foodDiscrepancy = data.Food + PlayerResources.Food;
        foodDiscrepancy = foodDiscrepancy - MaxFood > 0 ? foodDiscrepancy - MaxFood : 0;
        return new ResourceData(goldDiscrepancy,
            lumberDiscrepancy,
            ironDiscrepancy,
            foodDiscrepancy);
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
        var dataString = "";
        dataString += Gold > 0 ? $"\n\tGold: {Gold}" : "";
        dataString += Lumber > 0 ? $"\n\tLumber: {Lumber}" : "";
        dataString += Iron > 0 ? $"\n\tIron: {Iron}" : "";
        dataString += Food > 0 ? $"\n\tFood: {Food}" : "";
        return dataString;
    }

    public ResourceData GetDiscrepancy(ResourceData data)
    {
        var goldDiscrepancy = data.Gold - Gold;
        var lumberDiscrepancy = data.Lumber - Lumber;
        var ironDiscrepancy = data.Iron - Iron;
        var foodDiscrepancy = data.Food;
        return new ResourceData(goldDiscrepancy,
            lumberDiscrepancy,
            ironDiscrepancy,
            foodDiscrepancy);
    }
    
}

public enum ResourceType
{
    Gold,
    Lumber,
    Iron,
    Food
}