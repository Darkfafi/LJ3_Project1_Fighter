using UnityEngine;
using System.Collections;

public class ComTimer : MonoBehaviour {

	public delegate void VoidDelegate ();
	public delegate void IntDelegate(int currentCount);
	
	public event VoidDelegate TimerStarted;
	public event IntDelegate TimerTik;
	public event VoidDelegate TimerEnded;

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
	}
	public void ResumeTimer(){
		//cant resume if not paused first
		if (_paused) {
			_paused = false;
			_running = true;
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
}
