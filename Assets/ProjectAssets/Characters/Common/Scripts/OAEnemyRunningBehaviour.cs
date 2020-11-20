using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAEnemyRunningBehaviour : StateMachineBehaviour
{
    private OAEnemySensors sensors;
    private Rigidbody2D rb;
    private OAMovingEntity moveStats;
    private OASpriteGroupFlipper spriteGroup;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!sensors)
            sensors = animator.GetComponent<OAEnemySensors>();

        if (!spriteGroup)
            spriteGroup = animator.GetComponent<OASpriteGroupFlipper>();

        if (!rb) 
            rb = sensors.Rb;

        if (!moveStats)
            moveStats = sensors.MoveStats;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = sensors.targetDistance;

        float xDir = (distance < 0) ? -1 : 1;

        spriteGroup.FlipX(xDir > 0);
        rb.AddForce(Vector2.right * moveStats.movementSpeed * xDir, ForceMode2D.Force);
    }
}
