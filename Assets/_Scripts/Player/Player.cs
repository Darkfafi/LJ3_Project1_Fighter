﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	


	//Stats
	private PlayerStats _playerStats = new PlayerStats(5f,10f,2f,5f,2f,5f); // set all base stats

	public bool busyAction = false;

	// Combat
	private BasicStunAttack _basicAttack;
	private SpecialAttack _specialAttack;
	private AttackCather _attackCatcher;

	private Timer _stunTimer;
	
	// Input
	private PlayerInput _myPlayerInput;
	private string _horizontalAxis = "HorizontalPlayer1";
	private string _verticalAxis = "VerticalPlayer1";
	private string _actionKey = "ActionKeyPlayer1";

	// Utils
	private PlatformerMovement _myPlatformerMovement;
	private TouchDetector2D _touchDetector;
	
	private PlayerAnimationHandler _playerAnimHandler;

	void Awake()
	{

		_myPlayerInput = gameObject.AddComponent<PlayerInput>();
		_myPlatformerMovement = GetComponent<PlatformerMovement>();
		_touchDetector = gameObject.AddComponent<TouchDetector2D> ();

		_attackCatcher = gameObject.AddComponent<AttackCather> ();
		_basicAttack = gameObject.AddComponent<BasicStunAttack> ();
		gameObject.AddComponent<LandOnTopKill> ();

		_playerAnimHandler = gameObject.AddComponent<PlayerAnimationHandler> ();

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
			if (transform.localScale.x < 0) { //TODO dit moet in de platformer movement component
				transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_RIGHT, _playerStats.movementSpeed);
		}
	}

	public void SetCharacter(string characterName)
	{
		Animator newAnimator = gameObject.AddComponent<Animator>();
		newAnimator = CharDB.GetCharacterAnimator(characterName);

		//TODO: add special!
	}
	public void SetKeys(string playerHorizontalAxis,string playerVerticalAxis,string playerActionKey)
	{
		_horizontalAxis = playerHorizontalAxis;
		_verticalAxis = playerVerticalAxis;
		_actionKey = playerActionKey;
	}

	void MoveLeft()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT, playerStats.movementSpeed);
		}
	}
	void Jump()
	{
		if (!busyAction) {
			_myPlatformerMovement.Jump(_playerStats.jumpForce);
		}
	}
	void FallDown()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveVertical (PlatformerMovement.DIR_DOWN, _playerStats.fallSpeed);
		}
	}

	void DoAction(){
		if (!busyAction) {
			AttackBase currentAttack = _basicAttack;
			if(!_playerStats.transformed){
				currentAttack = _basicAttack;
			}else{
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

	public PlayerStats playerStats{
		get{return _playerStats;}
	}

	public PlayerAnimationHandler playerAnimHandler{
		get{return _playerAnimHandler;}
	}

	//public variables for keyinputs
	public string horizontalAxis{
		get {
			return _horizontalAxis;
		}
		set {
			_horizontalAxis = value;
		}
	}
	public string verticalAxis{
		get {
			return _verticalAxis;
		}
		set {
			_verticalAxis = value;
		}
	}
	public string actionKey{
		get {
			return _actionKey;
		}
		set {
			_verticalAxis = value;
		}
	}
}
