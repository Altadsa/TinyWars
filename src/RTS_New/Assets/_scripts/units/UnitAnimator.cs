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
                Debug.Log("Idle");
                break;
            case UnitState.MOVE:
                _controller.SetTrigger("move");
                break;
            case UnitState.ACT:
                Debug.Log("action");
                _controller.SetTrigger("action");
                break;
            case UnitState.DMG:
                Debug.Log("damage");
                _controller.SetTrigger("dmg");
                break;
            case UnitState.DIE:
                Debug.Log("die");
                _controller.SetTrigger("die");
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
