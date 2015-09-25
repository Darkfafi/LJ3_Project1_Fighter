using UnityEngine;
using System.Collections;

public class ComTimer : MonoBehaviour {

	public delegate void VoidDelegate ();
	public delegate void IntDelegate(int currentCount);
	
	public event VoidDelegate TimerStarted;
	public event IntDelegate TimerTik;
	public event VoidDelegate TimerEnded;

	public event VoidDelegate OnPaused;
	public event VoidDelegate OnResumed;

	private float _timerTimeGiven;
	private int _repeatAmount;
	
	private bool _paused = false;
	private bool _running = false;
	private int _repeatCounter;
	private float _timerTimeLeft;


	public void StartTimer(float timeInSeconds, int repeatCount = 0){
		_timerTimeGiven = timeInSeconds;
		_repeatAmount = repeatCount;

		_timerTimeLeft = _timerTimeGiven;
		_repeatCounter = 0;

		if (TimerStarted != null) {
			TimerStarted();
		}
		_paused = false;
		_running = true;
	}

	public void PauseTimer(){
		_paused = true;
		_running = false;
		if (OnPaused != null) {
			OnPaused();
		}
	}
	public void ResumeTimer(){
		//cant resume if not paused first
		if (_paused) {
			_paused = false;
			_running = true;
			if(OnResumed != null){
				OnResumed();
			}
		}
	}

	public void StopTimer(){
		_running = false;
	}

	public void Reset(bool startAfterReset){
		if (startAfterReset) {
			StartTimer(_timerTimeGiven,_repeatAmount);
		}else{
			_timerTimeLeft = _timerTimeGiven;
			_repeatCounter = 0;
			_running = false;
		}
	}

	private void Update(){
		if (_running) {
			_timerTimeLeft -= Time.deltaTime;
			if(_timerTimeLeft <= 0){
				_timerTimeLeft = 0;
				_repeatCounter ++;
				if(TimerTik != null){
					TimerTik(_repeatCounter);
				}
				if(_repeatCounter > _repeatAmount){
					if(TimerEnded != null){
						TimerEnded();
						StopTimer();
					}
				}
			}
		}
	}

	public bool running{
		get{return _running;}
	}
	public bool paused{
		get{return _paused;}
	}
	public float secondsLeft{
		get{return _timerTimeLeft;}
	}
	public int timesRepeated{
		get{return _repeatCounter;}
	}
	public float secondsGivenToCount{
		get{return _timerTimeGiven;}
	}
	public int timesGivenToRepeat{
		get{return _repeatAmount;}
	}

	public string GetHumanTimeString(){
		string timeString = "0";
		float tempTimeLeft = _timerTimeLeft;

		int minCounter = 0;

		while (tempTimeLeft >= 60) {
			minCounter++;
			tempTimeLeft -= 60;
		}

		if (minCounter >= 10) {
			timeString = minCounter + ":" + Mathf.FloorToInt (tempTimeLeft);
		} else {
			timeString += minCounter + ":" + Mathf.FloorToInt(tempTimeLeft);
		}

		return timeString;
	}

}
