using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAWInBehaviour : StateMachineBehaviour
{
    OAWin win;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Time.timeScale = 0;
        win = animator.GetComponent<OAWin>();
        win.Hud.GetComponent<OAIngameMenu>().OnWin();
    }
}
