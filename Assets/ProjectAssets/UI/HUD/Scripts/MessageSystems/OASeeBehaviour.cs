using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OASeeBehaviour : StateMachineBehaviour
{
    [SerializeField]
    string hasCommentedName = "hasCommentedX";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(hasCommentedName, true);
    }
}
