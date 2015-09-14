using UnityEngine;
using System.Collections;

public class SpecialAttack : AttackBase {

	protected int _useCounter;
	protected int _maxTimesUse;


	// de transform time logica staat in de attack voor het geval je verschillende specials  had dan zou de ene korter duren dan de ander.
	protected float _timeInTransformation;
	protected ComTimer _transformBackTimer;

	private Player _player;

	void Start(){
		_player = gameObject.GetComponent<Player> ();
		_transformBackTimer.StartTimer (_timeInTransformation);
		_transformBackTimer.TimerEnded += EndTransform;
	}

	protected override void OnAttack (Player player)
	{
		_useCounter ++;
		//if counter == maxUseCounter then transform back.
		if (_useCounter == _maxTimesUse) {
			EndTransform();
		}
	}

	private void EndTransform(){
		_transformBackTimer.StopTimer ();
		_transformBackTimer.TimerEnded -= EndTransform;
		_player.TransformPlayer (PlayerTransformer.NORMAL_MOD);
	}
}
