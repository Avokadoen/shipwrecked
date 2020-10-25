using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAMainMenu : MonoBehaviour
{
    public void OnPlayBtnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OnSettingBtnClick()
    {
#if UNITY_EDITOR
        Debug.Log("TODO");
#endif
    }

    public void OnQuitBtnClick()
    {
        // Only works when built
        Application.Quit();
    }
}
