using System.Collections;
using UnityEngine;

// TODO: I should REALLY combine parts of this code with the enemy sensor logic ... (Rigidbody toggle logic)
// TODO: split this into water and land movement component and enable/disable them in the ocean script

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

    [Tooltip("Modifier on speed in y direction when swimming")]
    [SerializeField]
    private float verticalSwimModifier = 0.2f;

    [Tooltip("Modifier on speed in x direction when swimming")]
    [SerializeField]
    private float horizontalSwimModifier = 4f;

    private bool isUnderWater = false;

    public void SetUnderWater(bool isUnderWater)
    {
        this.isUnderWater = isUnderWater;

        if (isUnderWater)
        {
            // TODO: we should put data in a scriptable object when we split this.
            //       this way all player scripts can share state through this 
            rigid.isKinematic = false;
            rigid.gravityScale = 0;
            rigid.drag = 0.4f;
            rigid.mass = 0.2f;

            deadZone += 0.01f;
        } else
        {
            rigid.isKinematic = true;
            rigid.gravityScale = 1;
            rigid.drag = 0f;
            rigid.mass = 1f;
            deadZone -= 0.01f;
        }
    }

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
        if (isUnderWater)
        {
            UpdateInWater();
        } else
        {
            UpdateOnGround();
        }


        // Update animator
        animator.SetBool("isUnderWater", isUnderWater);
    
        if (rigid.velocity.x > deadZone || rigid.velocity.x < -deadZone)
        {
            animator.SetBool("hasHorizontalMovement", true);
        }
        else
        {
            animator.SetBool("hasHorizontalMovement", false);
        }

        if (rigid.velocity.y > deadZone || rigid.velocity.y < -deadZone)
        {
            animator.SetBool("hasVerticalMovement", true);
            animator.SetBool("isVerticalMovUpwards", rigid.velocity.y > 0);
        }
        else
        {
            animator.SetBool("hasVerticalMovement", false);
        }
    }

    private void UpdateInWater()
    {
        if (Mathf.Abs(vertical) > deadZone)
        {
            // Vertical is way stronger for some reason
            rigid.AddForce(Vector2.up * vertical * moveStats.movementSpeed * verticalSwimModifier);
        }

        if (Mathf.Abs(horizontal) > deadZone)
        {
            spriteRenderer.flipX = horizontal > 0;
            rigid.AddForce(Vector2.right * horizontal * moveStats.movementSpeed * horizontalSwimModifier);
        }
    }

    private void UpdateOnGround()
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

        // Only update this when not in water
        animator.SetBool("isGrounded", isGrounded);
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
