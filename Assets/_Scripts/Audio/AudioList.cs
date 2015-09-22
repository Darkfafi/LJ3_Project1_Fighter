using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioList : MonoBehaviour 
{
	public AudioClip[] allAudio = new AudioClip[0];

	private List<AudioClip> audioList = new List<AudioClip> ();
	private List<AudioSource> audioSources = new List<AudioSource>();

	void Awake()
	{
		Lists ();
	}

	public void Lists()
	{
		foreach (var item in allAudio) {
			audioList.Add(item);
		}
	}

	public void PlayAudio(string audioname, bool loop = false)
	{
		AudioClip newSound = null;
		bool soundPlayed = false;
		foreach(AudioClip sound in audioList)
		{
			if(sound.name == audioname)
			{
				newSound = sound;
				break;
			}
		}
		if(newSound == null)
		{
			Debug.LogWarning("no sound has been found, add it in the list if exists.");
		} 
		else if(!loop)
		{
			foreach (var audioSource in audioSources) 
			{
				if(!audioSource.isPlaying)
				{
					audioSource.PlayOneShot(newSound);
					soundPlayed = true;
				}
			}
			if(!soundPlayed)
			{
				AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
				audioSources.Add(newAudioSource);
				newAudioSource.PlayOneShot(newSound);
			}
		} 
		else 
		{
			foreach (var audioSource in audioSources) 
			{
				if(!audioSource.isPlaying)
				{
					audioSource.clip = newSound;
					audioSource.loop = true;
					audioSource.Play();
					soundPlayed = true;
				}
			}
			if(!soundPlayed)
			{
				AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
				audioSources.Add(newAudioSource);
				newAudioSource.clip = newSound;
				newAudioSource.loop = true;
				newAudioSource.Play();
			}
		}
	}
	public void StopAudio(string sound)
	{
		foreach (var audioSource in audioSources) 
		{
			if(audioSource.isPlaying && audioSource.clip.name == sound)
			{
				audioSource.Stop();
			}
		}
	}
}
