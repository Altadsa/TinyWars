using System;
using System.Collections;
using UnityEngine;

public abstract class BuildingMenuItem : MenuData
{
    public abstract void Complete(Building building);
}

public abstract class Queueable : BuildingMenuItem, IResourceCost
{
    //TODO Add resource costs to queueable items.
    [Tooltip("The time it will take for the building to produce the item.")]
    [SerializeField] private double _time;
    
    [Tooltip("The requirements that must be met by the player in order to add the item to the queue.")] [SerializeField]
    private BuildingType[] _requirements;

    [SerializeField] private int _goldCost;
    [SerializeField] private int _lumberCost;
    [SerializeField] private int _ironCost;
    [SerializeField] private int _foodCost;
    
    public ResourceData Data => new ResourceData(_goldCost, 
        _lumberCost,
        _ironCost,
        _foodCost);

    public double Time => _time;

    public BuildingType[] Requirements => _requirements;
    
    
}