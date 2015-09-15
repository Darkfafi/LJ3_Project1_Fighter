using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour {
	public GameObject jumpParticles;
	// Use this for initialization
	void Start () {
		GetComponent<PlatformerMovement>().Jumped += CreateJumpEffect;
	}

	void CreateJumpEffect () 
	{
		//Placeholder particle system to check if it looks nice
		Vector3 feetPosition = this.transform.position + new Vector3(0,-1,0);
		GameObject currentJumpParticles = Instantiate(jumpParticles, feetPosition, this.transform.rotation) as GameObject;
		Vector3 newLocalScale = new Vector3(1,1,1);
		if(this.transform.localScale.x < 0)
			newLocalScale.x = -1;
		currentJumpParticles.transform.localScale = newLocalScale;
	}

	void CreateStartRunEffect()
	{

	}

	void CreateDashEffect()
	{

	}

	void CreateClashEffect()
	{

	}
}
