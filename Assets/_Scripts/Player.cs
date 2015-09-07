using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private string _playerControls;
	private PlayerInput _myPlayerInput;
	private PlatformerMovement _myPlatformerMovement;
	private float _movementSpeed = 5f;
	private float _jumpForce = 10f;
	private float _fallSpeed = 2f;

	private TouchDetector2D _touchDetector;

	void Awake()
	{
		_playerControls = Controls.PLAYER01;
		_myPlayerInput = gameObject.AddComponent<PlayerInput>();
		_myPlatformerMovement = GetComponent<PlatformerMovement>();
		_touchDetector = gameObject.AddComponent<TouchDetector2D> ();
	}
	
	void Start()
	{
		_myPlayerInput.RightKeyPressed += MoveRight;
		_myPlayerInput.LeftKeyPressed += MoveLeft;
		_myPlayerInput.JumpKeyPressed += Jump;
		_myPlayerInput.DownKeyPressed += FallDown;
	}
	void MoveRight()
	{
		_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_RIGHT, _movementSpeed);
	}
	void MoveLeft()
	{
		_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT, _movementSpeed);
	}
	void Jump()
	{
		_myPlatformerMovement.Jump(_jumpForce);
	}
	void FallDown()
	{
		_myPlatformerMovement.MoveVertical(PlatformerMovement.DIR_DOWN, _fallSpeed);
	}
	public string playerControls{
		get{
			return _playerControls;
		}
	}
}
