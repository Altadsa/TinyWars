using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Building : Entity
{
    [SerializeField] private BuildingData _buildingData;
    [SerializeField] private BuildingMenuItem[] _buildingMenuItems;
    
    public BuildingQueue Queue { get; private set; }
    public event Action BuildingDataUpdated;

    private Vector3 _unitSpawn;
    public Vector3 UnitSpawn => _unitSpawn;
    public Vector3 RallyPoint { get; private set; }
    
    private void Awake()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Queue = GetComponent<BuildingQueue>();
        Queue.QueueChanged += UpdateBuildingData;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
        _unitSpawn = transform.position + transform.right * 3;
        RallyPoint = _unitSpawn;
        // Initialise Building Modifiers.
        var entityModifiers = player.GetBuildingModifiers(_buildingData.BuildingType);
        UpdateModifiers(entityModifiers.EntityModifiers);
        entityModifiers.ModifiersChanged += UpdateModifiers;
        if (!GetComponent<BuildingHealth>())
            gameObject.AddComponent<BuildingHealth>();
        Health = GetComponent<BuildingHealth>();
    }

    public BuildingMenuItem[] MenuItems => Queue.enabled ? _buildingMenuItems : null;

    /// <summary>
    /// Replaces a completed upgrade (modifier/building) with the next one (if it exists).
    /// </summary>
    /// <param name="oldItem">The item to replace</param>
    /// <param name="newItem">The item to replace the oldItem with.</param>
    public void ReplaceItem(Queueable oldItem, Queueable newItem)
    {
        var replaceIndex = Array.IndexOf(_buildingMenuItems, oldItem);
        _buildingMenuItems[replaceIndex] = newItem;
    }

    /// <summary>
    /// Sets the new data for the building after upgrading it.
    /// </summary>
    /// <param name="data"></param>
    public void SetNewData(BuildingData data)
    {
        GetNewModifiers(data);
        BuildingDataUpdated?.Invoke();
    }

    private void GetNewModifiers(BuildingData data)
    {
        // Remove subscription to old modifier object.
        var oldModifiers = Player.GetBuildingModifiers(_buildingData.BuildingType);
        oldModifiers.ModifiersChanged -= UpdateModifiers;
        _buildingData = data;
        // Set new modifiers and subscribe to new modifier object.
        var newModifiers = Player.GetBuildingModifiers(_buildingData.BuildingType);
        UpdateModifiers(newModifiers.EntityModifiers);
        newModifiers.ModifiersChanged += UpdateModifiers;
    }

    public void SetRally()
    {
        StartCoroutine(SetRallyPoint());
    }

    public void SetConstruction()
    {
        gameObject.AddComponent<BuildingConstruction>();
        Queue.enabled = false;
    }

    public Vector3 GetSize()
    {
        return GetComponentInChildren<BoxCollider>().size;
    }

    public BuildingData BuildingData => _buildingData;

    public BuildingQueue GetQueue()
    {
        return !Queue.enabled ? null : Queue;
    }

    private void UpdateBuildingData()
    {
        BuildingDataUpdated?.Invoke();
    }
    
    private void OnDestroy()
    {
        BuildingDataUpdated = null;
        if (_pc)
            _pc.RemoveSelected(this);
    }
    
    IEnumerator SetRallyPoint()
    {
        var _rallyPoint = FindObjectOfType<BuildingRallyPoint>();
        var _camera = FindObjectOfType<CameraController>();
        Vector3 newPosHit = Vector3.zero;
        while (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(1))
            {
                _rallyPoint.RallyPointSet();
                yield break;
            }
            newPosHit = _camera.Hit.Value.point;
            _rallyPoint.SetRallyPoint(newPosHit);
            yield return null;
        }
        RallyPoint = newPosHit;
    }
}
