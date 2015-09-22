using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour {
	private GameObject _stunEffect;
	private GameObject _jumpEffect;
	private GameObject _clashEffect;
	private GameObject _dashEffect;
	private GameObject _startRunEffect;
	private GameObject _wallSlideEffect;
	
	private GameObject _currentStunEffect;
	private GameObject _currentWallSlideEffectFeet;
	private GameObject _currentWallSlideEffectHands;
	// Use this for initialization
	void Start () {
		_stunEffect = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;
		_jumpEffect = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;
		_clashEffect = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;
		_dashEffect = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;
		_startRunEffect = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;
		_wallSlideEffect = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;


		PlatformerMovement myPlatformerMovement = GetComponent<PlatformerMovement>();
		myPlatformerMovement.Jumped += CreateJumpEffect;
		myPlatformerMovement.StartedRunning += CreateStartRunEffect;
		myPlatformerMovement.StartedWallSlide += CreateWallSlideEffect;
		myPlatformerMovement.EndedWallSlde += RemoveWallSlideEffect;

		GetComponent<BasicStunAttack>().AttackStarted += CreateDashEffect;
		GetComponent<ClashAble>().Clashed += CreateClashEffect;

		Player myPlayerScript = GetComponent<Player>();
		myPlayerScript.StartStunned += CreateStunEffect;
		myPlayerScript.StopStunned += RemoveStunEffect;
	}

	void Update()
	{
		if(_currentStunEffect != null)
		{
			_currentStunEffect.transform.position = this.transform.position;
		}
		if(_currentWallSlideEffectFeet != null && _currentWallSlideEffectHands != null)
		{
			Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
			Vector3 handPosition = this.transform.position + new Vector3(1,0,0);
			_currentWallSlideEffectFeet.transform.position = feetPosition;
			_currentWallSlideEffectHands.transform.position = handPosition;
		}
	}

	void CreateStunEffect()
	{
		_currentStunEffect = Instantiate(_stunEffect, this.transform.position,Quaternion.identity) as GameObject;
	}

	void RemoveStunEffect()
	{
		Destroy(_currentStunEffect.gameObject);
	}

	void CreateWallSlideEffect(GameObject obj)
	{
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		Vector3 handPosition = this.transform.position + new Vector3(1,0,0);
		_currentWallSlideEffectFeet = Instantiate(_wallSlideEffect, feetPosition, Quaternion.identity) as GameObject;
		_currentWallSlideEffectHands = Instantiate(_wallSlideEffect, handPosition, Quaternion.identity) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		_currentWallSlideEffectHands.transform.localScale = newLocalScale;
		_currentWallSlideEffectFeet.transform.localScale = newLocalScale;
	}
	
	void RemoveWallSlideEffect(GameObject obj)
	{
		Destroy(_currentWallSlideEffectHands.gameObject);
		Destroy(_currentWallSlideEffectFeet.gameObject);
	}

	void CreateJumpEffect () 
	{
		//Placeholder particle system to check if it looks nice
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentJumpParticles = Instantiate(_jumpEffect, feetPosition, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentJumpParticles.transform.localScale = newLocalScale;
	}

	void CreateStartRunEffect()
	{
		//Placeholder particle system to check if it looks nice
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentStartRunParticles = Instantiate(_startRunEffect, feetPosition, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentStartRunParticles.transform.localScale = newLocalScale;
	}

	void CreateDashEffect()
	{
		//Placeholder particle system to check if it looks nice
		GameObject currentDashParticles = Instantiate(_dashEffect, this.transform.position, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentDashParticles.transform.localScale = newLocalScale;
	}

	void CreateClashEffect()
	{
		//Placeholder particle system to check if it looks nice
		GameObject currentClashParticles = Instantiate(_clashEffect, this.transform.position, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentClashParticles.transform.localScale = newLocalScale;
	}
}
