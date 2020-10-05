using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAEnemyRunningBehaviour : StateMachineBehaviour
{
    private OAEnemySensors sensors;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private OAMovingEntity moveStats;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!sensors)
            sensors = animator.GetComponent<OAEnemySensors>();

        if (!sr)
            sr = animator.GetComponent<SpriteRenderer>();

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
        float speed = 0.0f;
        if (xDir > 0)
        {
            speed = Mathf.Min(moveStats.maxSpeed, rb.velocity.x + xDir * moveStats.movementSpeed);
            sr.flipX = true;
        }
        else if (xDir < 0)
        {
            speed = Mathf.Max(-moveStats.maxSpeed, rb.velocity.x + (xDir * moveStats.movementSpeed));
            sr.flipX = false;
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
    }   
}
