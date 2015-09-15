using UnityEngine;
using System.Collections;

public class SpecialAttack : AttackBase {

	protected int _useCounter;
	protected int _maxTimesUse;

	protected override void OnAttack (Player player)
	{
		_useCounter ++;
		//if counter == maxUseCounter then transform back.
		if (_useCounter == _maxTimesUse) {
			EndTransform();
		}
	}

	private void EndTransform(){
		_useCounter = 0;
		gameObject.GetComponent<Player> ().TransformPlayer (PlayerTransformer.NORMAL_MOD);
	}
}
