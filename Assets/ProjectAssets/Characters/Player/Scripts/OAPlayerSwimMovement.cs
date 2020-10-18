using UnityEngine;

// TODO: we can get most of this from player state
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(OAPlayerStateStore))]
public class OAPlayerSwimMovement : MonoBehaviour
{
    [Tooltip("Move stats for the player on land")]
    [SerializeField]
    private OAMovingEntity moveStats;

    [Tooltip("deadZone for input and used to determine if the player is still")]
    [SerializeField]
    private float deadZone = 0.02f;

    [Tooltip("Limit in vertical speed modifier")]
    [SerializeField]
    private float verticalSwimLimit = 1.6f;

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
        if (CanApplyInput(ss.Horizontal, rigid.velocity.x))
        {
            spriteRenderer.flipX = ss.Horizontal > 0;
            rigid.AddForce(Vector2.right * ss.Horizontal * moveStats.movementSpeed, ForceMode2D.Impulse);
        }

        if (CanApplyInput(ss.Vertical, rigid.velocity.y * verticalSwimLimit))
        {
            // Vertical is way stronger for some reason
            rigid.AddForce(Vector2.up * ss.Vertical * moveStats.jumpForce, ForceMode2D.Impulse);
        }
    }

    bool CanApplyInput(float input, float currentSpeed)
    {
        // TODO: we apply force, and look at velocity. This is wrong.
        return Mathf.Abs(input) > deadZone && (currentSpeed > -moveStats.maxSpeed && currentSpeed < moveStats.maxSpeed); 
    }

    void OnEnable()
    {
        rigid.gravityScale = 0;
        rigid.drag = 0.4f;
    }
}
