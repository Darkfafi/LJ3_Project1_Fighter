using UnityEngine;
using System.Collections;

public class AttackBase : MonoBehaviour {

	protected const string IN_ATTACK = "InAttack";

	public const int STUN_POWER_KILL = 9000; //KILLS ENEMIES STUNNED OR NOT STUNNED.
	public const int STUN_POWER_KILL_WHILE_STUNNED = 5000; // ONLY KILLS ENEMIES THAT ARE ALREADY STUNNED

	protected int _coolDownTime = 1;
	protected ComTimer _attackCooldownTimer;

	void Awake(){
		_attackCooldownTimer = gameObject.AddComponent<ComTimer> ();
		_attackCooldownTimer.TimerEnded += EndCooldownTimer;
	}


	public void Attack(Player player){
		if (!_attackCooldownTimer.running) {
			OnAttack(player);
			_attackCooldownTimer.StartTimer(_coolDownTime);
		}
	}
	protected virtual void OnAttack(Player player){

	}

	void EndCooldownTimer(){

	}

	protected bool Hit(GameObject targetObject,float stunPower,float pushPower = 0){
		bool result = false; 
		if (targetObject.GetComponent<AttackCather> () != null) {
			result = targetObject.GetComponent<AttackCather>().CatchAttack(this.gameObject,stunPower,pushPower);
		}
		return result;
	}

	public ComTimer cooldownTimer{
		get{return _attackCooldownTimer;}
	}
}
