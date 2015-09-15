using UnityEngine;
using System.Collections;

public class ThrowRangedSpecialAttack : SpecialAttack {


	private GameObject _throwAbleObject;
	private float _throwStrength;

	void Start(){
		_throwStrength = 15;
		_maxTimesUse = 3;
	}

	protected override void OnAttack (Player player)
	{
		Vector3 spawnPos = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);

		_throwAbleObject = Instantiate ((GameObject)Resources.Load ("Prefabs/SpecialAttacks/ThrowAbleObject"), spawnPos, Quaternion.identity) as GameObject;
		int dir = (int)(Mathf.Abs (gameObject.transform.localScale.x) / gameObject.transform.localScale.x);
		_throwAbleObject.GetComponent<ThrowObject> ().SetStats (gameObject, dir, _throwStrength, player.playerStats.stunPower, player.playerStats.pushPower);

		base.OnAttack (player);
	}
}
