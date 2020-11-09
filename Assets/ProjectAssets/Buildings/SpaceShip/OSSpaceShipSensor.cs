using System;
using UnityEngine;

public class OSSpaceShipSensor : MonoBehaviour
{
    [Tooltip("Animator used to send messages to the UI")]
    [SerializeField]
    Animator spaceShipMessager;

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
        spawner.OnEradicated.AddListener(() => spaceShipMessager.SetBool("onWipeOut", true));
        ocean.OnHighTide.AddListener(() => spaceShipMessager.SetBool("onHighTide", true));
        ocean.OnLowTide.AddListener(() => spaceShipMessager.SetBool("onLowTide", true));

        // TODO: this needs a cooldown
        playerHealth.OnHurtHealth.AddListener((health) => spaceShipMessager.SetBool("onPlayerhurt", health <= 30));

        spaceShipMessager.SetBool("isEmotionOverride", true);
        spaceShipMessager.SetInteger("emotionOverrideValue", (int) OAMessageBroadcastBehaviour.ShipEmotion.Dead);
        shipArea.OnBuilt.AddListener(() => spaceShipMessager.SetBool("isEmotionOverride", false));
    }

    public void OnTriggerEnterPOI(string identifier)
    {
        HandleTriggerEvent(identifier, true);   
    }

    public void OnTriggerExitPOI(string identifier)
    {
        HandleTriggerEvent(identifier, false);
    }

    void HandleTriggerEvent(string identifier, bool isEnter)
    {
        switch (identifier)
        {
            case "boat":
                spaceShipMessager.SetBool("playerSeesBoat", isEnter);
                break;
            case "cave":
                spaceShipMessager.SetBool("playerInCave", isEnter);
                break;
            case "sand":
                spaceShipMessager.SetBool("playerSeesSandHills", isEnter);
                break;
            case "mountain":
                spaceShipMessager.SetBool("playerSeesMountain", isEnter);
                break;
        }
    }
}
