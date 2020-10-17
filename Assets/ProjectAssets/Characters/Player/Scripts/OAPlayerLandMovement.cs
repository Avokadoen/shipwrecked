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

    [Tooltip("How long the player has to wait before jumping again")]
    [SerializeField]
    private float jumpCooldown = 0.3f;
    private float jumpCdCounter = 0f;

    [Tooltip("Dead zone for input and used to determine if the player is still")]
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
        // Handle horizontal input
        if (ss.Horizontal > ss.deadZone)
        {
            rigid.AddForce(Vector2.right * moveStats.movementSpeed * ss.Horizontal, ForceMode2D.Impulse);
            spriteRenderer.flipX = ss.flipOnHorizontalPositive;
        }
        else if (ss.Horizontal < -ss.deadZone)
        {
            rigid.AddForce(Vector2.right * moveStats.movementSpeed * ss.Horizontal, ForceMode2D.Impulse);
            spriteRenderer.flipX = !ss.flipOnHorizontalPositive;
        }

        jumpCdCounter += Time.fixedDeltaTime;
        // Handle vertical input
        if (ss.Vertical > ss.deadZone && ss.IsGrounded && jumpCooldown <= jumpCdCounter)
        {
            rigid.AddForce(Vector2.up * moveStats.jumpForce, ForceMode2D.Impulse);
            jumpCdCounter = 0f;
        }
    }

    void OnEnable()
    {
        rigid.gravityScale = 1f;
        rigid.drag = 0f;
    }

}
