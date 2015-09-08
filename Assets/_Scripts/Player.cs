﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	private string _playerControls;
	private PlayerInput _myPlayerInput;
	private PlatformerMovement _myPlatformerMovement;


	//Stats
	private float _movementSpeed = 5f;
	private float _jumpForce = 10f;
	private float _fallSpeed = 2f;
	private float _stunPower = 5f;
	private float _dashForce = 1.5f;

	private bool _busyAction = false;


	private TouchDetector2D _touchDetector;
	private AttackCather _attackCatcher;

	void Awake()
	{
		_playerControls = Controls.PLAYER01;
		_myPlayerInput = gameObject.AddComponent<PlayerInput>();
		_myPlatformerMovement = GetComponent<PlatformerMovement>();
		_touchDetector = gameObject.AddComponent<TouchDetector2D> ();
		_attackCatcher = gameObject.AddComponent<AttackCather> ();

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
		if (!_busyAction) {
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_RIGHT, _movementSpeed);
		}
	}
	void MoveLeft()
	{
		if (!_busyAction) {
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_LEFT, _movementSpeed);
		}
	}
	void Jump()
	{
		if (!_busyAction) {
			_myPlatformerMovement.Jump (_jumpForce);
		}
	}
	void FallDown()
	{
		if (!_busyAction) {
			_myPlatformerMovement.MoveVertical (PlatformerMovement.DIR_DOWN, _fallSpeed);
		}
	}

	void DoAction(){
		if (!_busyAction) {
			// if in normal form do a basicStunAttack else do special attack.
			GetComponent<BasicStunAttack>().Attack(_stunPower,_dashForce);
		}
	}


	// Hit by attacks (MAYBE CODE IN A DIFFERENT COMPONENT)

	void OnStunHit(float stunPower, GameObject attacker){
		//TODO CALL STUN FUNCTION
	}

	void OnStunKillHit(GameObject attacker){
		//TODO IF STUNNED THEN CALL DEAD FUNCTION
	}
	void OnKillHit(GameObject attacker){
		//TODO CALL DEAD FUNCTION
	}

	public string playerControls{
		get{
			return _playerControls;
		}
	}
}
