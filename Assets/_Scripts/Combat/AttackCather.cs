using UnityEngine;
using System.Collections;

public class AttackCather : MonoBehaviour {

	public delegate void FloatGoDelegate(float valueFloat, GameObject Go);
	public delegate void GoDelegate(GameObject Go);
	public event FloatGoDelegate OnStunAttackCatch;
	public event GoDelegate OnStunKillAttackCatch;
	public event GoDelegate OnKillAttackCatch;

	public void CatchAttack(GameObject objHitBy, float stunPower){
		if (stunPower < AttackBase.STUN_POWER_KILL_WHILE_STUNNED) {
			if (OnStunAttackCatch != null) {
				OnStunAttackCatch (stunPower, objHitBy);
			}
		} else if (stunPower == AttackBase.STUN_POWER_KILL_WHILE_STUNNED) {
			if (OnStunKillAttackCatch != null) {
				OnStunKillAttackCatch (objHitBy);
			}
		} else if (stunPower == AttackBase.STUN_POWER_KILL) {
			if (OnKillAttackCatch != null) {
				OnKillAttackCatch (objHitBy);
			}
		}
	}
}
