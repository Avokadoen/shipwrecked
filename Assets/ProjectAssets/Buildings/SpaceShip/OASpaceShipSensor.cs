using System;
using UnityEngine;

using PoiId = OAPointOfInterest.PoiId;

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
    OABuildingArea shipArea;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameObject.FindGameObjectWithTag("MessageBroadcaster"));
        spaceshipMessager = GameObject.FindGameObjectWithTag("MessageBroadcaster").GetComponent<Animator>();

        spawner.OnEradicated.AddListener(() => spaceshipMessager.SetBool("onWipeOut", true));
        ocean.OnHighTide.AddListener(() => spaceshipMessager.SetBool("onHighTide", true));
        ocean.OnLowTide.AddListener(() => spaceshipMessager.SetBool("onLowTide", true));

        // TODO: this needs a cooldown
        playerHealth.OnHurtHealth.AddListener((health) => spaceshipMessager.SetBool("onPlayerhurt", health <= 30));

        spaceshipMessager.SetBool("isEmotionOverride", true);
        spaceshipMessager.SetInteger("emotionOverrideValue", (int) OAMessageBroadcastBehaviour.ShipEmotion.Dead);
        shipArea.OnBuilt.AddListener(() => spaceshipMessager.SetBool("isEmotionOverride", false));
    }

    public void OnTriggerEnterPOI(PoiId identifier)
    {
        Debug.Log(GameObject.FindGameObjectWithTag("MessageBroadcaster"));
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
        }
    }
}
