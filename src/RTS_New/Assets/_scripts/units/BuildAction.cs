using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BuildAction : UnitAction
 {
     private float _efficiency = 25;
     private float _speed;
     
     public int Priority { get; } = 1;
 
     protected override void Start()
     {
         base.Start();
         _speed = _unit.GetModifierValue(Modifier.BuildSpeed);
         _unit.ModifiersUpdated += UpdateModifiers;
     }
 
     private void UpdateModifiers()
     {
         _speed = _unit.GetModifierValue(Modifier.BuildSpeed);
     }
 
     public  override bool IsActionValid(GameObject targetGo, Vector3 targetPos)
     {
         if (!targetGo) return false;
         var building = targetGo.GetComponent<Building>();
         if (!building) return false;
         var allied = building.IsAllied(_unit.Player);
         if (!allied) return false;
         StartCoroutine(Build(building, targetPos));
         return true;
 
     }
 
     IEnumerator Build(Building building, Vector3 position)
     {
         var dst = position;
         yield return StartCoroutine(MoveToPosition(dst, _agent.stoppingDistance));
         var buildingHealth = building.Health as BuildingHealth;
         _unitActions.SetState(UnitState.ACT);
         while (!buildingHealth.HealthFull)
         {
             buildingHealth.Repair(_efficiency);
             yield return new WaitForSeconds(_speed);
         }
         _unitActions.SetState(UnitState.IDLE);
     }
     
 }