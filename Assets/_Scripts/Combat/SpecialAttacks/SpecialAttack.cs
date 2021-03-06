﻿using UnityEngine;
using System.Collections;

public class SpecialAttack : AttackBase {
	
	protected int _useCounter;
	protected int _maxTimesUse = 0;
	protected float _rangeAttack = 3;

	protected override void OnAttack (Player player)
	{
		_useCounter ++;
		//if counter == maxUseCounter then transform back. dus als max 0 is dan is het oneindig.
		if (_useCounter == _maxTimesUse) {
			EndTransform();
		}
	}

	protected void EndTransform(){
		gameObject.GetComponent<Player> ().TransformPlayer (PlayerTransformer.NORMAL_MOD);
	}
	public void UseCounterReset(){
		_useCounter = 0;
	}
	public float rangeAttack{
		get{return _rangeAttack;}
	}
}
