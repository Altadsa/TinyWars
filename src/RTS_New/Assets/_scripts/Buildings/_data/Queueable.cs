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

[CreateAssetMenu(menuName = "Data/Building Upgrade")]
public class BuildingUpgrade : Queueable
{
    [SerializeField] private GameObject _baseModel;
    [SerializeField] private GameObject _inProgress;
    [SerializeField] private GameObject _complete;

    public GameObject Base => _baseModel;
    public GameObject InProgress => _inProgress;
    public GameObject Completed => _complete;
    
    public override void Complete(Building building)
    {
        var player = building.Player;
        var oldModel = building.transform.GetChild(2);
        Destroy(oldModel);
        var newModel = Instantiate(_complete, building.transform);
        newModel.GetComponent<MeshRenderer>().material = player.EntityMaterial;
    }
}
