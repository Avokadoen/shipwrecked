using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OAComicAudio : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> comicSound;

    [SerializeField]
    List<int> ignoreOnClick = new List<int>();

    AudioSource source;
    int pos = 0;
    int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnComicIterate()
    {
        clickCount += 1;

        if (pos >= comicSound.Count)
            return;

        if (ignoreOnClick.Contains(clickCount))
            return;

        source.clip = comicSound[pos];
        source.Play();
        pos += 1;
    }
}
