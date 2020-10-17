using System.Collections;
using System.Collections.Generic;
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

    private bool isGrounded = false;
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    private float horizontal = 0f;
    public float Horizontal { get => horizontal; set => horizontal = value; }

    private float vertical = 0f;
    public float Vertical { get => vertical; set => vertical = value; }


    [Tooltip("Health stats of the player")]
    [SerializeField]
    private OAHealth healthStat = null;
    public float health = -1f;

    // Variables used to update state in animator
    private Animator animator;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capCollider;

    // Player systems
    private OAPlayerLandMovement landMovement;
    private OAPlayerSwimMovement swimMovement;
    private OAPlayerEquipment playerEquipment;

    public void SetIsUnderWater(bool isUnderWater)
    {
        this.isUnderWater = isUnderWater;
        animator.SetBool("isUnderWater", isUnderWater);
        landMovement.enabled = !isUnderWater;
        swimMovement.enabled = isUnderWater;
        playerEquipment.SetEquipmentIndex(-1);
    }

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(healthStat, "OAPlayerStateStore is missing health state!");

        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();

        landMovement = GetComponent<OAPlayerLandMovement>();
        swimMovement = GetComponent<OAPlayerSwimMovement>();
        playerEquipment = GetComponent<OAPlayerEquipment>();

        SetIsUnderWater(isUnderWater);
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        isGrounded = capCollider.isGrounded(gameObject.layer, 0.2f);

        // Update animator
        animator.SetBool("isGrounded", isGrounded);

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
}
