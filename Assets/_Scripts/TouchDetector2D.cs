using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchDetector2D : MonoBehaviour {

	//delegates and events
	public delegate void VecDelegate (Vector2 vec);
	public delegate void GoVecDelegate (GameObject obj,Vector2 vec);
	
	public event GoVecDelegate TouchStarted;
	public event VecDelegate TouchEnded;

	//Check 
	private Dictionary<Vector2,bool> _sidesTouched = new Dictionary<Vector2, bool>();
	private Vector2[] _sidesToCheck = new Vector2[]{Vector2.up,Vector2.down,Vector2.right,Vector2.left}; 

	// Collider variables
	private BoxCollider2D colliderBox;
	private Vector2 centerCollider;
	private Vector2 sizeCollider;

	void Awake(){
		_sidesTouched.Add (Vector2.up, false);
		_sidesTouched.Add (Vector2.right, false);
		_sidesTouched.Add (Vector2.down, false);
		_sidesTouched.Add (Vector2.left, false);

		colliderBox = GetComponent<BoxCollider2D> ();
		sizeCollider = colliderBox.size;
		centerCollider = new Vector2 (sizeCollider.x / 2, sizeCollider.y / 2);
	}

	// Update is called once per frame
	void Update () {
		RaycastHit2D hit;
		float dist; 
		Vector2 currentDirVector;
		Vector2 startRay;
		for (int i = 0; i < _sidesToCheck.Length; i++) {
			currentDirVector = _sidesToCheck[i];
			startRay = new Vector2(transform.position.x,transform.position.y);
			if(i <= 1){
				dist = sizeCollider.y / 2;
				dist += dist * 0.1f;
				startRay.y += dist * currentDirVector.y;
			}else{
				dist = sizeCollider.x / 2;
				dist += dist * 0.1f;
				startRay.x += dist * currentDirVector.x;
			}
			hit = Physics2D.Raycast(startRay,currentDirVector,0.01f);
		
			if(hit.collider != null && hit.collider.gameObject != this.gameObject){
				if(!_sidesTouched[currentDirVector]){
					if(TouchStarted != null){
						TouchStarted(hit.collider.gameObject,currentDirVector);
						_sidesTouched[currentDirVector] = true;
					}
				}
			}else if(_sidesTouched[currentDirVector]){
				if(TouchEnded != null){
					TouchEnded(currentDirVector);
				}
				_sidesTouched[currentDirVector] = false;
			}
		}
	}
}
