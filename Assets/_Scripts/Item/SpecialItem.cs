using UnityEngine;
using System.Collections;

public class SpecialItem : MonoBehaviour {
	private float _hitsTillBreak;
	// Use this for initialization
	void Start () {
		GetComponent<AttackCather>().OnStunAttackCatch += GetHit;
		_hitsTillBreak = Random.Range(1,4);
	}

	void GetHit (float stunPower, GameObject objHitBy, float pushPower) {
		_hitsTillBreak--;
		if(_hitsTillBreak <= 0)
		{
			objHitBy.GetComponent<Player>().TransformPlayer(PlayerTransformer.SPECIAL_MOD);
			Destroy(this.gameObject);
		}
	}
}
