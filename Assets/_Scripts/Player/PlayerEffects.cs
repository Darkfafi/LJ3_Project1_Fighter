
using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour {
	private GameObject _stunEffect;
	private GameObject _jumpEffect;
	private GameObject _doubleJumpEffect;
	private GameObject _clashEffect;
	private GameObject _dashEffect;
	private GameObject _startRunEffect;
	private GameObject _wallSlideEffect;
	private GameObject _deathEffect;
	private GameObject _hitEffect;

	private GameObject _currentStunEffect;
	private GameObject _currentDashEffect;
	private GameObject _currentWallSlideEffectFeet;
	private GameObject _currentWallSlideEffectHands;

	private Vector3 _heightPositionMod = new Vector3 (0, 1.4f, 0);

	private string _effectsPath = "Prefabs/Effects/";
	// Use this for initialization
	void Start () {
		_stunEffect = Resources.Load(_effectsPath + "FXStun", typeof(GameObject)) as GameObject;
		_jumpEffect = Resources.Load(_effectsPath + "FXJumpGround", typeof(GameObject)) as GameObject;
		_doubleJumpEffect = Resources.Load(_effectsPath + "FXDoubleJump", typeof(GameObject)) as GameObject;
		_clashEffect = Resources.Load(_effectsPath + "FXImpact", typeof(GameObject)) as GameObject;
		_dashEffect = Resources.Load(_effectsPath + "FXDash", typeof(GameObject)) as GameObject;
		_startRunEffect = Resources.Load(_effectsPath + "FXBeginRun", typeof(GameObject)) as GameObject;
		_wallSlideEffect = Resources.Load(_effectsPath + "FXWallGlide", typeof(GameObject)) as GameObject;
		_deathEffect = Resources.Load(_effectsPath + "FXDeath", typeof(GameObject)) as GameObject;
		_hitEffect = Resources.Load(_effectsPath + "FXHit", typeof(GameObject)) as GameObject;

		PlatformerMovement myPlatformerMovement = GetComponent<PlatformerMovement>();
		myPlatformerMovement.Jumped += CreateJumpEffect;
		myPlatformerMovement.DoubleJumped += CreateDoubleJumpEffect;
		myPlatformerMovement.StartedRunning += CreateStartRunEffect;
		myPlatformerMovement.StartedWallSlide += CreateWallSlideEffect;
		myPlatformerMovement.EndedWallSlide += RemoveWallSlideEffect;

		GetComponent<BasicStunAttack>().AttackStarted += CreateDashEffect;
		GetComponent<ClashAble>().Clashed += CreateClashEffect;
		GetComponent<AttackCather>().OnStunAttackCatch += CreateHitEffect;

		Player myPlayerScript = GetComponent<Player>();
		myPlayerScript.StartStunned += CreateStunEffect;
		myPlayerScript.StopStunned += RemoveStunEffect;
		myPlayerScript.StartedDying += CreateDeathEffect;
	}

	void CreateDeathEffect()
	{
		Instantiate(_deathEffect,this.transform.position+ _heightPositionMod,Quaternion.identity);
	}

	void Update()
	{
		if(_currentStunEffect != null)
		{
			_currentStunEffect.transform.position = this.transform.position + _heightPositionMod;
		}
		if(_currentWallSlideEffectFeet != null && _currentWallSlideEffectHands != null)
		{
			Vector3 feetPosition = this.transform.position + new Vector3(0.75f,-0.75f,0);
			Vector3 handPosition = this.transform.position + new Vector3(0.75f,1f,0);
			if(this.transform.localScale.x < 0)
			{
				handPosition = this.transform.position + new Vector3(-0.75f,1f,0);
				feetPosition = this.transform.position + new Vector3(-0.75f,-0.75f,0);
			}
			_currentWallSlideEffectFeet.transform.position = feetPosition + _heightPositionMod;
			_currentWallSlideEffectHands.transform.position = handPosition + _heightPositionMod;
		}
		if(_currentDashEffect != null)
		{
			_currentDashEffect.transform.position = this.transform.position + _heightPositionMod;
		}
	}
	void CreateHitEffect(float valueFloat, GameObject Go, float valueFloatSec)
	{
		Instantiate(_hitEffect, this.transform.position + _heightPositionMod, Quaternion.identity);
	}

	void CreateDoubleJumpEffect()
	{
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		Instantiate(_doubleJumpEffect, feetPosition + _heightPositionMod, Quaternion.identity);
	}
	void CreateStunEffect()
	{
		if (_currentStunEffect != null) {
			RemoveStunEffect();
		}
		_currentStunEffect = Instantiate(_stunEffect, this.transform.position + _heightPositionMod,Quaternion.identity) as GameObject;
	}

	void RemoveStunEffect()
	{
		Destroy(_currentStunEffect.gameObject);
	}

	void CreateWallSlideEffect(GameObject obj)
	{
		if (_currentWallSlideEffectFeet != null && _currentWallSlideEffectHands != null) {
			RemoveWallSlideEffect(obj);
		}

		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		Vector3 handPosition = this.transform.position + new Vector3(1,0.5f,0);
		_currentWallSlideEffectFeet = Instantiate(_wallSlideEffect, feetPosition + _heightPositionMod, Quaternion.identity) as GameObject;
		_currentWallSlideEffectHands = Instantiate(_wallSlideEffect, handPosition + _heightPositionMod, Quaternion.identity) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
		{
			newLocalScale.x = -1;
		}
		_currentWallSlideEffectHands.transform.localScale = newLocalScale;
		_currentWallSlideEffectFeet.transform.localScale = newLocalScale;
	}
	
	void RemoveWallSlideEffect(GameObject obj)
	{
		if (_currentWallSlideEffectFeet != null && _currentWallSlideEffectHands != null) {
			Destroy (_currentWallSlideEffectHands.gameObject);
			Destroy (_currentWallSlideEffectFeet.gameObject);
		}
	}

	void CreateJumpEffect () 
	{
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentJumpParticles = Instantiate(_jumpEffect, feetPosition + _heightPositionMod, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x > 0)
			newLocalScale.x = -1;
		currentJumpParticles.transform.localScale = newLocalScale;
	}

	void CreateStartRunEffect()
	{
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentStartRunParticles = Instantiate(_startRunEffect, feetPosition + _heightPositionMod, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentStartRunParticles.transform.localScale = newLocalScale;
	}

	void CreateDashEffect()
	{
		//new Vector3((Mathf.Abs(transform.localScale.x) / transform.localScale.x) * 2,0,0)
		_currentDashEffect = Instantiate(_dashEffect, this.transform.position + _heightPositionMod, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x > 0)
			newLocalScale.x = -1;
		_currentDashEffect.transform.localScale = newLocalScale;
	}

	void CreateClashEffect()
	{
		GameObject currentClashParticles = Instantiate(_clashEffect, this.transform.position + _heightPositionMod, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentClashParticles.transform.localScale = newLocalScale;
	}
}
