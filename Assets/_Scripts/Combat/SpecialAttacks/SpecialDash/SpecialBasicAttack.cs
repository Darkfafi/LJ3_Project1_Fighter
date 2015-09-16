using UnityEngine;
using System.Collections;

public class SpecialBasicAttack : SpecialAttack {

	BasicStunAttack _basicAttack;

	// Use this for initialization
	void Start () {
		_basicAttack = gameObject.GetComponent<BasicStunAttack> ();
		_maxTimesUse = 5; 
	}

	protected override void OnAttack (Player player)
	{
		_basicAttack.Attack (player);
		player.clasher.clashAble = false; // Cannot be clashed with.
		base.OnAttack (player);
	} 
}
