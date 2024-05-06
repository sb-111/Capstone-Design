using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : StateMachineBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string triggerName;
    [SerializeField] string triggerName2;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
        animator.ResetTrigger(triggerName2);

    }
}
