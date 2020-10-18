using UnityEngine;

public class OAEnemyAttackingBehaviour : StateMachineBehaviour
{
    private OAEnemySensors sensors;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO: test if this get cached between each enter
        if (!sensors)
            sensors = animator.GetComponent<OAEnemySensors>();

        if (!rb)
            rb = sensors.Rb;

        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.speed = sensors.AttackStats.attackSpeed;
    }
}
