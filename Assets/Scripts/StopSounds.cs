using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSounds : MonoBehaviour
{
	//Stop all sounds
	private AudioSource[] allAudioSources;

	void StopAllAudio()
	{
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource audioS in allAudioSources)
		{
			audioS.Pause();
		}
	}

	void ResumeAllAudio()
	{
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource audioS in allAudioSources)
		{
			audioS.UnPause();
		}
	}
}
