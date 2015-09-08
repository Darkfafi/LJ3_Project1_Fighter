using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	private string _playerControls;
	private PlayerInput _myPlayerInput;
	private PlatformerMovement _myPlatformerMovement;


	//Stats
	private bool _transformed = false;
	private float _movementSpeed = 5f;
	private float _jumpForce = 10f;
	private float _fallSpeed = 2f;
	private float _stunPower = 5f;
	private float _pushPower = 2f;
	private float _dashForce = 5f;

	public bool busyAction = false;

	private BasicStunAttack _basicAttack;
	private SpecialAttack _specialAttack;

	private TouchDetector2D _touchDetector;
	private AttackCather _attackCatcher;
	
	private Timer _stunTimer;

	void Awake()
	{
		_playerControls = Controls.PLAYER01;

		_myPlayerInput = gameObject.AddComponent<PlayerInput>();
		_myPlatformerMovement = GetComponent<PlatformerMovement>();
		_touchDetector = gameObject.AddComponent<TouchDetector2D> ();
		_attackCatcher = gameObject.AddComponent<AttackCather> ();

		_basicAttack = gameObject.AddComponent<BasicStunAttack> ();
		gameObject.AddComponent<LandOnTopKill> ();

		_attackCatcher.OnStunAttackCatch += OnStunHit;
		_attackCatcher.OnStunKillAttackCatch += OnStunKillHit; // if Jump on my head hit while in stun
		_attackCatcher.OnKillAttackCatch += OnKillHit;
	}
	void Start()
	{
		_myPlayerInput.RightKeyPressed += MoveRight;
		_myPlayerInput.LeftKeyPressed += MoveLeft;
		_myPlayerInput.JumpKeyPressed += Jump;
		_myPlayerInput.DownKeyPressed += FallDown;
		_myPlayerInput.ActionKeyPressed += DoAction;
	}

	// Movement

	void MoveRight()
	{
		if (!busyAction) {
			if(transform.localScale.x < 0){
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
			}
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_RIGHT, _movementSpeed);
		}
	}
	void MoveLeft()
	{
		if (!busyAction) {
			if(transform.localScale.x > 0){
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1,transform.localScale.y,transform.localScale.z);;
			}
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_LEFT, _movementSpeed);
		}
	}
	void Jump()
	{
		if (!busyAction) {
			_myPlatformerMovement.Jump (_jumpForce);
		}
	}
	void FallDown()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveVertical (PlatformerMovement.DIR_DOWN, _fallSpeed);
		}
	}

	void DoAction(){
		if (!busyAction) {
			// if in normal form do a basicStunAttack else do special attack.
			AttackBase currentAttack = _basicAttack;
			if(!_transformed){
				currentAttack = _basicAttack;
			}else{
				//TODO CurrentAttack = Special Attack
				currentAttack = _specialAttack;
			}

			currentAttack.Attack(this); //geef stats class, player class of gameobject mee zodat de infor gegeven globaal kan blijven.
		}
	}

	// Hit by attacks (MAYBE CODE IN A DIFFERENT COMPONENT)
	void OnStunHit(float stunPower, GameObject attacker, float pushPower){
		//TODO CALL STUN FUNCTION
		if (!_stunTimer.IsRunning ()) {
			GetStunned(stunPower);
		}
	}

	void OnStunKillHit(GameObject attacker, float pushPower){
		//TODO IF STUNNED THEN CALL DEAD FUNCTION
		if (_stunTimer.IsRunning ()) {
			GetKilled();
		}
	}
	void OnKillHit(GameObject attacker, float pushPower){
		//TODO CALL DEAD FUNCTION
		GetKilled();
	}

	void GetStunned(float stunPower){
		_stunTimer = new Timer ((int)(500 * stunPower));
		_stunTimer.TimerEnded += StunTimerEnded;
		_stunTimer.Start ();
		busyAction = true;
	}
	void HealStun(){
		_stunTimer.Stop ();
		busyAction = false;
	}
	void GetKilled(){
		// Die
	}

	void StunTimerEnded(){
		HealStun ();
	}

	// GETTERS

	public string playerControls{
		get{
			return _playerControls;
		}
	}

	public float movementSpeed{
		get{
			return _movementSpeed;
		}
	}
	public float jumpForce{
		get{
			return _jumpForce;
		}
	}
	public float stunPower{
		get{
			return _stunPower;
		}
	}
	public float dashForce{
		get{
			return _dashForce;
		}
	}
	public float pushPower{
		get{
			return _pushPower;
		}
	}
}
