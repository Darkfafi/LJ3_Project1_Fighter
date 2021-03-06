﻿using UnityEngine;
using System.Collections;

public class SpecialBasicAttack : SpecialAttack {

	BasicStunAttack _basicAttack;


	// Use this for initialization
	void Start () {	
		_basicAttack = gameObject.GetComponent<BasicStunAttack> ();
		_maxTimesUse = 8;
		_coolDownTime = 0.5f;
		_rangeAttack = 4.2f;
	}

	protected override void OnAttack (Player player)
	{
		_basicAttack.Attack (player,_coolDownTime);
		//player.clasher.clashAble = false; // Cannot be clashed with.
		base.OnAttack (player);
	} 
}
// :D <3