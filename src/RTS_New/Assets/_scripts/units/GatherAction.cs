using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GatherAction : UnitAction
{
    private float _gatherSpeed;
    private int _resourceCapacity = 10;
    private int _resourceCount = 0;
    
    public int Priority { get; } = 1;

    private Player _player;

    private SelectionController _sc;
    
    private ResourceType _resourceType;

    private Resource _currentResource;

    private Building _dropOffBuilding;

    private static readonly BuildingType[] GoldDrop = {BuildingType.KEEP, BuildingType.TOWNHALL, BuildingType.CASTLE};
    private static readonly BuildingType[] IronDrop = {BuildingType.BLACKSMITH};
    private static readonly BuildingType[] LumberDrop = {BuildingType.LUMBERMILL};

    protected override void Start()
    {
        base.Start();
        _player = _unit.Player;
        _sc = FindObjectsOfType<PlayerController>()
            .FirstOrDefault(p => p.Player == _player)
            .SelectionController;
        _gatherSpeed = _unit.GetModifierValue(Modifier.GatherSpeed);
        _resourceCapacity = (int)_unit.GetModifierValue(Modifier.ResourceCapacity);
        _unit.ModifiersUpdated += UpdateModifiers;
    }

    private void UpdateModifiers()
    {
        _gatherSpeed = _unit.GetModifierValue(Modifier.BuildSpeed);
        _resourceCapacity = (int)_unit.GetModifierValue(Modifier.ResourceCapacity);
    }

    private void TargetResourceDepleted()
    {
        _currentResource = null;
        StopAllCoroutines();
        if (_agent.hasPath)
            _agent.SetDestination(transform.position);
        _unitActions.SetState(UnitState.IDLE);
        if (_resourceCount > 0)
            StartCoroutine(StoreResource());
    }
    
    public override bool IsActionValid(GameObject targetGo, Vector3 targetPos)
    {
        if (!targetGo) return false;
        var resource = targetGo.GetComponent<Resource>();
        if (!resource) return false;
        _currentResource = resource;
        _resourceType = _currentResource.Type;
        _currentResource.OnResourceDepleted += TargetResourceDepleted;
        _dropOffBuilding = FindClosestToResource();
        StartCoroutine(Gather());
        return true;

    }

    private Building FindClosestToResource()
    {
        var buildingType = ResolveResourceBuilding();
        var resourcePoint = _currentResource.transform.position;
        var viableBuildings = _sc.Selectable.FindAll(e => e is Building building
                                                          && buildingType.Contains(building.BuildingData.BuildingType));
        if (viableBuildings.Count == 0)
            viableBuildings = _sc.Selectable.FindAll(e => e is Building building
                                                              && GoldDrop.Contains(building.BuildingData.BuildingType));
        var closest = viableBuildings.OrderBy(e => e.DistanceToPoint(resourcePoint)).First();
        return closest as Building;
    }

    private BuildingType[] ResolveResourceBuilding()
    {
        switch (_resourceType)
        {
            case ResourceType.Iron:
                return IronDrop;
            case ResourceType.Lumber:
                return LumberDrop;
            default:
                return GoldDrop;
        }
    }
    
    IEnumerator Gather()
    {
        var dst = _currentResource.transform.position;
        yield return StartCoroutine(MoveToPosition(dst, _agent.stoppingDistance));
        var currentCarryWeight = 0;
        var resourceWeight = _currentResource.Weight;
        _unitActions.SetState(UnitState.ACT);
        while (currentCarryWeight + resourceWeight <= _resourceCapacity)
        {
            yield return new WaitForSeconds(_gatherSpeed);
            
            currentCarryWeight += resourceWeight;
            _resourceCount += _currentResource.Gather();
        }

        StartCoroutine(StoreResource());
    }

    IEnumerator StoreResource()
    {
        var dst = _dropOffBuilding.transform.position;
        yield return StartCoroutine(MoveToPosition(dst, _agent.stoppingDistance));
        _unitActions.SetState(UnitState.IDLE);
        _player.AddToResources(_resourceType, _resourceCount);
        _resourceCount = 0;
        if (_currentResource)
            StartCoroutine(Gather());
    }

}