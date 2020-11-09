using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAMessageEventBehaviour : StateMachineBehaviour
{
    [SerializeField]
    string eventName = "onX";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(eventName, false);
    }
}
