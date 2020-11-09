using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OATutorialBehaviour : StateMachineBehaviour
{
    readonly string messageStage = "messageStage";

    bool hasIncremented = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasIncremented)
            return;

        var step = animator.GetInteger(messageStage);
        animator.SetInteger(messageStage, step + 1);

        hasIncremented = true;
    }
}
