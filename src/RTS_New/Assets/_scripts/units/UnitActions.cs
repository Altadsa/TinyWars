using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    private List<IUnitAction> actions;

    private void Start()
    {
        actions = GetComponents<IUnitAction>().ToList();
        //wDebug.Log(actions.Length);
    }

    public void DetermineAction(RaycastHit actionTarget)
    {
        foreach (var unitAction in actions)
        {
            if (unitAction.IsActionValid(actionTarget)) return;
        }
    }
}