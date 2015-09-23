using UnityEngine;
using System.Collections;

public class AttackCather : MonoBehaviour {

	public delegate void FloatGoFloatDelegate(float valueFloat, GameObject Go,float valueFloatSec);
	public delegate void GoFloatDelegate(GameObject Go,float valueFloat);
	public event FloatGoFloatDelegate OnStunAttackCatch;
	public event GoFloatDelegate OnStunKillAttackCatch;
	public event GoFloatDelegate OnKillAttackCatch;

	public bool catcherOn = true;

	public bool CatchAttack(GameObject objHitBy, float stunPower,float pushPower){
		bool result = false;
		if (catcherOn) {
			if (stunPower < AttackBase.STUN_POWER_KILL_WHILE_STUNNED) {
				if (OnStunAttackCatch != null) {
					OnStunAttackCatch (stunPower, objHitBy, pushPower);
				}
			} else if (stunPower == AttackBase.STUN_POWER_KILL_WHILE_STUNNED) {
				if (OnStunKillAttackCatch != null) {
					OnStunKillAttackCatch (objHitBy, pushPower);
				}
			} else if (stunPower == AttackBase.STUN_POWER_KILL) {
				if (OnKillAttackCatch != null) {
					OnKillAttackCatch (objHitBy, pushPower);
				}
			}
			result = true;
		}

		return result;
	}
}
