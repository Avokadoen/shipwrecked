using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeTracks", menuName = "OceanAlien/ThemeTracks", order = 2)]
public class OAThemeTracks : ScriptableObject
{
    [Tooltip("Random pause between each track, x is lowest, y is highes possible pause")]
    public Vector2 pauseRndRange;

    [Tooltip("All songs that should be grouped together i,e 'all main menu music'")]
    public List<AudioClip> clips;

    [Tooltip("How long fade will take")]
    [Min(0.1f)]
    public float fadeDuration = 1f;

    [Tooltip("How each song should fade in")]
    public AnimationCurve fadeIn;

    [Tooltip("How each song should fade out")]
    public AnimationCurve fadeOut;
}
