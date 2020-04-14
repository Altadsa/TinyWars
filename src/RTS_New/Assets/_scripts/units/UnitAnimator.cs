using UnityEngine;
using UnityEngine.UI;

public class UnitAnimator : MonoBehaviour
{
    private Animator _controller;
    
    private void Start()
    {
        GetComponent<UnitActions>().StateUpdated += UpdateAnimator;
        _controller = GetComponentInChildren<Animator>();
    }

    private void UpdateAnimator(UnitState state)
    {
        var triggerName = "";
        switch (state)
        {
            case UnitState.IDLE:
                triggerName = "idle";
                break;
            case UnitState.MOVE:
                triggerName = "move";
                break;
            case UnitState.ACT:
                triggerName = "action";
                break;
            case UnitState.DMG:
                triggerName = "dmg";
                break;
            case UnitState.DIE:
                triggerName = "die";
                break;
        }
        Debug.Log($"Trigger Name: {triggerName}");
        _controller.SetTrigger(triggerName);
    }
}

public enum UnitState
{
    IDLE,
    MOVE,
    ACT,
    DMG,
    DIE
}
