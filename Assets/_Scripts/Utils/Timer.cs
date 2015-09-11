using UnityEngine;
using System.Collections;
using System.Threading;

public class Timer{
	
	public delegate void VoidDelegate ();
	public delegate void IntDelegate(int currentCount);

	public event VoidDelegate TimerStarted;
	public event IntDelegate TimerTik;
	public event VoidDelegate TimerEnded;

	private int _milliSecondsToWait;
	private int _timesToRepeat;

	private int _currentCount;

	private Thread _thread;

	public Timer(int millisecToWait,int timesToRepeat = 0){
		_milliSecondsToWait = millisecToWait;
		_timesToRepeat = timesToRepeat;
	}

	public void Start(){
		if (IsRunning()) {
			Stop ();
		}
		_thread = new Thread(new ThreadStart(Tik));
		_thread.Start ();
		if (TimerStarted != null) {
			TimerStarted();
		}
	}

	void Tik(){
		Thread.Sleep (_milliSecondsToWait);
		_currentCount ++;
		if (TimerTik != null) {
			TimerTik(_currentCount);
		}

		if (_currentCount < _timesToRepeat) {
			Tik ();
		} else {
			if(TimerEnded != null){
				TimerEnded();
			}
			Stop();
		}
	}
	public void Stop(){
		_thread.Abort ();
	}

	public bool IsRunning(){
		return _thread != null && _thread.IsAlive;
	}
}
