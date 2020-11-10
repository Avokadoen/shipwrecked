using System;
using System.Collections.Generic;
using UnityEngine;

using PoiId = OAPointOfInterest.PoiId;

// TODO: event timers should be their own class or something

public class OASpaceShipSensor : MonoBehaviour
{
    Animator spaceshipMessager;

    [SerializeField]
    OATideAnimator ocean;

    [SerializeField]
    OAEnemySpawner spawner;

    [SerializeField]
    OAKillable playerHealth;

    [SerializeField]
    List<OAKillable> shipHealth;

    [SerializeField]
    OABuildingArea shipArea;

    [SerializeField]
    float onTakingDamageCooldownTime = 40;
    float takingDamageCDCounter = 0f;

    [SerializeField]
    float onPlayerHurtCooldownTime = 40;
    float playerHurtCDCounter = 0f;


    // Start is called before the first frame update
    void Start()
    {
        takingDamageCDCounter = onTakingDamageCooldownTime;
        playerHurtCDCounter = onPlayerHurtCooldownTime;

        spaceshipMessager = GameObject.FindGameObjectWithTag("MessageBroadcaster").GetComponent<Animator>();

        spawner.OnEradicated.AddListener(() => spaceshipMessager.SetBool("onWipeOut", true));
        ocean.OnHighTide.AddListener(() => spaceshipMessager.SetBool("onHighTide", true));
        ocean.OnLowTide.AddListener(() => spaceshipMessager.SetBool("onLowTide", true));

        playerHealth.OnHurtHealth.AddListener((health) => {
            if (playerHurtCDCounter < onPlayerHurtCooldownTime)
                return;

            spaceshipMessager.SetBool("onPlayerHurt", health <= 30);
            playerHurtCDCounter = 0;
        });

        foreach(var health in shipHealth)
        {
            health.OnHurt.AddListener(() => {
                if (takingDamageCDCounter < onTakingDamageCooldownTime)
                    return;

                spaceshipMessager.SetBool("onTakingDamage", true);
                takingDamageCDCounter = 0;
            });
        }

        // If the ship gets totally destroyed
        shipHealth[0].OnDeath.AddListener(() => spaceshipMessager.SetBool("onShipDead", true));
    }

    void Update()
    {
        takingDamageCDCounter = Mathf.Min(Time.deltaTime + takingDamageCDCounter, onTakingDamageCooldownTime + 1f);
        playerHurtCDCounter = Mathf.Min(Time.deltaTime + playerHurtCDCounter, onPlayerHurtCooldownTime + 1f);
    }

    public void OnTriggerEnterPOI(PoiId identifier)
    {
        HandleTriggerEvent(identifier, true);   
    }

    public void OnTriggerExitPOI(PoiId identifier)
    {
        HandleTriggerEvent(identifier, false);
    }

    void HandleTriggerEvent(PoiId identifier, bool isEnter)
    {
        switch (identifier)
        {
            case PoiId.Boat:
                spaceshipMessager.SetBool("playerSeesBoat", isEnter);
                break;
            case PoiId.Cave:
                spaceshipMessager.SetBool("playerInCave", isEnter);
                break;
            case PoiId.Sand:
                spaceshipMessager.SetBool("playerSeesSandHills", isEnter);
                break;
            case PoiId.Mountain:
                spaceshipMessager.SetBool("playerSeesMountain", isEnter);
                break;
            case PoiId.BlueShellTut:
                spaceshipMessager.SetBool("playerSeesBlueShell", isEnter);
                break;
        }
    }
}
