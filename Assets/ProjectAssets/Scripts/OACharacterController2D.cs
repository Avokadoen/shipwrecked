using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OACharacterController2D : MonoBehaviour
{
    public Vector2 velocity { get => velocityState; }
    public bool isGrounded { get => isGroundedState; }

    [Tooltip("Collider to be considered the character's collider")]
    [SerializeField]
    private Collider2D cCollider;

    [Tooltip("Slope limit determine how steep slope the character\ncan stand in before not being grounded")]
    [SerializeField]
    private float slopeLimit = 45;

    private List<Collision2D> collisions;
    private Vector3 lastFramePos;
    private Vector2 velocityState;
    private bool isGroundedState;

    void Awake()
    {
        // TODO: Maybe use array instead

        if (!cCollider)
        {
            cCollider = GetComponent<Collider2D>();
        }

        collisions = new List<Collision2D>();
        lastFramePos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 _velocity = lastFramePos - transform.position;
        velocityState = new Vector2(_velocity.x, _velocity.y);
        lastFramePos = transform.position;

        isGroundedState = isGroundedTest();
        if (isGroundedState)
        {
            Debug.Log("grounded");
        }
    }

    // TODO: optimize
    private bool isGroundedTest()
    {
        // TODO We can multithread this!
        Vector2 charPos = new Vector2(transform.position.x, transform.position.y);
        foreach (var collision in collisions)
        {
            for(var i = 0; i < collision.contactCount; i++)
            {
                var direction = (collision.GetContact(i).point - charPos).normalized;
                if (Vector3.Angle(direction, Vector3.down) < slopeLimit)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Move(Vector2 stride)
    {
        transform.Translate(new Vector3(stride.x, stride.y, 0));
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("grounded");
        collisions.Add(col);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        var index = collisions.IndexOf(col);
        if (index >= 0)
        {
            collisions.SwapRemoveAt(index);
        }
    }
}
