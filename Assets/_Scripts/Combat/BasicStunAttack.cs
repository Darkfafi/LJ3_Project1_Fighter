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
	private float _slideDuritation = 0.3f;

	private Player _player;

	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		touchDetector = gameObject.GetComponent<TouchDetector2D> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		touchDetector.TouchStarted += OnTouch;
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
		if (vec == _attackingDir) {
			if(_inAttackTimer.running){
				Hit(gObject,_hitPowerForce); //gObject.GetComponent<StunCatcher>().CatchStun(this.gameObject,_hitPowerForce);
			}
		}
	}

	private void EndTimer(){
		StopAttacking ();
	}

	public void StopAttacking(){
		rigidBody.velocity = new Vector2(0f,0f);
		rigidBody.gravityScale = _oldGravityScale;
		SetAttacking(false);
		_hitPowerForce = 0;
		if (AttackStopped != null) {
			AttackStopped();
		}

	}

	void SetAttacking(bool value){
		_player.busyAction = value;
		if (value == true) {
			_inAttackTimer.StartTimer(_slideDuritation);
		} else if(_inAttackTimer.running) {
			_inAttackTimer.StopTimer();
		}
	}
}
