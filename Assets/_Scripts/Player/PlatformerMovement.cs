using UnityEngine;
using System.Collections;

public class PlatformerMovement : MonoBehaviour {

	// public constants
	public static int DIR_RIGHT = 1;
	public static int DIR_LEFT = -1;
	public static int DIR_DOWN = -1;
	public static int DIR_UP = 1;

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

	//Physics
	private Rigidbody2D _rigidbody;

	//TouchDetector
	TouchDetector2D touch;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void Start(){
		touch = gameObject.GetComponent<TouchDetector2D> ();
		touch.TouchStarted += TouchDetectionStart;
		touch.TouchEnded += TouchDetectionEnd;
	}

	public void MoveHorizontal(int directionConst, float moveSpeed){

		if (transform.localScale.x != directionConst * Mathf.Abs(transform.localScale.x)) { //TODO dit moet in de platformer movement component
			transform.localScale = new Vector3 (directionConst * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}

		if (!touch.IsTouchingSide(new Vector2(directionConst,0))) {
			transform.Translate (new Vector3 (directionConst * moveSpeed, 0, 0) * Time.deltaTime);
		}

		//set velocity to 0 so you can move without getting resistance
		if(_rigidbody.velocity.x != 0)
		{
			_rigidbody.velocity = new Vector2(0,_rigidbody.velocity.y);
		}
	}

	public void MoveVertical(int directionConst, float moveSpeed)
	{
		transform.Translate (new Vector3 (0,directionConst * moveSpeed,0) * Time.deltaTime);
	}

	public void Jump(float jumpForce)
	{
		if(_onGround)
		{
			_rigidbody.velocity += new Vector2(0,jumpForce);
		} 
		else if(_inWallSlide)
		{
			//check wich direction you are currently sliding at + add velocity at negative direction.
			_rigidbody.velocity += new Vector2(-GetPlayerDirection() * jumpForce/2,jumpForce/2);
		}
	}

	public int GetPlayerDirection()
	{
		int dir = 0;
		if(transform.localScale.x > 0)
		{
			dir = 1;
		}
		else 
		{
			dir = 0;
		}
		return dir;
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
	public bool inWallSlide{
		get{return _inWallSlide;}
	}
}
