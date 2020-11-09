using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using ShipEmotion = OAMessageBroadcastBehaviour.ShipEmotion;

// TODO: the dialog should be a state machine where we always go through tutorial state (maybe add options in setting to skip)
[RequireComponent(typeof(OAAudioPlayer))]
public class OAMessagePanel : MonoBehaviour
{
    public enum Status
    {
        Close = 0,
        Open = 1,
        Idle = 2
    }

    [SerializeField]
    TMPro.TextMeshProUGUI messageText;

    [Tooltip("Delay between each character in a word")]
    [SerializeField]
    float charDelay = 0.05f;

    [Tooltip("Delay between each space")]
    [SerializeField]
    float spaceDelay = 0.15f;

    [Tooltip("Delay between each punctation ('.', ',', '?')")]
    [SerializeField]
    float punctationDelay = 0.5f;

    [Tooltip("How long the panel stay up after all text is displayed")]
    [SerializeField]
    float closePanelDelay = 4f;

    [SerializeField]
    GameObject profilePanel;
    public GameObject ProfilePanel { get => profilePanel; }

    [SerializeField]
    GameObject textPanel;
    public GameObject TextPanel { get => textPanel; }

    [SerializeField]
    Animator anim;

    [SerializeField]
    Animator spaceshipPortrait;

    [SerializeField]
    OABuildingArea shipArea;

    private UnityEvent onMessageComplete;
    public UnityEvent OnMessageComplete { get => onMessageComplete; }

    private OAAudioPlayer audioPlayer;
    private AudioSource source;


    void Start()
    {
        audioPlayer = GetComponent<OAAudioPlayer>();
        source = GetComponent<AudioSource>();

        anim.SetInteger("status", (int)Status.Idle);

        if (onMessageComplete == null)
            onMessageComplete = new UnityEvent();

    }

    public void SetMessage(string message, ShipEmotion emotion)
    {
        profilePanel.SetActive(true);
        textPanel.gameObject.SetActive(true);

        spaceshipPortrait.SetInteger("shipEmotion", (int) emotion);

        StopAllCoroutines();
        StartCoroutine(WriteMessage(message));
    }

    // SPAGHETTI WARNING!!! ! :'( 
    // TODO: rewrite all of this
    IEnumerator WriteMessage(string message)
    {
        anim.SetInteger("status", (int)Status.Open);

        messageText.text = "";
        int pos = 0;
        var sb = new StringBuilder("", message.Length);

        var audioDuration = 0f;
        var prev_iter_time = Time.time;
        float playDialogSound()
        {
            audioPlayer.PlayRandomClip();
            audioDuration = 0;
            return source.clip.length * source.pitch;
        }
        var audioLength = playDialogSound();

        while (pos != message.Length)
        {
            if (audioDuration >= audioLength)
            {
                audioLength = playDialogSound();
            }
            audioDuration += Time.time - prev_iter_time;
            prev_iter_time = Time.time;

            sb.Append(message[pos]);

            // TODO: this creates a lot of garbage (I think?)
            messageText.text = sb.ToString();

            switch (message[pos])
            {
                case ' ':
                    yield return new WaitForSeconds(spaceDelay);
                    break;
                case '.':
                case '?':
                case ',':
                    source.Pause();
                    var delay = punctationDelay * .5f;
                    yield return new WaitForSeconds(delay);
                    source.Play();
                    yield return new WaitForSeconds(delay);
                    break;
                default:
                    yield return new WaitForSeconds(charDelay);
                    break;
            }

            pos += 1;
        }

        source.Stop();

        onMessageComplete.Invoke();

        yield return new WaitForSeconds(closePanelDelay);

        anim.SetInteger("status", (int)Status.Close);
    }
}
