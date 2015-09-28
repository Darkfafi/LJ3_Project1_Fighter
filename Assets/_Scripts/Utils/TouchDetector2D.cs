using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchDetector2D : MonoBehaviour {

	//delegates and events
	public delegate void VecDelegate (Vector2 vec);
	public delegate void GoVecDelegate (GameObject obj,Vector2 vec);
	
	public event GoVecDelegate TouchStarted;
	public event GoVecDelegate OnTouch;
	public event GoVecDelegate TouchEnded;

	//Check 
	private Dictionary<Vector2,GameObject> _sidesTouched = new Dictionary<Vector2, GameObject>();
	private Vector2[] _sidesToCheck = new Vector2[]{Vector2.up,Vector2.down,Vector2.right,Vector2.left}; 

	// Collider variables
	private BoxCollider2D colliderBox;
	//private Vector2 centerCollider;
	private Vector2 sizeCollider;

	private GameObject objectTouched;

	void Awake(){
		_sidesTouched.Add (Vector2.up, null);
		_sidesTouched.Add (Vector2.right, null);
		_sidesTouched.Add (Vector2.down, null);
		_sidesTouched.Add (Vector2.left, null);

		colliderBox = GetComponent<BoxCollider2D> ();
		sizeCollider = colliderBox.size;
		sizeCollider.x *= transform.localScale.x;
		sizeCollider.y *= transform.localScale.y;
		//centerCollider = new Vector2 (sizeCollider.x / 2, sizeCollider.y / 2);
	}

	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit2D hit;
		float dist; 
		Vector2 currentDirVector;
		Vector2 startRay;
		for (int i = 0; i < _sidesToCheck.Length; i++) {
			currentDirVector = _sidesToCheck[i];
			startRay = new Vector2(transform.position.x,transform.position.y + sizeCollider.y / 2);
			if(i <= 1){
				dist = sizeCollider.y / 2;
				dist += dist * 0.12f;
				startRay.y += dist * currentDirVector.y;
			}else{
				dist = sizeCollider.x / 2;
				dist += dist * 0.12f;
				startRay.x += dist * currentDirVector.x;
			}
			hit = Physics2D.Raycast(startRay,currentDirVector,0.01f);
		
			if(hit.collider != null && hit.collider.gameObject != this.gameObject){
				if(!_sidesTouched[currentDirVector]){
					if(TouchStarted != null){
						TouchStarted(hit.collider.gameObject,currentDirVector);
						objectTouched = hit.collider.gameObject;
						_sidesTouched[currentDirVector] = hit.collider.gameObject;
					}
				}
				if(OnTouch != null){
					OnTouch(hit.collider.gameObject,currentDirVector);
				}
			}else if(_sidesTouched[currentDirVector]){
				if(TouchEnded != null){
					TouchEnded(objectTouched, currentDirVector);
				}
				_sidesTouched[currentDirVector] = null;
			}
		}
	}

	public bool IsTouchingSide(Vector2 sideDirection){
		return _sidesTouched[sideDirection] != null;
	}

	public GameObject IsTouchingSideGetGameObject(Vector2 sideDirection){
		return _sidesTouched[sideDirection];
	}

	public GameObject GetObjectCurrentlyTouching()
	{
		return objectTouched;
	}
}
