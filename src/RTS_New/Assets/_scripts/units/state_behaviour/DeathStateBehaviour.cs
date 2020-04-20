using UnityEngine;

public class DeathStateBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (stateInfo.normalizedTime >= 1f)
            Destroy(animator.transform.parent.gameObject);
    }
}
