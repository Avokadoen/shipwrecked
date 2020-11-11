using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAComicBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("getNext", false);
    }
}
