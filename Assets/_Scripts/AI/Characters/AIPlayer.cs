using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour {

	//Level knowledge
	Vector2 levelBounds;

	//Pathfinding
	private Grid _grid;

	// Movement
	private AIMovement _movement;

	// Use this for initialization
	void Start () {
		Vector3 tempBounds;

		tempBounds = GameObject.FindGameObjectWithTag (Tags.SCREEN_BOUND_OBJECT).GetComponent<SpriteRenderer>().bounds.size;
		levelBounds = new Vector2 (tempBounds.x, tempBounds.y);

		_grid = new Grid ((int)(levelBounds.x / 5), (int)(levelBounds.y / 5));

		_movement = gameObject.AddComponent<AIMovement> ();
	}


	public Grid grid{
		get{return _grid;}
	}
}
