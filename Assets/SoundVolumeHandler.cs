using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SoundVolumeHandler : MonoBehaviour {
	public Slider mainAudioSlider;
	public Slider fxAudioSlider;
	public void OnMainAudioSliderChange()
	{
		AudioPlayer.backgroundVolume = mainAudioSlider.value;
	}
	public void OnFXAudioSliderChange()
	{
		AudioPlayer.fxVolume = fxAudioSlider.value;
	}
}
