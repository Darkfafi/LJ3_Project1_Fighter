using UnityEngine;
using System.Collections;

public class SpecialItemMovement : MonoBehaviour {
	private float _springForce = 10f;
	private float _maxX = 10f;
	private float _minX = -10f;
	private float _maxY = 10f;
	private float _minY = -10f;

	void Start () {
		GetComponent<SpecialItem>().StartBall += StartMoving;
	}
	void StartMoving()
	{
		GetComponent<Rigidbody2D>().velocity = transform.right * _springForce;
	}
	void Update() {
		//if this is out of bounds destroy itself.
		//TODO: check if there is a better solution
		if(this.transform.position.x > _maxX || this.transform.position.x < _minX || this.transform.position.y > _maxY || this.transform.position.y < _minY)
			Destroy(this.gameObject);
	}
}
