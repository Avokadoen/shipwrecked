using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// State of game
enum GameState
{
    Running,
    Paused,
    PlayerDead
}

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

    GameState gameState = GameState.Running;

    void Start()
    {
        menu.SetActive(false);

        quitBtn.onClick.AddListener(OnQuitClick);
        retryBtn.onClick.AddListener(OnRetryClick);
        mainMenuBtn.onClick.AddListener(OnMainMenuClick);

        playerKillable = GameObject.FindWithTag("Player").GetComponent<OAKillable>();
        playerKillable.OnDeath.AddListener(OnDied);
    }

    void Update()
    {
        // Set time scale to 0 if we have paused
        Time.timeScale = (gameState == GameState.Paused) ? 0 : 1;

        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }

        switch(gameState)
        {
            case GameState.Running:
                OnPause();
                break;
            case GameState.Paused:
                OnResume();
                break;
            case GameState.PlayerDead:
                break;
        }

    }

    void OnDied()
    {
        gameState = GameState.PlayerDead;

        menu.SetActive(true);
        pauseText.SetActive(false);
        deadText.SetActive(true);
    }

    void OnPause()
    {
        gameState = GameState.Paused;

        menu.SetActive(true);
        pauseText.SetActive(true);
        deadText.SetActive(false);
    }

    void OnResume()
    {
        gameState = GameState.Running;
        menu.SetActive(false);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnRetryClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OnMainMenuClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
