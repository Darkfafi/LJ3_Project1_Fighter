using UnityEngine;
using System.Collections;

public class PlatformerMovement : MonoBehaviour {

	// public constants
	public static int DIR_RIGHT = 1;
	public static int DIR_LEFT = -1;

	// public delegates and events
	public delegate void GameObjectDelegate (GameObject obj);

	public event GameObjectDelegate LandedOnGround;
	public event GameObjectDelegate ReleasedFromGround;

	public event GameObjectDelegate StartedWallSlide;
	public event GameObjectDelegate EndedWallSlde;

	// Current state.
	private bool _inWallSlide = false;
	private bool _onGround = false;

	// Collider variables
	private BoxCollider2D colliderBox;
	private Vector2 centerCollider;
	private Vector2 sizeCollider;

	// Remember Data
	private GameObject _preGround;
	private GameObject _preWall;


	TouchDetector2D touch;

	void Start(){
		touch = gameObject.AddComponent<TouchDetector2D> ();
		touch.TouchStarted += TouchDetectionStart;
		touch.TouchEnded += TouchDetectionEnd;
	}

	public void Move(int directionConst, float moveSpeed){
		transform.Translate (new Vector3 (directionConst * moveSpeed,0,0));
	}

	void Update(){
		Debug.Log (_onGround);
	}

	void TouchDetectionStart(GameObject obj, Vector2 vec){
		if (vec == Vector2.down) {
			// we can even say if its a player then no grounded but attack if stunned else ignore.
			_onGround = true;
			_preGround = obj;
			if(LandedOnGround != null){
				LandedOnGround(obj);
			}
		} else if (vec == Vector2.left || vec == Vector2.right) {
			if(!_onGround){
				_inWallSlide = true;
				if(StartedWallSlide != null){
					StartedWallSlide(obj);
				}
			}
		}
	}

	void TouchDetectionEnd(Vector2 vec){
		if (vec == Vector2.down) {
			_onGround = false;
			if (ReleasedFromGround != null) {
				ReleasedFromGround (_preGround);
			}
		} else if (vec == Vector2.left || vec == Vector2.right) {
			_inWallSlide = false;
			if(EndedWallSlde != null){
				EndedWallSlde(_preWall);
			}
		}
	}

	public bool onGround{
		get{return _onGround;}
	}
	public bool sideTouching{
		get{return _inWallSlide;}
	}
}
