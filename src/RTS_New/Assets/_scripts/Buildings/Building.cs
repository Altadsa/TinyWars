﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Building : Entity
{
    [SerializeField] private BuildingData _buildingData;
    [SerializeField] private BuildingMenuItem[] _buildingMenuItems;

    private BuildingQueue _queue;
    public event Action BuildingDataUpdated;

    private Vector3 _unitSpawn;
    public Vector3 UnitSpawn => _unitSpawn;
    public Vector3 RallyPoint { get; private set; }
    
    private void Awake()
    {
        // Disable NavMeshObstacle until initialised
        var navMeshObstacle = GetComponent<NavMeshObstacle>();
        navMeshObstacle.enabled = false;

        var meshCollider = GetComponent<MeshCollider>(); 
        Size = meshCollider.bounds.size;
        //Size = Size.SwapYZ();
        meshCollider.enabled = false;
        
        _queue = GetComponent<BuildingQueue>();
        _queue.QueueChanged += UpdateBuildingData;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        GetComponentInChildren<MeshRenderer>().material = player.BuildingMaterial;
        
        // setup components
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
        if (!GetComponent<BuildingHealth>())
            gameObject.AddComponent<BuildingHealth>();
        Health = GetComponent<BuildingHealth>();       
        
        // set spawn points
        _unitSpawn = transform.position + transform.right * 3;
        RallyPoint = _unitSpawn;
        
        // Initialise Building Modifiers.
        var entityModifiers = player.GetBuildingModifiers(_buildingData.BuildingType);
        UpdateModifiers(entityModifiers.EntityModifiers);
        entityModifiers.ModifiersChanged += UpdateModifiers;

    }

    public BuildingMenuItem[] MenuItems => _queue.enabled ? _buildingMenuItems : null;

    // for setting visual bb during placement.
    public Vector3 Size { get; private set; }

    public BuildingData BuildingData => _buildingData;

    public BuildingQueue Queue => !_queue.enabled ? null : _queue;
    
    
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
    public void SetBuildingData(BuildingData data)
    {
        GetNewModifiers(data);
        BuildingDataUpdated?.Invoke();
    }

    private void GetNewModifiers(BuildingData data)
    {
        // update max food if new provided food is an increase
        var foodDifference = data.FoodProvided - _buildingData.FoodProvided;
        if (foodDifference > 0)
            Player.ChangeMaxFood(foodDifference);
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

    public void SetConstruction(bool constructed)
    {
        _queue.enabled = constructed;
        if (!constructed)
        {
            gameObject.AddComponent<BuildingConstruction>();
        }
        else
        {
            Player.ChangeMaxFood(_buildingData.FoodProvided);
            Player.SetRequirementMet(_buildingData.BuildingType);
        }
    }
    
    private void UpdateBuildingData()
    {
        BuildingDataUpdated?.Invoke();
    }
    
    private void OnDestroy()
    {
        BuildingDataUpdated = null;
        if (!_pc) return;
        Player.ChangeMaxFood(-_buildingData.FoodProvided);
        _pc.RemoveSelected(this);
    }
    
    IEnumerator SetRallyPoint()
    {
        var _rallyPoint = FindObjectOfType<BuildingRallyPoint>();
        var _camera = FindObjectOfType<CameraController>();
        Vector3 newPosHit = Vector3.zero;
        while (!_camera.BlockOnInput(0))
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
        yield return new WaitForEndOfFrame();
        RallyPoint = newPosHit;
        _rallyPoint.RallyPointSet();
        _camera.BlockRaycast(false);

    }
}
