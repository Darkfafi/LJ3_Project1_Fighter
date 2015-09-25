using UnityEngine;
using System.Collections;

public class SpecialItemMovement : MonoBehaviour {
	private float _springForce = 10f;
	private float _levelBorderMinY = -10f;
	private float _levelBorderMaxY = 10f;
	private float _levelBorderMinX = -10f;
	private float _levelBorderMaxX = 10f;

	void Start () {
		GameObject boundsGameObject = GameObject.FindGameObjectWithTag (Tags.SCREEN_BOUND_OBJECT);
		
		SpriteRenderer rndr = boundsGameObject.gameObject.GetComponent<SpriteRenderer> ();
		
		_levelBorderMaxX = boundsGameObject.transform.position.x + ((rndr.bounds.size.x * boundsGameObject.transform.localScale.x) / 2) + 3;
		_levelBorderMinX = boundsGameObject.transform.position.x - ((rndr.bounds.size.x * boundsGameObject.transform.localScale.x) / 2) - 3;
		_levelBorderMaxY = boundsGameObject.transform.position.y + ((rndr.bounds.size.y * boundsGameObject.transform.localScale.y) / 2) + 3;
		_levelBorderMinY = boundsGameObject.transform.position.y - ((rndr.bounds.size.y * boundsGameObject.transform.localScale.y) / 2) - 3;

		GetComponent<SpecialItem>().StartBall += StartMoving;
	}
	void StartMoving()
	{
		GetComponent<Rigidbody2D>().velocity = transform.right * _springForce;
	}
	void Update() {
		//if this is out of bounds destroy itself.
		//TODO: check if there is a better solution
		if(this.transform.position.x > _levelBorderMaxX || this.transform.position.x < _levelBorderMinX || this.transform.position.y > _levelBorderMaxY || this.transform.position.y < _levelBorderMinY)
			Destroy(this.gameObject);
	}
}
