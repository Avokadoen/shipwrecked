using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: OnEnemyWaveCleared -> HighTide music

[RequireComponent(typeof(OAJukebox))]
public class OAOceanJukebox : MonoBehaviour
{
    [Tooltip("Ocean tide animator")]
    [SerializeField]
    OATideAnimator tideAnimator;

    [Tooltip("All caves in the level")]
    [SerializeField]
    List<OACaveTrigger> caveTriggers;

    [Tooltip("Theme for high tide")]
    [SerializeField]
    OAThemeTracks highTideTheme;
    int highTideThemeId;

    [Tooltip("Theme for low tide")]
    [SerializeField]
    OAThemeTracks lowTideTheme;
    int lowTideThemeId;

    [Tooltip("Theme for cave tide")]
    [SerializeField]
    OAThemeTracks caveTheme;
    int caveThemeId;

    OAJukebox jukebox;
    int currentId;

    /// <summary>
    /// Used by cave triggers to play cave soundtrack
    /// </summary>
    public void OnEnterLeaveCave(OACaveTrigger.TriggerType triggerType)
    {
        // TODO: this will break when you are in a cave and then the 
        //       tide changes to high-tide as it should set the track to cave track, but will change it to high tide track
        if (currentId == lowTideThemeId)
            return;

        // TODO: Avoid player abusing this trigger to break sound
        var trackId = (triggerType == OACaveTrigger.TriggerType.Enter) ? caveThemeId : highTideThemeId;
        SetTheme(trackId);
    }

    // Start is called before the first frame update
    void Start()
    {
        jukebox = GetComponent<OAJukebox>();

        highTideThemeId = highTideTheme.GetHashCode();
        lowTideThemeId = lowTideTheme.GetHashCode();
        caveThemeId = caveTheme.GetHashCode();

        jukebox.SetTheme(highTideThemeId);

        tideAnimator.AddHighTideListener(OnHighTide);
        tideAnimator.AddLowTideListener(OnLowTide);

        foreach (var cave in caveTriggers)
        {
            cave.AddListener(OnEnterLeaveCave);
        }
    }

    void OnHighTide()
    {
        SetTheme(highTideThemeId);
    }

    void OnLowTide()
    {
        SetTheme(lowTideThemeId);
    }

    void SetTheme(int id)
    {
        if (currentId == id)
            return;

        currentId = id;
        jukebox.SetTheme(id);
    }
}
