using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OARangedWeapon : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onFire = new UnityEvent();

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

    [Tooltip("How much energy the weapon generate on one second")]
    private float energyRegenRate = 0.6f;

    // This could be on the bullet itself if we had multiple bullet types
    [Tooltip("How much it cost to fire one bullet")]
    private float energyiFreCost = 0.2f;

    private float lastFireCounter = 0.0f;
    private float energy = 1f;
    private bool coolingOff = false;
    private Slider energyIndicator;

    void Awake()
    {
        OAExtentions.AssertObjectNotNull(bullets, "OARangedWeapon is missing OABulletPool!");
        lastFireCounter = fireCooldown;
    }

    private void Start()
    {
        energyIndicator = GameObject.FindGameObjectWithTag("HUD")
                                        .transform
                                        .Find("WeaponEnergy")
                                        .Find("EnergyValue")
                                        .GetComponent<Slider>();

        energyIndicator.transform.parent.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // We don't allow infinte number incrementaiton!
        lastFireCounter = Mathf.Min(lastFireCounter + Time.deltaTime, fireCooldown + 1);
        coolingOff = (coolingOff) ? energy < 1 : false;

        var offCooldown = lastFireCounter >= fireCooldown;
        if (Input.GetButton("Fire1") && offCooldown && !coolingOff) 
        {
            lastFireCounter = 0f;
            var bullet = bullets.GetNext();
            if (bullet)
            {
                onFire.Invoke();

                var direction = Vector3.right.Rotate2D(transform.eulerAngles.z);
                Vector3 spawn = transform.position + (direction * bulletSpawnXOffset);
                spawn.y += bulletSpawnYOffset;
                bullet.ActivateBullet(direction, spawn);

                energy -= energyiFreCost;
            }

            coolingOff = energy <= 0;
        } 

        // TODO: if the player is greedy and hold this to zero, then (s)he should have to wait for it to fully recharge
        energy = (offCooldown) ? Mathf.Min(1f, energy + energyRegenRate * Time.deltaTime) : energy;
        energyIndicator.value = energy;
    }

    private void OnEnable()
    {
        if (!energyIndicator)
            return;

        energyIndicator.transform.parent.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        energyIndicator.transform.parent.gameObject.SetActive(false);
    }
}
