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

    [Tooltip("Modifier on speed in y direction when swimming")]
    [SerializeField]
    private float verticalSwimModifier = 0.2f;

    [Tooltip("Modifier on speed in x direction when swimming")]
    [SerializeField]
    private float horizontalSwimModifier = 4f;

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
        if (Mathf.Abs(ss.Vertical) > deadZone)
        {
            // Vertical is way stronger for some reason
            rigid.AddForce(Vector2.up * ss.Vertical * moveStats.movementSpeed * verticalSwimModifier);
        }

        if (Mathf.Abs(ss.Horizontal) > deadZone)
        {
            spriteRenderer.flipX = ss.Horizontal > 0;
            rigid.AddForce(Vector2.right * ss.Horizontal * moveStats.movementSpeed * horizontalSwimModifier);
        }
    }

    void OnEnable()
    {
        rigid.isKinematic = false;
        rigid.gravityScale = 0;
        rigid.drag = 0.4f;
        rigid.mass = 0.2f;
    }
}
