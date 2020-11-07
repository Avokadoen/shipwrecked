using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAClosePanelBehaviour : StateMachineBehaviour
{
    GameObject profilePanel;
    GameObject textPanel;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!profilePanel || !textPanel)
        {
            var panelScript = animator.GetComponent<OAMessagePanel>();
            profilePanel = panelScript.ProfilePanel;
            textPanel = panelScript.TextPanel;
        }
        profilePanel.SetActive(false);
        textPanel.SetActive(false);
    }

}
