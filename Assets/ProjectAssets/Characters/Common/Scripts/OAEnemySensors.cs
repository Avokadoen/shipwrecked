using System.Collections;
using UnityEngine;

// TODO: rename
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
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

    private SpriteRenderer spriteRenderer; // TODO: Remove?
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

        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingMask = LayerMask.GetMask("Building");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.isKinematic = col.isGrounded(gameObject.layer, 0.02f);

        targetDistance = playerState.transform.position.x - transform.position.x;
        front.x = targetDistance;
        front.Normalize();
        var hit = Physics2D.Raycast(transform.position, front, attackStats.range);
        bool isInTargetRange = Mathf.Abs(targetDistance) < attackStats.range;
        animator.SetBool("isInAttackRange", isInTargetRange || hit.collider);

        animator.SetInteger("health", selfKillable.Health);

    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    col.collider.IsTouchingLayers(buildingMask);
    //}
}
