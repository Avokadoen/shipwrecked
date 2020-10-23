using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OAIngameMenu : MonoBehaviour
{
    [SerializeField]
    GameObject deadText;

    [SerializeField]
    GameObject pauseText;

    [SerializeField]
    GameObject menu;

    [SerializeField]
    Button quitBtn;

    [SerializeField]
    Button retryBtn;

    [SerializeField]
    Button mainMenuBtn;


    OAKillable playerKillable;

    bool hasDied = false;

    void Start()
    {
        menu.SetActive(false);

        quitBtn.onClick.AddListener(OnQuitClick);
        retryBtn.onClick.AddListener(OnRetryClick);
        mainMenuBtn.onClick.AddListener(OnMainMenuClick);

        playerKillable = GameObject.FindWithTag("Player").GetComponent<OAKillable>();
        playerKillable.AddDeathListener(OnDied);
    }

    void OnDied()
    {
        hasDied = true;

        menu.SetActive(true);
        pauseText.SetActive(false);
        deadText.SetActive(true);
    }

    void Update()
    {
        if (hasDied)
            return;
    }

    public void OnQuitClick()
    {
        Debug.Log("Quit");
    }

    public void OnRetryClick()
    {
        Debug.Log("Retry");
    }

    public void OnMainMenuClick()
    {
        Debug.Log("Main menu");
    }
}
