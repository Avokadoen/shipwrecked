using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OASpaceshipSounds : MonoBehaviour
{
    [SerializeField]
    OAAudioPlayer swooshPlayer;

    [SerializeField]
    OAAudioPlayer rumblePlayer;


    public void PlaySwoosh()
    {
        swooshPlayer.PlayRandomClip();
    }

    public void PlayRumble()
    {
        rumblePlayer.PlayRandomClip();
    }
}
