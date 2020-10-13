using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OABallisticBullet : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The properties of the bullet")]
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

    void OnCollisionEnter2D(Collision2D col)
    {
        col.gameObject.SendMessage("ApplyDamage", stats.baseDamage, SendMessageOptions.DontRequireReceiver); // TODO: this is probably terribly slow
        pool.ReturnObject(this);
    }
}
