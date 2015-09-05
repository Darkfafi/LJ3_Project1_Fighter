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

	public event GameObjectDelegate StartedSideTouch;
	public event GameObjectDelegate EndedSideTouch;

	// Current state.
	private bool _inWallSlide = false;
	private bool _onGround = false;

	// Collider variables
	private BoxCollider2D colliderBox;
	private Vector2 centerCollider;
	private Vector2 sizeCollider;

	// Remember Data
	private GameObject _preGround;


	void Start(){
		colliderBox = GetComponent<BoxCollider2D> ();
		sizeCollider = colliderBox.size;
		centerCollider = new Vector2 (sizeCollider.x / 2, sizeCollider.y / 2);
	}

	public void Move(int directionConst, float moveSpeed){
		transform.Translate (new Vector3 (directionConst * moveSpeed,0,0));
	}

	void Update(){
		RaycastHit2D hit;
		//check if landed on an object
		hit = Physics2D.Raycast(centerCollider,Vector2.down,sizeCollider.y / 2);

		if(hit.collider != null && !_onGround){
			_onGround = true;
			if(LandedOnGround != null){
				LandedOnGround(hit.collider.gameObject);
			}
			_preGround = hit.collider.gameObject;
		}else if(_onGround){
			_onGround = false;
			if(ReleasedFromGround != null){
				ReleasedFromGround(_preGround);
			}
		}
		if (!_onGround) {
			//check wallslide.
			hit = Physics2D.Raycast(centerCollider,Vector2.right,sizeCollider.x / 2);
			if(hit.collider != null){
				_inWallSlide = true;
			}else{
				hit = Physics2D.Raycast(centerCollider,Vector2.left,sizeCollider.x / 2); 
				if(hit != null){
					_inWallSlide = true;
				}else{
					_inWallSlide = false;
				}
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
