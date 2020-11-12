using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameSceneBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
