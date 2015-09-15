using UnityEngine;
using System.Collections;

public class RigidbodyUtil2D : MonoBehaviour {
	//This util is for pausing the rigidbody velocity etc.
	private Vector2 _savedVelocity;
	private float _savedAngularVelocity;
	private Rigidbody2D _rigidbody;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		GameController gameController = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<GameController>();
		gameController.PauseGame += OnPauseGame;
		gameController.ResumeGame += OnResumeGame;
	}

	void OnPauseGame() {
		_savedVelocity = _rigidbody.velocity;
		_savedAngularVelocity = _rigidbody.angularVelocity;
		_rigidbody.isKinematic = true;
	}
	
	void OnResumeGame() {
		_rigidbody.isKinematic = false;
		_rigidbody.velocity = _savedVelocity;
		_rigidbody.angularVelocity = _savedAngularVelocity;
	}
}
