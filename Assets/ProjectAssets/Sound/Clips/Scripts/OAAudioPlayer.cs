using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OAAudioPlayer : MonoBehaviour
{
    [Tooltip("Random pitch range for audio clips")]
    [SerializeField]
    Vector2 pitchRange = new Vector2(0.8f, 1.2f);

    [Tooltip("All possible audio clips that can be played")]
    [SerializeField]
    List<AudioClip> clips;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void PlayRandomClip()
    {
        source.clip = clips[Random.Range(0, clips.Count)];
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);

        source.Play();
    }
}
