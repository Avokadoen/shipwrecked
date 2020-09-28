using UnityEngine;

public enum NPCState
{
    Runnig = 0,
    Attacking = 1
}

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent((typeof(Animator)))]
public class OAEnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 1;

    [SerializeField]
    private OAPlayer player;

    [SerializeField]
    private Transform shipTransform;

    [SerializeField]
    private Rigidbody2D rigid2D;

    [SerializeField]
    private Collider2D myCollider;

    [SerializeField]
    private OAMovingEntity moveStats;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // We start by checking if members are assigned

        animator.SetInteger("State", (int) NPCState.Runnig);

        OAExtentions.AssertObjectNotNull(player, "Enemy is missing playerTransform");
        OAExtentions.AssertObjectNotNull(shipTransform, "Enemy is missing shipTransform");

        if (!rigid2D)
        {
            rigid2D = GetComponent<Rigidbody2D>();
        }

        if (!myCollider)
        {
            myCollider = GetComponent<Collider2D>();
        }

        OAExtentions.AssertObjectNotNull(moveStats, "Enemy is missing MovingEntity");

        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // rigid2D.isKinematic = collider2D.CheckIfGrounded();

        // TODO: fix this ugly mess
        float distance = player.transform.position.x - transform.position.x;
        if (Mathf.Abs(distance) < attackRange)
        {
            animator.SetInteger("State", (int) NPCState.Attacking);
            rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        } else
        {
            animator.SetInteger("State", (int) NPCState.Runnig);
            // TODO: Math, not if pls
            float xDir = (distance < 0) ? -1 : 1;
            float speed = 0.0f;
            if (xDir > 0)
            {
                speed = Mathf.Min(moveStats.maxSpeed, rigid2D.velocity.x + xDir * moveStats.movementSpeed);
                spriteRenderer.flipX = true;
            }
            else if (xDir < 0)
            {
                speed = Mathf.Max(-moveStats.maxSpeed, rigid2D.velocity.x + (xDir * moveStats.movementSpeed));
                spriteRenderer.flipX = false;
            }
            rigid2D.velocity = new Vector2(speed, rigid2D.velocity.y).normalized;
        }

        
    }
}
