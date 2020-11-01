using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OABallisticBullet : MonoBehaviour
{
    [Tooltip("When the bullet hits something")]
    [SerializeField]
    private UnityEvent onHit;

    [Tooltip("The properties of the bullet")]
    [SerializeField]
    private OABulletStats stats;

    [SerializeField]
    private Rigidbody2D rigid;

    private Vector3 direction;
    private OABulletPool pool;
    public OABulletPool Pool { get => pool; set => pool = value; }

    public void ActivateBullet(Vector3 direction, Vector3 startPos)
    {
        gameObject.SetActive(true);
        transform.position = startPos;
        this.direction = direction;
        rigid.velocity = direction * stats.speed;
    }

    void Start()
    {
        OAExtentions.AssertObjectNotNull(stats, "'stats' is not defined on OABallisticBullet");
        OAExtentions.AssertObjectNotNull(pool, "'pool' is not defined on OABallisticBullet");

        if (!rigid)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if ((pool.transform.position - transform.position).magnitude > stats.maxRange)
        {
            pool.ReturnObject(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        onHit.Invoke();
        col.gameObject.SendMessage("ApplyDamage", stats.baseDamage, SendMessageOptions.DontRequireReceiver); // TODO: this is probably terribly slow?
        pool.ReturnObject(this);
    }
}
