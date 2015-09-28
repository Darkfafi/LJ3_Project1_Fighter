using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameRules : MonoBehaviour {
	public Slider stockSlider;
	public Slider spawnTimeSlider;
	public Slider timeSlider;
	
	public Text stockValueText;
	public Text spawnTimeText;
	public Text timeValueText;

	void Awake()
	{
		stockSlider.value = PlayerPrefs.GetInt("StockValue");
		stockValueText.text =  PlayerPrefs.GetInt("StockValue").ToString();
		timeSlider.value = PlayerPrefs.GetInt("TimeValue");
		timeValueText.text = PlayerPrefs.GetInt("TimeValue").ToString();
		spawnTimeSlider.value = PlayerPrefs.GetInt("SpawnTime");
		spawnTimeText.text = PlayerPrefs.GetInt("SpawnTime").ToString();
	}

	public void OnStockSliderChange()
	{
		int stockValue = (int)stockSlider.value;
		stockValueText.text = stockValue.ToString();
		PlayerPrefs.SetInt("StockValue", stockValue);
		if(stockValue == 0)
		{
			timeSlider.minValue = 3;
		} else {
			timeSlider.minValue = 0;
		}
	}
	public void OnTimeSliderChange()
	{
		int timeValue = (int)timeSlider.value;
		timeValueText.text = timeValue.ToString();
		PlayerPrefs.SetInt("TimeValue", timeValue);
		if(timeValue == 0)
		{
			stockSlider.minValue = 3;
		} else {
			stockSlider.minValue = 0;
		} 
	}
	public void OnSpawnTimeSliderChange()
	{
		int spawnTime = (int)spawnTimeSlider.value;
		spawnTimeText.text = spawnTime.ToString();
		PlayerPrefs.SetInt("SpawnTime", spawnTime);
	}
}
