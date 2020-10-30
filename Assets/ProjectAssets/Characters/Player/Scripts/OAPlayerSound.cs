using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAPlayerSound : MonoBehaviour
{

	[SerializeField]
	private OAAudioPlayer footstepPlayer;

	[SerializeField]
	private OAAudioPlayer swimmingPlayer;
 
    public void PlayFootstepSound() 
    {
		footstepPlayer.PlayRandomClip();
    }

   public void PlaySwimmingSound() 
    {
		swimmingPlayer.PlayRandomClip();
    }
}
