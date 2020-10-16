using System.Collections;
using UnityEngine;

// TODO: when player release button and is grounded, (s)he should stop instantly 
// TODO: I should REALLY parts of this code with the enemy sensor logic ... (Rigidbody toggle logic)

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class OAPlayerMovement : MonoBehaviour
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

    [SerializeField]
    private Animator animator;

    [Tooltip("Flip the player sprite when the horizontal movement is positive")]
    [SerializeField]
    private bool flipOnHorizontalPositive = true;

    private float horizontal;
    private float vertical;

    // TODO: Both player and AnimatorUpdater has this, find a way of deduplicate
    [SerializeField]
    private float deadZone = 0.001f;

    void Awake()
    {
        OAExtentions.AssertObjectNotNull(moveStats, "Player is missing moveStats!");

        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (!rigid)
            rigid = GetComponent<Rigidbody2D>();

        if (!capCollider)
            capCollider = GetComponent<CapsuleCollider2D>();

        if (!animator)
            animator = GetComponent<Animator>();
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
        var isGrounded = capCollider.isGrounded(gameObject.layer, 0.2f);
        if (isGrounded && !rigid.isKinematic)
        {
            StartCoroutine(SetKinematic());
        }
        else if (!isGrounded)
        {
            rigid.isKinematic = false;
        }

        Vector2 velocity = rigid.velocity;
        velocity.x = 0f;
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


        animator.SetBool("isGrounded", isGrounded);

        if (rigid.velocity.x > deadZone || rigid.velocity.x < -deadZone)
        {
            animator.SetBool("hasHorizontalMovement", true);
        }
        else
        {
            animator.SetBool("hasHorizontalMovement", false);
        }

        if (rigid.velocity.y > deadZone || rigid.velocity.y < -deadZone) // TODO: falling or jumping
        {
            animator.SetBool("hasVerticalMovement", true);
            animator.SetBool("isVerticalMovUpwards", rigid.velocity.y > 0);
        }
        else
        {
            animator.SetBool("hasVerticalMovement", false);
        }
    }

    // TODO: hack to let rigid resolve collision before turning of simulation
    //       figure out a less hacky solution ..
    IEnumerator SetKinematic()
    {
        // Let the rigid body resolve any collision
        // We will call this 10 fixed updates to let the rigid resolve any conflict
        for (var i = 0; i < 10; i++)
        {
            yield return null;
        }

        rigid.isKinematic = true;
        rigid.velocity = Vector2.zero;
    }
}
