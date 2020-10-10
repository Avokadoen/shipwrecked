using System.Collections;
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

    [Tooltip("Flip the player sprite when the horizontal movement is positive")]
    [SerializeField]
    private bool flipOnHorizontalPositive = true;

    private float horizontal;
    private float vertical;

    // TODO: Both player and AnimatorUpdater has this, find a way of deduplicate
    [SerializeField]
    private float deadZone = 0.001f;

    private bool settingKinematic = false;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void Awake()
    {
        OAExtentions.AssertObjectNotNull(moveStats, "Player is missing moveStats!");
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
        // TODO: this distrupt collision resolution of rigid body. Use a coroutine
        //       to let the rigid resolve colission before doing 
        var isGrounded = capCollider.isGrounded(gameObject.layer, 0.03f);
        if (isGrounded)
        {
            SetKinematic();
        }
        else
        {
            rigid.isKinematic = false;
        }

        Vector2 velocity = rigid.velocity;
        // Handle horizontal input
        if (horizontal > deadZone)
        {
            var speed = Mathf.Min(rigid.velocity.x + moveStats.movementSpeed, moveStats.maxSpeed);
            velocity.x = speed;
            spriteRenderer.flipX = flipOnHorizontalPositive;
        }
        else if (horizontal < -deadZone)
        {
            var speed = Mathf.Max(rigid.velocity.x - moveStats.movementSpeed, -moveStats.maxSpeed);
            velocity.x = speed;
            spriteRenderer.flipX = !flipOnHorizontalPositive;
        }

        // Handle vertical input
        if (vertical > deadZone && isGrounded)
        {
            velocity.y = moveStats.jumpForce;
        }

        rigid.velocity = velocity;
    }

    // hack to let rigid resolve collision before turning of simulation
    IEnumerator SetKinematic()
    {
        if (settingKinematic)
            yield break;

        settingKinematic = true;
        yield return new WaitForFixedUpdate();
        rigid.isKinematic = true;
        rigid.velocity = Vector2.zero;
        settingKinematic = false;
        yield return null;
    }
}
