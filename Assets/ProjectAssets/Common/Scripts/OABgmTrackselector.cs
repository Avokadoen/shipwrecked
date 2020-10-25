using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OABgmTrackselector : MonoBehaviour
{

  

    [SerializeField]
    private List<AudioClip> tracks;

    [SerializeField]
    private Vector2 breakRange;

    private AudioSource audioSource;
    private float trackPlayedDuration;
    private float trackLength;
    private float trackBreak;
    private int previousTrack = -1;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // adds audioSource if it's missing

        if (tracks.Count < 2)
        {
#if UNITY_EDITOR
            Debug.LogError("OABgmTrackselector need atleast 2 tracks!");
            EditorApplication.isPlaying = false;
#endif
        }

    }

    // Update is called once per frame
    void Update()
    {
        trackPlayedDuration += Time.deltaTime;

        if (trackPlayedDuration < trackLength + trackBreak)
            return;

        var trackSelector = previousTrack;
        while (trackSelector == previousTrack)
        {
            trackSelector = Random.Range(0, tracks.Count);
        }
        previousTrack = trackSelector;
        audioSource.clip = tracks[trackSelector];
        trackPlayedDuration = 0;
        trackLength = tracks[trackSelector].length;
        trackBreak = Random.Range(breakRange.x, breakRange.y);

        audioSource.Play();
        
    }
}
