using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: we can get most of this from player state
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(OAPlayerStateStore))]
public class OAPlayerLandMovement : MonoBehaviour
{
    [Tooltip("Move stats for the player on land")]
    [SerializeField]
    private OAMovingEntity moveStats;

    [Tooltip("deadZone for input and used to determine if the player is still")]
    [SerializeField]
    private float deadZone = 0.01f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private OAPlayerStateStore ss;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        ss = GetComponent<OAPlayerStateStore>();
    }

    void FixedUpdate()
    {
        if (ss.IsGrounded && !rigid.isKinematic)
        {
            StartCoroutine(SetKinematic());
        }
        else if (!ss.IsGrounded)
        {
            rigid.isKinematic = false;
        }

        Vector2 velocity = rigid.velocity;
        velocity.x = 0f;
        // Handle horizontal input
        if (ss.Horizontal > ss.deadZone)
        {
            var speed = Mathf.Min(rigid.velocity.x + moveStats.movementSpeed, moveStats.maxSpeed);
            velocity.x = speed;
            spriteRenderer.flipX = ss.flipOnHorizontalPositive;
        }
        else if (ss.Horizontal < -ss.deadZone)
        {
            var speed = Mathf.Max(rigid.velocity.x - moveStats.movementSpeed, -moveStats.maxSpeed);
            velocity.x = speed;
            spriteRenderer.flipX = !ss.flipOnHorizontalPositive;
        }

        // Handle vertical input
        if (ss.Vertical > ss.deadZone && ss.IsGrounded)
        {
            velocity.y = moveStats.jumpForce;
        }

        rigid.velocity = velocity;
    }

    void OnEnable()
    {
        rigid.isKinematic = true;
        rigid.gravityScale = 1;
        rigid.drag = 0f;
        rigid.mass = 1f;
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
