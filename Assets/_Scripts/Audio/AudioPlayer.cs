using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioPlayer : MonoBehaviour 
{
	public AudioClip[] allAudio = new AudioClip[0];

	public static float fxVolume = 1;
	public static float backgroundVolume = 1;

	private List<AudioClip> _audioList = new List<AudioClip> ();
	private List<AudioSource> _audioSources = new List<AudioSource>();

	private Dictionary<AudioSource, MonoBehaviour> _audioPlayedByMono = new Dictionary<AudioSource, MonoBehaviour>();

	void Awake()
	{
		Lists ();
	}

	public void Lists()
	{
		foreach (var item in allAudio) {
			_audioList.Add(item);
		}
	}

	public void PlayBackgroundMusic(string backgroundMusicName)
	{
		AudioClip newMusic = null;
		foreach(AudioClip sound in _audioList)
		{
			if(sound.name == backgroundMusicName)
			{
				newMusic = sound;
				break;
			}
		}
		if(newMusic == null)
		{
			Debug.LogWarning("no sound has been found, add it in the list if exists.");
		} 
		AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
		newAudioSource.clip = newMusic;
		newAudioSource.volume = backgroundVolume;
		newAudioSource.loop = true;
		newAudioSource.Play();
	}

	public void PlayAudio(string audioname, bool loop = false, MonoBehaviour playedBy = null)
	{
		AudioClip newSound = null;
		bool soundPlayed = false;
		foreach(AudioClip sound in _audioList)
		{
			if(sound.name == audioname)
			{
				newSound = sound;
				break;
			}
		}
		if(newSound == null)
		{
			Debug.LogWarning("No sound has been found, check soundname or add it in the list if exists.");
		} 
		foreach (var audioSource in _audioSources) 
		{
			if(!audioSource.isPlaying)
			{
				audioSource.volume = fxVolume;
				audioSource.loop = loop;
				if(!loop)
				{
					audioSource.PlayOneShot(newSound);
				} else if(loop && playedBy != null  && !_audioPlayedByMono.ContainsValue(playedBy)){
					audioSource.clip = newSound;
					audioSource.Play();
					_audioPlayedByMono.Add(audioSource,playedBy);
				} else {
					Debug.LogError("Can't play sound: Need to know who it is played by or already has a looped sound!");
				}
				soundPlayed = true;
				break;
			}
		}
		if(!soundPlayed)
		{
			CreateNewAudioSource(newSound, loop, playedBy);
		}
	}

	private void CreateNewAudioSource(AudioClip newSound,bool loop, MonoBehaviour playedBy)
	{
		AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
		_audioSources.Add(newAudioSource);
		newAudioSource.volume = fxVolume;
		newAudioSource.loop = loop;
		if(!loop)
		{
			newAudioSource.PlayOneShot(newSound);
		} else if(loop && playedBy != null && !_audioPlayedByMono.ContainsValue(playedBy)){
			newAudioSource.clip = newSound;
			newAudioSource.Play();
			_audioPlayedByMono.Add(newAudioSource,playedBy);
		} else {
			Debug.LogError("Can't create new sound: Need to know who it is played by or already has a looped sound!");
		}
	}

	public void StopAudio(string sound, MonoBehaviour playedBy = null)
	{
		if(playedBy != null)
		{
			if(_audioPlayedByMono.ContainsValue(playedBy))
			{
				foreach (var audioSource in _audioSources) {
					if((_audioPlayedByMono.ContainsKey(audioSource) && audioSource.clip.name == sound) && _audioPlayedByMono[audioSource] == playedBy)
					{
						audioSource.Stop();
						_audioPlayedByMono.Remove(audioSource);
					}
				}
			}
		} 
		else 
		{
			Debug.LogWarning("Did not get playedby value: dupplicate sounds will be stopped!");
			foreach (var audioSource in _audioSources) {
				if(audioSource.clip.name == sound)
				{
					audioSource.Stop();
				}
			}
		}
	}
}
