using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAWin : MonoBehaviour
{
    [SerializeField]
    GameObject vmCamPlayer;
    public GameObject VmCamPlayer { get => vmCamPlayer; }

    [SerializeField]
    GameObject vmCamShip;
    public GameObject VmCamShip { get => vmCamPlayer; }

    [SerializeField]
    OAPlayerStateStore playerState;
    public OAPlayerStateStore PlayerState { get => playerState; }

    private GameObject hud;
    public GameObject Hud { get => hud; }

    // Start is called before the first frame update
    void Start()
    {
        vmCamShip.SetActive(true);

        vmCamPlayer.SetActive(false);
        playerState.gameObject.SetActive(false);

        // TODO: This is kinda hacky ...
        hud = GameObject.FindGameObjectWithTag("HUD");
        hud.GetComponent<OAHUDEquipmentAssigner>().SetActiveEquipmentHud(false);
    }
}
