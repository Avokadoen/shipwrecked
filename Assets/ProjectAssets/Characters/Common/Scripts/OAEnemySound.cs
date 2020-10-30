using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAEnemySound : MonoBehaviour
{

	[SerializeField]
	private OAAudioPlayer footstepPlayer;

	[SerializeField]
	private OAAudioPlayer attackPlayer;

	public void PlayFootstepSound()
	{
		footstepPlayer.PlayRandomClip();
	}

	public void PlayAttackSound()
	{
		attackPlayer.PlayRandomClip();
	}
}
 