using UnityEngine;
using System.Collections;
using System.Timers;

public class BasicStunAttack : AttackBase {

	public delegate void VoidDelegate();
	public event VoidDelegate AttackStarted;
	public event VoidDelegate AttackStopped;

	private TouchDetector2D touchDetector;
	
	private float _hitPowerForce;
	private Vector2 _attackingDir;

	private float _oldGravityScale;

	private ComTimer _inAttackTimer;
	private float _slideDuritation = 0.25f;

	private Player _player;

	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		touchDetector = gameObject.GetComponent<TouchDetector2D> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		touchDetector.OnTouch += OnTouch;
		_player = gameObject.GetComponent<Player> ();

		_inAttackTimer = gameObject.AddComponent<ComTimer> ();
		_inAttackTimer.TimerEnded += EndTimer;


	}

	protected override void OnAttack(Player player){
		//hitPower determines how long the hit person is stunned The hit person calculates with this. So maybe the orc is stunned for a shorter time then the Asian chick...
		//slidePower is how much you push yourself towards the direction.
		_hitPowerForce = player.playerStats.stunPower;

		SetAttacking(true);

		_oldGravityScale = rigidBody.gravityScale;
		rigidBody.gravityScale = 0;

		if (gameObject.transform.localScale.x < 0) {
			_attackingDir = Vector2.left;
		} else {
			_attackingDir = Vector2.right;
		}
		rigidBody.velocity = player.playerStats.dashForce * _attackingDir;
		if (AttackStarted != null) {
			AttackStarted();
		}
	}

	void OnTouch(GameObject gObject, Vector2 vec){
		if (vec == _attackingDir) { //TODO: if isDashing then clash
			if(_inAttackTimer.running){
				if(gObject.GetComponent<ClashAble>() != null && gObject.GetComponent<ClashAble>().clashAble){
					_player.clasher.Clash(gObject,_player.playerStats.pushPower);
				}else{
					if(Hit(gObject,_hitPowerForce)){
						if(gObject.GetComponent<Rigidbody2D>()){
							gObject.GetComponent<Rigidbody2D>().velocity += gameObject.GetComponent<Rigidbody2D>().velocity.normalized * (_player.playerStats.pushPower * 0.8f);
						}	
						StopAttacking();
					}

				}
			}
		} // clash = opposite direction velocity + particle system emitter added with timer.
	}

	private void EndTimer(){
		StopAttacking ();
	}

	public void StopAttacking(){
		//rigidBody.velocity = new Vector2(0f,0f);
		rigidBody.velocity = rigidBody.velocity / 2;
		rigidBody.gravityScale = _oldGravityScale;
		SetAttacking(false);
		_hitPowerForce = 0;
		if (AttackStopped != null) {
			AttackStopped();
		}

	}

	void SetAttacking(bool value){
		_player.busyAction = value;
		_player.clasher.clashAble = value;
		if (value == true) {
			_inAttackTimer.StartTimer(_slideDuritation);
		} else if(_inAttackTimer.running) {
			_inAttackTimer.StopTimer();
		}
	}
}
