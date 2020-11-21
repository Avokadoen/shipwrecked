using System.Collections;
using UnityEngine;

// Used to hold all shared state for the player. It also updates the animator and selects the correct
// movement component
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
[RequireComponent(typeof(OAPlayerSwimMovement), typeof(OAPlayerLandMovement), typeof(OAPlayerEquipment))]
public class OAPlayerStateStore : MonoBehaviour
{
    [Tooltip("How much input you need to consider it a input signal")]
    public float deadZone = 0.001f;

    [Tooltip("Flip the player sprite when the horizontal movement is positive")]
    public bool flipOnHorizontalPositive;

    [Tooltip("Modifier on speed in y direction when swimming")]
    public float verticalSwimModifier = 0.2f;

    [Tooltip("Modifier on speed in x direction when swimming")]
    public float horizontalSwimModifier = 4f;

    // Get set to hide it in inspector
    private bool isUnderWater = false;
    public bool IsUnderWater { get => isUnderWater; set => isUnderWater = value; }

    public bool IsGrounded { get => groundContactCount > 0; }

    private float horizontal = 0f;
    public float Horizontal { get => horizontal; set => horizontal = value; }

    private float vertical = 0f;
    public float Vertical { get => vertical; set => vertical = value; }
    
    // Variables used to update state in animator
    private Animator animator;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capCollider;

    // Player systems
    private OAPlayerLandMovement landMovement;
    private OAPlayerSwimMovement swimMovement;
    private OAPlayerEquipment playerEquipment;

    private LayerMask groundLayer;
    private int groundContactCount = 0;

    public void SetIsUnderWater(bool isUnderWater)
    {
        this.isUnderWater = isUnderWater;
        animator.SetBool("isUnderWater", isUnderWater);
        landMovement.enabled = !isUnderWater;
        swimMovement.enabled = isUnderWater;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();

        landMovement = GetComponent<OAPlayerLandMovement>();
        swimMovement = GetComponent<OAPlayerSwimMovement>();
        playerEquipment = GetComponent<OAPlayerEquipment>();

        SetIsUnderWater(isUnderWater);
        playerEquipment.SetEquipmentIndex(-1);

        groundLayer = LayerMask.NameToLayer("Ground");

        // This fixes a bug that only occurs when we build (Currently only tested WebGL) where gravity
        // does not affect the player. Probably some race-ish problem with land vs swim movement
        rigid.isKinematic = false;
        rigid.gravityScale = 1;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Update animator
        animator.SetBool("isGrounded", IsGrounded);

        if ((rigid.velocity.x > deadZone || rigid.velocity.x < -deadZone) || horizontal != 0)
        {
            animator.SetBool("hasHorizontalMovement", true);
        }
        else
        {
            animator.SetBool("hasHorizontalMovement", false);
        }

        if ((rigid.velocity.y > deadZone || rigid.velocity.y < -deadZone) || vertical != 0)
        {
            animator.SetBool("hasVerticalMovement", true);
            animator.SetBool("isVerticalMovUpwards", rigid.velocity.y > 0);
        }
        else
        {
            animator.SetBool("hasVerticalMovement", false);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == groundLayer)
        {
            groundContactCount += 1;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (col.gameObject.layer == groundLayer)
        {
            StartCoroutine(SmoothIsGrounded());
        }
    }

    // TODO: Hack as edge colliders are very buggy
    // Use something else than a edge collider
    IEnumerator SmoothIsGrounded()
    {
        yield return new WaitForSeconds(0.1f);
        groundContactCount -= 1;
    }
}
