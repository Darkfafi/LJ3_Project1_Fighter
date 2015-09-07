using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private TouchDetector2D _touchDetector;

	// Use this for initialization
	void Awake () {
		_touchDetector = gameObject.AddComponent<TouchDetector2D> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Stun(float stunPower){

	}
}
