using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CapsuleCollider2D))]
public class OAPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

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
    private float deadZone = 0.001f;

    private float horizontal;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {
        if (!animator) 
        {
            animator = GetComponent<Animator>(); 
        }

        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (!rigid)
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        if (!capCollider)
        {
            capCollider = GetComponent<CapsuleCollider2D>();
        }

        if (!moveStats)
        {
            Debug.LogError("Player is missing moveStats!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

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
        }
        else
        {
            animator.SetBool("hasVerticalMovement", false);
        }
    }

    void FixedUpdate()
    {
        var isGrounded = capCollider.CheckIfGrounded();
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
