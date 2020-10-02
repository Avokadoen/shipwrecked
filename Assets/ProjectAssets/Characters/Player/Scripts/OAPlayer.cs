using UnityEngine;

// TODO: when player release button and is grounded, (s)he should stop instantly 

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
public class OAPlayer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Rigidbody2D rigid;

    [SerializeField]
    private CapsuleCollider2D capCollider;

#pragma warning disable CS0649  // Never assigned warning
    [SerializeField]
    private OAMovingEntity moveStats;
#pragma warning restore CS0649  // Never assigned warning

#pragma warning disable CS0649  // Never assigned warning
    [SerializeField]
    private OAHealth healthStat;
    private int health;
#pragma warning restore CS0649  // Never assigned warning

    private float horizontal;
    private float vertical;

    // TODO: Both player and AnimatorUpdater has this, find a way of deduplicate
    [SerializeField]
    private float deadZone = 0.001f;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void Awake()
    {
        OAExtentions.AssertObjectNotNull(moveStats,  "Player is missing moveStats!");
        OAExtentions.AssertObjectNotNull(healthStat, "Player is missing healthStat!");

        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (!rigid)
            rigid = GetComponent<Rigidbody2D>();

        if (!capCollider)
            capCollider = GetComponent<CapsuleCollider2D>();

        health = healthStat.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        var isGrounded = capCollider.isGrounded();
        if (isGrounded)
        {
            rigid.isKinematic = true;
            rigid.velocity = Vector2.zero;
        } else
        {
            rigid.isKinematic = false;
        }

        Vector2 velocity = rigid.velocity;
        // Handle horizontal input
        if (horizontal > deadZone)
        {
            var speed = Mathf.Min(rigid.velocity.x + moveStats.movementSpeed, moveStats.maxSpeed);
            velocity.x = speed;
            spriteRenderer.flipX = false;
        }
        else if (horizontal < -deadZone)
        {
            var speed = Mathf.Max(rigid.velocity.x - moveStats.movementSpeed, -moveStats.maxSpeed);
            velocity.x = speed;
            spriteRenderer.flipX = true;
        }
 
        // Handle vertical input
        if (vertical > deadZone && isGrounded)
        {
            velocity.y = moveStats.jumpForce;
        }
        
        rigid.velocity = velocity;
    }
}
