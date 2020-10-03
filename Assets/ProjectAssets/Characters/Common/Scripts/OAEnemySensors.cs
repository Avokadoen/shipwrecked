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
    private OAHealth healthStat = null;
    public OAHealth HealthStat { get => healthStat; }
    public int health;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get => rb;}

    [SerializeField]
    private Collider2D col;

    [SerializeField]
    private OADeathHandler deathHandler;

    private SpriteRenderer spriteRenderer;

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

        if (!deathHandler)
            deathHandler = transform.parent.gameObject.GetComponent<OADeathHandler>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        health = healthStat.maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            deathHandler.OnDead(spriteRenderer);
            return;
        }

        rb.isKinematic = col.isGrounded(gameObject.layer);

        targetDistance = player.transform.position.x - transform.position.x;
        animator.SetBool("isInAttackRange", Mathf.Abs(targetDistance) < attackStats.range);

        animator.SetInteger("health", health);

    }
}
