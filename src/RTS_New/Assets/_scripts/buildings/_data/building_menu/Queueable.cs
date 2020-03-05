using System.Collections.Generic;
using UnityEngine;

public abstract class Queueable : ScriptableObject, IMenuItem
{
    [Tooltip("Icon to represent the item in the building menu.")]
    [SerializeField] private Sprite _icon;
    [Tooltip("Name of the item as it will appear in the building menu.")]
    [SerializeField] private string _name;
    [Tooltip("Description of the item as it will appear in the menu.")]
    [SerializeField] private string _desc;
    //TODO Add resource costs to certain queueable items.
    [Tooltip("The time it will take for the building to produce the item.")]
    [SerializeField] private double _time;

    [Tooltip("The index of the item i.e. where it will appear in the menu.")]
    [Range(0,15)]
    [SerializeField] private int _priority;

    [Tooltip("The requirements that must be met by the player in order to add the item to the queue.")] [SerializeField]
    private BuildingType[] _requirements;
    
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _desc;
    public double Time => _time;

    public int Priority => _priority;

    public BuildingType[] Requirements => _requirements;

    public abstract void Complete(Building building);
}