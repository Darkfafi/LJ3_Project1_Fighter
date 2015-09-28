using UnityEngine;
using System.Collections;

public class LandOnTopKill : AttackBase {

	private Player _player;
	private TouchDetector2D _touchDetector;

	// Use this for initialization
	void Start () {
		_player = GetComponent<Player> ();
		_touchDetector = GetComponent<TouchDetector2D> ();
		_touchDetector.TouchStarted += OnTouch;
	}
	
	// Update is called once per frame
	void OnTouch (GameObject objHit, Vector2 touchVector) {
		if (touchVector == Vector2.down 
		    && (_touchDetector.IsTouchingSideGetGameObject(Vector2.left) != objHit 
		    && _touchDetector.IsTouchingSideGetGameObject(Vector2.up) != objHit 
		    && _touchDetector.IsTouchingSideGetGameObject(Vector2.right) != objHit)) {

			if(objHit.tag != Tags.SPECIAL_ITEM && !_player.busyAction){
				if(Hit(objHit, AttackBase.STUN_POWER_KILL_WHILE_STUNNED))
					GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(gameObject.transform.localScale.x) / gameObject.transform.localScale.x * 2,5);
			}
		}
	}
}
