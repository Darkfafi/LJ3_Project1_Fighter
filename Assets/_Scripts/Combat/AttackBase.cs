using UnityEngine;
using System.Collections;

public class AttackBase : MonoBehaviour {

	public const int STUN_POWER_KILL = 9000; //KILLS ENEMIES STUNNED OR NOT STUNNED.
	public const int STUN_POWER_KILL_WHILE_STUNNED = 5000; // ONLY KILLS ENEMIES THAT ARE ALREADY STUNNED

	protected void Hit(GameObject targetObject,float stunPower){
		//if stunPower is over lets say 10000 then kill.
		if (targetObject.GetComponent<AttackCather> () != null) {
			targetObject.GetComponent<AttackCather>().CatchAttack(this.gameObject,stunPower);
		}
	}
}
