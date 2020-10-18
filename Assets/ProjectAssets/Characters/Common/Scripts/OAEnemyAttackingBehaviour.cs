using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AttackState
{
    Attacking,
    WindingUp
}

public class OAEnemyAttackingBehaviour : StateMachineBehaviour
{
    private OAEnemySensors sensors;
    private Rigidbody2D rb;

    private float duration = 0;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO: test if this get cached between each enter
        if (!sensors)
            sensors = animator.GetComponent<OAEnemySensors>();

        if (!rb)
            rb = sensors.Rb;

        rb.velocity = new Vector2(0, rb.velocity.y);
        duration = 0;
        animator.SetBool("isAttackCompleteReady", false);
        animator.speed = sensors.AttackStats.attackSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        duration += Time.deltaTime;
        if (duration > sensors.AttackStats.attackSpeed)
        {
            // player.TakeDamage(sensors.AttackStats.damage); TODO: Check colliders in a radius around attack and apply damage
            duration = 0;
            animator.SetBool("isAttackCompleteReady", true);
            return;
        }
    }
}
