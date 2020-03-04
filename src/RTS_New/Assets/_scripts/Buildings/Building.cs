using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BuildingHealth))]
[RequireComponent(typeof(NavMeshObstacle))]
public class Building : Entity
{
    [SerializeField] private BuildingData _buildingData;
    [SerializeField] private Queueable[] _buildingMenuItems;

    public event Action BuildingDataUpdated;
    
    private bool _constructed = true;

    [SerializeField] private Transform _unitSpawn;
    public Transform UnitSpawn => _unitSpawn;
    public Vector3 RallyPoint { get; private set; }
    
    private void Awake()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        RallyPoint = _unitSpawn.position;
        Debug.DrawLine(RallyPoint, Vector3.up*100, Color.blue, 100);
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
    }

    public Queueable[] MenuItems => _constructed ? _buildingMenuItems : null;

    public void ReplaceItem(Queueable oldItem, Queueable newItem)
    {
        var replaceIndex = Array.IndexOf(_buildingMenuItems, oldItem);
        _buildingMenuItems[replaceIndex] = newItem;
    }

    public void SetNewData(BuildingData data)
    {
        _buildingData = data;
        BuildingDataUpdated?.Invoke();
    }
    
    public void SetRallyPoint(Vector3 pos)
    {
        RallyPoint = pos;
    }
    
    public void SetConstruction()
    {
        gameObject.AddComponent<BuildingConstruction>();
    }

    public Vector3 GetSize()
    {
        return GetComponentInChildren<BoxCollider>().size;
    }

    public BuildingData BuildingData => _buildingData;

    public BuildingQueue GetQueue()
    {
        if (!_constructed) return null;
        return GetComponent<BuildingQueue>();
    }

    private void OnDestroy()
    {
        BuildingDataUpdated = null;
        _pc.RemoveSelected(this);
    }
}
