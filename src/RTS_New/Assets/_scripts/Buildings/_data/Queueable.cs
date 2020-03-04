using System.Collections.Generic;
using UnityEngine;

public abstract class Queueable : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField] private string _desc;
    //Cost
    [SerializeField] private double _time;

    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _desc;
    public double Time => _time;
    
    public abstract void Complete(Building building);
}