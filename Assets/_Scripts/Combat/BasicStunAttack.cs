using UnityEngine;
using System.Collections;

public class BasicStunAttack : AttackBase {

	public delegate void VoidDelegate();
	public event VoidDelegate AttackStarted;
	public event VoidDelegate AttackStopped;

	private TouchDetector2D touchDetector;

	private bool _attacking;
	private float _hitPowerForce;
	private Vector2 _attackingDir;

	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		touchDetector = gameObject.GetComponent<TouchDetector2D> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		touchDetector.TouchStarted += OnTouch;
	}

	public override void Attack(Player player){
		//hitPower determines how long the hit person is stunned The hit person calculates with this. So maybe the orc is stunned for a shorter time then the Asian chick...
		//slidePower is how much you push yourself towards the direction.
		_hitPowerForce = player.playerStats.stunPower;
		SetAttacking(true);
		if (gameObject.transform.localScale.x < 0) {
			_attackingDir = Vector2.left;
		} else {
			_attackingDir = Vector2.right;
		}
		rigidBody.velocity += player.playerStats.dashForce * _attackingDir;
		if (AttackStarted != null) {
			AttackStarted();
		}
	}

	void Update(){
		if (Mathf.Abs (rigidBody.velocity.x) <= 0.3f && _attacking) {
			StopAttacking();
			Debug.Log("dffg");
		}
	}

	void OnTouch(GameObject gObject, Vector2 vec){
		if (vec == _attackingDir && _attacking) {
			Hit(gObject,_hitPowerForce); //gObject.GetComponent<StunCatcher>().CatchStun(this.gameObject,_hitPowerForce);
		}
	}

	public void StopAttacking(){
		SetAttacking(false);
		_hitPowerForce = 0;
		if (AttackStopped != null) {
			AttackStopped();
		}
	}

	void SetAttacking(bool value){
		_attacking = value;
		gameObject.GetComponent<Player> ().busyAction = value;
	}
}
