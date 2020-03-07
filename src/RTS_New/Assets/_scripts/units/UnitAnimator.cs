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
        switch (state)
        {
            case UnitState.IDLE:
                _controller.SetTrigger("idle");
                break;
            case UnitState.MOVE:
                _controller.SetTrigger("move");
                break;
            case UnitState.ACT:
                _controller.SetTrigger("action");
                break;
        }
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
