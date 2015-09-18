using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameRules : MonoBehaviour {
	private string _gameMode = "Stock";

	public Slider modeSlider;
	public Slider spawnTimeSlider;

	public Text buttonText;
	public Text modeValueText;
	public Text spawnTimeText;

	public void SwitchGameMode() //for when the time game mode is implemented
	{
		if(_gameMode == "Stock")
		{
			_gameMode = "Time";
			modeSlider.minValue = 3;
			modeSlider.maxValue = 10;
		} else {
			_gameMode = "Stock";
			modeSlider.minValue = 3;
			modeSlider.maxValue = 7;
		}
		buttonText.text = _gameMode;
		PlayerPrefs.SetString("GameMode", _gameMode);
	}

	public void OnModeSliderChange()
	{
		int modeValue = (int)modeSlider.value;
		modeValueText.text = modeValue.ToString();
		PlayerPrefs.SetInt("ModeValue", modeValue);
	}
	public void OnSpawnTimeSliderChange()
	{
		int spawnTime = (int)spawnTimeSlider.value;
		spawnTimeText.text = spawnTime.ToString();
		PlayerPrefs.SetInt("SpawnTime", spawnTime);
	}
}
