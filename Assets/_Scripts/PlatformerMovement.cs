using UnityEngine;
using System.Collections;

public class PlatformerMovement : MonoBehaviour {

	public static int DIR_RIGHT = 1;
	public static int DIR_LEFT = -1;

	private bool _inWallSlide = false;
	private bool _onGround = false;

	private BoxCollider2D colliderBox;
	private Vector2 centerCollider;
	private Vector2 sizeCollider;

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

		if(hit.collider != null){
			_onGround = true;
		}else{
			_onGround = false;
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
}
