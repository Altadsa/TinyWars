using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GatherAction : UnitAction
{
    private float _gatherSpeed;
    private int _resourceCapacity = 10;
    private int _resourceCount = 0;
    
    public int Priority { get; } = 1;

    private Player _player;

    private ResourceType _resourceType;

    protected override void Start()
    {
        base.Start();
        _player = _unit.Player;
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
        StartCoroutine(Gather());
        return true;

    }

    private Resource _currentResource;
    
    IEnumerator Gather()
    {
        _unitActions.SetState(UnitState.MOVE);
        var dst = _currentResource.transform.position;
        _agent.SetDestination(dst);
        yield return new WaitUntil(() => _agent.hasPath);
        yield return new WaitUntil(() => _agent.remainingDistance < _agent.stoppingDistance);
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
        var returnBuilding = FindObjectsOfType<Building>().FirstOrDefault
            (b => b.BuildingData.BuildingType == BuildingType.TOWNHALL && b.Player == _player);
        var dst = returnBuilding.transform.position;
        _unitActions.SetState(UnitState.MOVE);
        _agent.SetDestination(dst);
        yield return new WaitUntil(() => _agent.hasPath);
        yield return new WaitUntil(() => _agent.remainingDistance < _agent.stoppingDistance);
        _unitActions.SetState(UnitState.IDLE);
        _player.AddToResources(_resourceType, _resourceCount);
        _resourceCount = 0;
        if (_currentResource)
            StartCoroutine(Gather());
    }

}