using UnityEngine;
using System.Collections;

public class ThrowObject : MonoBehaviour {

	public const int DIR_RIGHT = 1;
	public const int DIR_LEFT = -1;

	private GameObject _thrower = null;
	private int _direction = 0;
	private float _speed = 0;
	private float _stunPower = 0;
	private float _pushPower = 0;

	public void SetStats(GameObject thrower,int throwDirection,float projectileSpeed,float stunPower, float pushPower){
		_thrower = thrower;
		_direction = throwDirection;
		_stunPower = stunPower;
		_pushPower = pushPower;
		_speed = projectileSpeed;
	}

	void Update(){
		Debug.Log(_speed +" | "+ _direction);
		transform.Translate(new Vector3 (_speed * _direction, 0,0) * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject != _thrower) {
			if(other.gameObject.GetComponent<AttackCather> () != null){
				other.gameObject.GetComponent<AttackCather>().CatchAttack(_thrower,_stunPower,_pushPower);
			}
			Destroy(this.gameObject);
		}
	}
}
