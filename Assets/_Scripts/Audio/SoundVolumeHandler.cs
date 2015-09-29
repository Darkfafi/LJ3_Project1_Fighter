using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SoundVolumeHandler : MonoBehaviour {
	public Slider mainAudioSlider;
	public Slider fxAudioSlider;
	public bool inGame;

	private AudioPlayer _audioPlayer;

	void Awake()
	{
		if(inGame)
			_audioPlayer = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<AudioPlayer>();
	}

	public void OnMainAudioSliderChange()
	{
		if(inGame)
		{
			_audioPlayer.backgroundVolume = mainAudioSlider.value;
		}
		PlayerPrefs.SetFloat("backgroundVolume", mainAudioSlider.value);
	}
	public void OnFXAudioSliderChange()
	{
		if(inGame)
		{
			_audioPlayer.fxVolume = fxAudioSlider.value;
		}
		PlayerPrefs.SetFloat("fxVolume", fxAudioSlider.value);
	}
}
