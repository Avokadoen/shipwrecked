// #define SHOW_ATTACK_AREA

using System.Collections;
using UnityEngine;

// TODO: rename
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class OAEnemySensors : MonoBehaviour
{
    // TODO: handle if player is too close to enemy somehow
    [SerializeField]
    private OAAttackStats attackStats = null;
    public OAAttackStats AttackStats { get => attackStats; }

    [SerializeField]
    private OAPlayerStateStore playerState;
    public OAPlayerStateStore PlayerState { get => playerState; set => playerState = value; }

    [SerializeField]
    private Transform shipTransform;

    [SerializeField]
    private Transform ratHead;
    public Transform RatHead { get => ratHead; }

    [SerializeField]
    private OAMovingEntity moveStats = null;
    public OAMovingEntity MoveStats { get => moveStats; }

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get => rb;}

    [SerializeField]
    private Collider2D col;

    [SerializeField]
    private OAKillable selfKillable;

    private LayerMask playerAndBuldingLayer;
    private Vector2 range;
    // We limit attack testing to 4 object. This might be more, or it migth be enough with less
    private RaycastHit2D[] hits = new RaycastHit2D[4];

    public float targetDistance;
    private Vector3 front;

    // Start is called before the first frame update
    void Start()
    {
        // OAExtentions.AssertObjectNotNull(shipTransform, "Enemy is missing shipTransform");

        OAExtentions.AssertObjectNotNull(moveStats, "Enemy is missing MovingEntity");
        OAExtentions.AssertObjectNotNull(attackStats, "Enemy is missing AttackStats");

        if (!animator)
            animator = GetComponent<Animator>();

        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        if (!col)
            col = GetComponent<Collider2D>();

        if (!selfKillable)
            selfKillable = GetComponent<OAKillable>();

        

        playerAndBuldingLayer = LayerMask.GetMask(new string[] { "Player", "Building" });
        range = new Vector2(AttackStats.range * 1.2f, AttackStats.range);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetDistance = playerState.transform.position.x - ratHead.position.x;
        front.x = targetDistance;
        front.Normalize();
        bool isInTargetRange = Mathf.Abs(targetDistance) < attackStats.range;
        var hitCount = Physics2D.RaycastNonAlloc(ratHead.position, front, hits, attackStats.range, playerAndBuldingLayer);

        animator.SetBool("isInAttackRange", isInTargetRange || hitCount > 0);
    }

    void CommitAttack()
    {
        int hitCount = Physics2D.BoxCastNonAlloc(
            RatHead.position,
            range,
            0f,
            front,
            hits,
            attackStats.range,
            playerAndBuldingLayer.value
        );

#if UNITY_EDITOR && SHOW_ATTACK_AREA
        var horizontal = new Vector3(range.x, 0);
        var vertical = new Vector3(0, -range.y);
        var dur = 0.2f;
        Debug.DrawRay(RatHead.position, horizontal, Color.red, dur);
        Debug.DrawRay(RatHead.position, vertical, Color.red, dur);
        Debug.DrawRay(RatHead.position + vertical, horizontal, Color.red, dur);
        Debug.DrawRay(RatHead.position + horizontal, vertical, Color.red, dur);

        Debug.Log(hitCount);
#endif

        for (int i = 0; i < hitCount; i++)
        {
            hits[i].collider.gameObject.SendMessage("ApplyDamage", AttackStats.damage, SendMessageOptions.DontRequireReceiver); // TODO: this is probably terribly slow?
        }
    }

}
