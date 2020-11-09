using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAMessageBroadcastBehaviour : StateMachineBehaviour
{
    [TextAreaAttribute(minLines: 3, maxLines: 3)]
    [SerializeField]
    string message = "";

    [SerializeField]
    float playDelay = 2f;

    [SerializeField]
    bool disableNext = false;

    float playCount = 0;
    bool hasPlayed = false;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playCount += Time.deltaTime;
        if (playCount < playDelay || hasPlayed)
            return;

        animator.SetBool("next", false);

        var panel = animator.GetComponent<OAMessagePanelRef>().Panel;
        panel.SetMessage(message);

        if (!disableNext) 
            panel.OnMessageComplete.AddListener(() => animator.SetBool("next", true));

        hasPlayed = true;
    }
}
