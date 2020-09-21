using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OARangedWeapon : MonoBehaviour
{
    [SerializeField]
    private OABulletPool bullets;

    [SerializeField]
    [Tooltip("How many seconds before the weapon can fire again")] // TODO: improve tooltip, rename variable
    private float fireCooldown = 0.5f;

    [SerializeField]
    [Tooltip("Used to offset")] // TODO: improve tooltip, rename variable
    private float bulletSpawnXOffset = 10.0f;

    [SerializeField]
    [Tooltip("Used to offset spawn in world y axis")]
    private float bulletSpawnYOffset = 1.0f;

    private float lastFireCounter = 0.0f;


    void Awake()
    {
        OAExtentions.AssertObjectNotNull(bullets, "OARangedWeapon is missing OABulletPool!");
        lastFireCounter = fireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // We don't allow infinte number incrementaiton!
        lastFireCounter = Mathf.Min(lastFireCounter + Time.deltaTime, fireCooldown + 1);
        if (Input.GetButton("Fire1") && lastFireCounter >= fireCooldown)
        {
            lastFireCounter = 0f;
            var bullet = bullets.GetNext();
            if (bullet)
            {
                var direction = Vector3.right.Rotate2D(transform.eulerAngles.z);
                Vector3 spawn = transform.position + (direction * bulletSpawnXOffset);
                spawn.y += bulletSpawnYOffset;
                bullet.ActivateBullet(direction, spawn);
            }
        }
    }
}
