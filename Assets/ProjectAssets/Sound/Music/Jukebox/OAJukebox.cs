using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OAJukebox : MonoBehaviour
{
    [SerializeField]
    private List<OAThemeTracks> editorThemes;

    private Dictionary<int, OAThemeTracks> themes;
    private OAThemeTracks previous;
    private OAThemeTracks selected;

    private AudioSource source;

    private float trackDuration = 0f;
    private float trackCursor = 0f;
    private int themeKey;
    // Start is called before the first frame update
    void Awake()
    {
        themes = new Dictionary<int, OAThemeTracks>();

        selected = editorThemes[0];

        // Move all tracks to a hashset
        foreach (var track in editorThemes)
        {
            themes.Add(track.GetHashCode(), track);
        }

        // Clear the list as it is not needed anymore
        editorThemes.Clear();

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        trackCursor += Time.deltaTime;
        if (trackCursor >= trackDuration)
        {
            trackDuration += 2f; // Let the coroutine run before entering here again
            StartCoroutine(FadeToNext(selected));
        }
    }

    public void SetTheme(int key)
    {
        themeKey = key;
        previous = selected; 
        if (!themes.TryGetValue(themeKey, out selected))
        {
#if UNITY_EDITOR
            Debug.LogError("Attempted to get a theme that does not exist");
            selected = previous;
#endif
        }

        // Incase we are already fading  
        StopAllCoroutines();
        StartCoroutine(FadeToNext(previous));
    }

    private IEnumerator FadeToNext(OAThemeTracks previous)
    {
        // TODO: Fade duration, overlap in fade ...
        float time = 0f;
        while (source.volume > 0 && time < 1f)
        {
            source.volume = previous.fadeOut.Evaluate(time);
            yield return new WaitForFixedUpdate(); // We use fixed and not normal update as there seems to be no normal update wait
            time += Time.fixedDeltaTime;
        }

        // TODO: avoid previous track if possible
        int trackIndex = Random.Range(0, selected.clips.Count);
        source.clip = selected.clips[trackIndex];
        trackDuration = selected.clips[trackIndex].length;
        trackCursor = 0f;
        source.Play();
        time = 0f;
        while (source.volume < 1 && time < 1f)
        {
            source.volume = selected.fadeIn.Evaluate(time);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
    }
}
