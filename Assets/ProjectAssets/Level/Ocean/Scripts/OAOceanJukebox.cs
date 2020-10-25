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
    public void OnEnterCave()
    {
        // TODO: implement this, also think about:
        // - We need to deal with low tide taking priority
        // - Avoid player abusing this trigger to break sound
        Debug.LogError("OnEnterCave is not implemented");
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
