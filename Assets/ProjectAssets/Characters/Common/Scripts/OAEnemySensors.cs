using System.Collections;
using UnityEngine;

// TODO: rename
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class OAEnemySensors : MonoBehaviour
{
    // TODO: handle if player is too close to enemy somehow
    [SerializeField]
    private OAAttackStats attackStats = null;
    public OAAttackStats AttackStats { get => attackStats; }

    [SerializeField]
    private OAPlayerStateStore playerState;
    public OAPlayerStateStore PlayerState { get => playerState; }

    [SerializeField]
    private Transform shipTransform;

    [SerializeField]
    private Transform ratHead;

    [SerializeField]
    private OAMovingEntity moveStats = null;
    public OAMovingEntity MoveStats { get => moveStats; }

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get => rb;}

    [SerializeField]
    private Collider2D col;

    [SerializeField]
    private OAKillable selfKillable;

    private LayerMask buildingMask;

    public float targetDistance;
    private Vector3 front;

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(playerState, "Enemy is missing playerTransform");
        OAExtentions.AssertObjectNotNull(shipTransform, "Enemy is missing shipTransform");

        OAExtentions.AssertObjectNotNull(moveStats, "Enemy is missing MovingEntity");
        OAExtentions.AssertObjectNotNull(attackStats, "Enemy is missing AttackStats");

        if (!animator)
            animator = GetComponent<Animator>();

        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        if (!col)
            col = GetComponent<Collider2D>();

        if (!selfKillable)
            selfKillable = GetComponent<OAKillable>();

        buildingMask = LayerMask.NameToLayer("Building");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetDistance = playerState.transform.position.x - ratHead.position.x;
        front.x = targetDistance;
        front.Normalize();
        bool isInTargetRange = Mathf.Abs(targetDistance) < attackStats.range;
        var hit = Physics2D.Raycast(transform.position, front, attackStats.range, buildingMask);
        animator.SetBool("isInAttackRange", isInTargetRange || hit.collider);
        animator.SetInteger("health", selfKillable.Health);
    }
}
