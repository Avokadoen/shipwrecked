using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAMainMenu : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    float playDelay = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    bool playPressed = false;
    public void OnPlayBtnClick()
    {
        if (playPressed)
            return;

        playPressed = true;
        animator.SetBool("playPressed", playPressed);
    }

    public void OnPlayAnimationDone()
    {
        IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(playDelay);

            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        StartCoroutine(LoadGameScene());
    }

    public void OnSettingBtnClick()
    {
#if UNITY_EDITOR
        Debug.Log("TODO");
#endif
    }

    public void OnQuitBtnClick()
    {
        // TODO: fix: crashes the page in webgl target
        // Only works when built
        Application.Quit();
    }
}
