using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAPlayerSound : MonoBehaviour
{

	[SerializeField]
	private OAAudioPlayer ground;

	[SerializeField]
	private OAAudioPlayer water;
 
    public void PlayFootstepSound() 
    {
		ground.PlayRandomClip();
    }

   public void PlaySwimmingSound() 
    {
		water.PlayRandomClip();
    }
}
