using UnityEngine;

public class UnitActions : MonoBehaviour
{
    private IUnitAction[] actions;

    private void Start()
    {
        actions = GetComponents<IUnitAction>();
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