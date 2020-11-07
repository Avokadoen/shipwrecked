using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OAAudioPlayer))]
public class OAMessagePanel : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI messageText;

    [SerializeField]
    float charDelay = 0.05f;

    [SerializeField]
    float spaceDelay = 0.15f;

    private OAAudioPlayer audioPlayer;
    private AudioSource source;

    void Start()
    {
        audioPlayer = GetComponent<OAAudioPlayer>();
        source = GetComponent<AudioSource>();
    }

    [ContextMenu("test")]
    void test()
    {
        SetMessage("This is a test of the set message. Does it work? I don't know. Let's find out");
    }

    public void SetMessage(string message)
    {
        StartCoroutine(WriteMessage(message));
    }

    IEnumerator WriteMessage(string message)
    {
        messageText.text = "";
        int pos = 0;
        var sb = new StringBuilder("", message.Length);

        var audioDuration = 0;
        float playDialogSound()
        {
            audioPlayer.PlayRandomClip();
            audioDuration = 0;
            return source.clip.length;
        }
        var audioLength = playDialogSound();

        while (pos != message.Length)
        {
            if (audioDuration >= audioLength)
            {
                audioLength = playDialogSound();
            }

            sb.Append(message[pos]);

            // TODO: this creates a lot of garbage (I think?)
            messageText.text = sb.ToString();
            if (message[pos] == ' ')
            {
                yield return new WaitForSeconds(spaceDelay);
            } else
            {
                yield return new WaitForSeconds(charDelay);
            }

            pos += 1;
        }

        source.Stop();
    }
}
