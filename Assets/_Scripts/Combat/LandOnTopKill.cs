using UnityEngine;
using System.Collections;

public class LandOnTopKill : AttackBase {

	private TouchDetector2D _touchDetector;

	// Use this for initialization
	void Start () {
		_touchDetector = GetComponent<TouchDetector2D> ();
		_touchDetector.TouchStarted += OnTouch;
	}
	
	// Update is called once per frame
	void OnTouch (GameObject objHit, Vector2 touchVector) {
		if (touchVector == Vector2.down) {
			if(Hit(objHit, AttackBase.STUN_POWER_KILL_WHILE_STUNNED))
				GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(gameObject.transform.localScale.x) / gameObject.transform.localScale.x * 2,5);
		}
	}
}
