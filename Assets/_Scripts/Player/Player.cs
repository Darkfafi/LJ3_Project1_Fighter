using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private PlayerInput _myPlayerInput;
	private PlatformerMovement _myPlatformerMovement;
	private float _movementSpeed = 5f;
	private float _jumpForce = 10f;
	private float _fallSpeed = 2f;

	private string _horizontalAxis;
	private string _verticalAxis;
	private string _actionKey;

	private TouchDetector2D _touchDetector;

	void Awake()
	{
		_myPlatformerMovement = GetComponent<PlatformerMovement>();
		_touchDetector = gameObject.AddComponent<TouchDetector2D> ();
		_myPlayerInput = gameObject.AddComponent<PlayerInput>();
	}
	
	void Start()
	{
		_myPlayerInput.RightKeyPressed += MoveRight;
		_myPlayerInput.LeftKeyPressed += MoveLeft;
		_myPlayerInput.JumpKeyPressed += Jump;
		_myPlayerInput.DownKeyPressed += FallDown;
	}

	public void SetCharacter(string characterName)
	{
		Animator newAnimator = gameObject.AddComponent<Animator>();
		newAnimator = CharDB.GetCharacterAnimator(characterName);

		//TODO: add special!
	}
	public void SetKeys(string playerHorizontalAxis,string playerVerticalAxis,string playerActionKey)
	{
		_horizontalAxis = playerHorizontalAxis;
		_verticalAxis = playerVerticalAxis;
		_actionKey = playerActionKey;
	}

	void MoveRight()
	{
		if(_myPlatformerMovement.sideTouching && PlatformerMovement.DIR_RIGHT != _myPlatformerMovement.GetPlayerDirection() || !_myPlatformerMovement.sideTouching)
		{
			_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_RIGHT, _movementSpeed);
		}
	}
	void MoveLeft()
	{
		if(_myPlatformerMovement.sideTouching && PlatformerMovement.DIR_LEFT != _myPlatformerMovement.GetPlayerDirection() || !_myPlatformerMovement.sideTouching)
		{
			_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT, _movementSpeed);
		}
	}
	void Jump()
	{
		_myPlatformerMovement.Jump(_jumpForce);
	}
	void FallDown()
	{
		_myPlatformerMovement.MoveVertical(PlatformerMovement.DIR_DOWN, _fallSpeed);
	}

	//public variables for keyinputs
	public string horizontalAxis{
		get {
			return _horizontalAxis;
		}
		set {
			_horizontalAxis = value;
		}
	}
	public string verticalAxis{
		get {
			return _verticalAxis;
		}
		set {
			_verticalAxis = value;
		}
	}
	public string actionKey{
		get {
			return _actionKey;
		}
		set {
			_verticalAxis = value;
		}
	}
}
