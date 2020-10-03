using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAEnemyWindBehaviour : StateMachineBehaviour
{
    private OAEnemySensors sensors;
    private float duration;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO: test if this get cached between each enter
        if (!sensors)
            sensors = animator.GetComponent<OAEnemySensors>();

        animator.SetBool("isAttackCompleteReady", true);
        animator.speed = 1 / sensors.AttackStats.attackCooldown;
        duration = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        duration += Time.deltaTime;
        if (duration >= sensors.AttackStats.attackCooldown)
        {
            animator.SetBool("isAttackCompleteReady", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
