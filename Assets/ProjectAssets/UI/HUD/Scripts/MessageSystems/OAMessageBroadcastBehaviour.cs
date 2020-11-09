using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAMessageBroadcastBehaviour : StateMachineBehaviour
{
    public enum ShipEmotion
    {
        Default = 0,
        Dead = 1,
        Scared = 2,
        Worried = 3
    }

    [Tooltip("Message you want the spaceship to deliver to the player. \nIf your message don't fit the text box in the editor, it won't fit the UI.")]
    [TextAreaAttribute(minLines: 5, maxLines: 5)]
    [SerializeField]
    string message = "";

    // work around for unity state machine bug with fixed time ...
    [Tooltip("How long it should wait before playing the message")]
    [SerializeField]
    float playDelay = 2f;

    [Tooltip("Will disable automatic setting of next")]
    [SerializeField]
    bool disableNext = false;

    [SerializeField]
    ShipEmotion emotion = ShipEmotion.Default;

    float playCount = 0;
    bool hasPlayed = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("next", false);

        hasPlayed = false;
        playCount = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasPlayed)
            return;

        if (playCount < playDelay)
        {
            playCount += Time.deltaTime;
            return;
        }
       
        var panel = animator.GetComponent<OAMessagePanelRef>().Panel;
        panel.SetMessage(message, emotion);

        if (!disableNext) 
            panel.OnMessageComplete.AddListener(() => animator.SetBool("next", true));

        hasPlayed = true;
    }
}
