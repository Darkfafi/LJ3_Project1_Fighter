using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour {
	private GameObject _placeHolderParticles;
	// Use this for initialization
	void Start () {
		_placeHolderParticles = Resources.Load("Prefabs/placeholderparticle", typeof(GameObject)) as GameObject;

		GetComponent<PlatformerMovement>().Jumped += CreateJumpEffect;
		GetComponent<PlatformerMovement>().StartRunning += CreateStartRunEffect;
		GetComponent<BasicStunAttack>().AttackStarted += CreateDashEffect;
		GetComponent<ClashAble>().Clashed += CreateClashEffect;
	}

	void CreateJumpEffect () 
	{
		//Placeholder particle system to check if it looks nice
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentJumpParticles = Instantiate(_placeHolderParticles, feetPosition, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentJumpParticles.transform.localScale = newLocalScale;
	}

	void CreateStartRunEffect()
	{
		//Placeholder particle system to check if it looks nice
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentStartRunParticles = Instantiate(_placeHolderParticles, feetPosition, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentStartRunParticles.transform.localScale = newLocalScale;
	}

	void CreateDashEffect()
	{
		//Placeholder particle system to check if it looks nice
		GameObject currentDashParticles = Instantiate(_placeHolderParticles, this.transform.position, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentDashParticles.transform.localScale = newLocalScale;
	}

	void CreateClashEffect()
	{
		//Placeholder particle system to check if it looks nice
		GameObject currentClashParticles = Instantiate(_placeHolderParticles, this.transform.position, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentClashParticles.transform.localScale = newLocalScale;
	}
}
