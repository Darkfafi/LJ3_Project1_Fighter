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

	public void Attack(float hitPower,float slidePower){
		//hitPower determines how long the hit person is stunned The hit person calculates with this. So maybe the orc is stunned for a shorter time then the Asian chick...
		//slidePower is how much you push yourself towards the direction.
		_hitPowerForce = hitPower;
		_attacking = true;
		if (gameObject.transform.localScale.x < 0) {
			_attackingDir = Vector2.left;
		} else {
			_attackingDir = Vector2.right;
		}
		rigidBody.velocity += slidePower * _attackingDir;
		if (AttackStarted != null) {
			AttackStarted();
		}
	}

	void Update(){
		if (Mathf.Abs (rigidBody.velocity.x) <= 0.3f && _attacking) {
			StopAttacking();
		}
	}

	void OnTouch(GameObject gObject, Vector2 vec){
		if (vec == _attackingDir && _attacking) {
			Hit(gObject,_hitPowerForce); //gObject.GetComponent<StunCatcher>().CatchStun(this.gameObject,_hitPowerForce);
		}
	}

	public void StopAttacking(){
		_attacking = false;
		_hitPowerForce = 0;
		if (AttackStopped != null) {
			AttackStopped();
		}
	}
}
