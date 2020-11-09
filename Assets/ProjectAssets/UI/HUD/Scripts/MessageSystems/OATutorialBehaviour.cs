using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OATutorialBehaviour : StateMachineBehaviour
{
    readonly string messageStage = "messageStage";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var step = animator.GetInteger(messageStage);
        animator.SetInteger(messageStage, step + 1);
    }
}
