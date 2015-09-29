using UnityEngine;
using System.Collections;

public class LevelMusicHandler : MonoBehaviour {
	public string musicName;
	void Start () 
	{
		AudioPlayer audioPlayer = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<AudioPlayer>();
		audioPlayer.PlayBackgroundMusic(musicName);
	}
}
