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
		if(this.transform.position.x > _levelBorderMaxX)
		{
			Vector3 newPos = new Vector3(_levelBorderMinX + 1,this.transform.position.y,this.transform.position.z);
			this.transform.position = newPos;
		} 
		else if (this.transform.position.x < _levelBorderMinX) 
		{
			Vector3 newPos = new Vector3(_levelBorderMaxX - 1,this.transform.position.y,this.transform.position.z);
			this.transform.position = newPos;
		}
		else if( this.transform.position.y > _levelBorderMaxY)
		{
			Vector3 newPos = new Vector3(this.transform.position.x,_levelBorderMinY + 1,this.transform.position.z);
			this.transform.position = newPos;
		}
		else if (this.transform.position.y < _levelBorderMinY)
		{
			Vector3 newPos = new Vector3(this.transform.position.x,_levelBorderMaxY - 1,this.transform.position.z);
			this.transform.position = newPos;
		}
	}
}
