using UnityEngine;
using System.Collections;

public class ThrowRangedSpecialAttack : SpecialAttack {


	private GameObject _throwAbleObject;
	private float _throwStrength;
	private Animator _animator;
	private Player _player;

	void Start(){
		_throwStrength = 15;
		_maxTimesUse = 3;
		_animator = GetComponent<Animator> ();
	}

	protected override void OnAttack (Player player)
	{
		_player = player;
		//_player.busyAction = true;
		_player.AddBusyAction (IN_ATTACK);
		Vector3 spawnPos = new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z);
		_throwAbleObject = Instantiate ((GameObject)Resources.Load ("Prefabs/SpecialAttacks/ThrowAbleObject"), spawnPos, Quaternion.identity) as GameObject;
		int dir = (int)(Mathf.Abs (gameObject.transform.localScale.x) / gameObject.transform.localScale.x);
		_throwAbleObject.GetComponent<ThrowObject> ().SetStats (gameObject, dir, _throwStrength, player.playerStats.stunPower, player.playerStats.pushPower);	 
		base.OnAttack (player);
	}


	void Update(){
		if (_animator.GetCurrentAnimatorStateInfo (0).IsName ("TransformAttack")) {
			if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _animator.GetCurrentAnimatorStateInfo(0).length){
				//_player.busyAction = false;
				_player.RemoveBusyAction(IN_ATTACK);
			}
		}
	}
}
