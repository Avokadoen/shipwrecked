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
    private OAPlayer player;
    public OAPlayer Player { get => player; }

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


    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(player, "Enemy is missing playerTransform");
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
        rb.isKinematic = col.isGrounded(gameObject.layer);

        targetDistance = player.transform.position.x - transform.position.x;
        animator.SetBool("isInAttackRange", Mathf.Abs(targetDistance) < attackStats.range);

        animator.SetInteger("health", selfKillable.Health);

    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    col.collider.IsTouchingLayers(buildingMask);
    //}
}
