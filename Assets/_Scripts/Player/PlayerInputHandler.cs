using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {

	private Player _player;

	// Input
	private PlayerInput _myPlayerInput;
	private string _horizontalAxis = "HorizontalPlayer1";
	private string _verticalAxis = "VerticalPlayer1";
	private string _actionKey = "ActionKeyPlayer1";
	private string _jumpKey = "Null";


	// Use this for initialization
	void Awake () {
		_myPlayerInput = gameObject.AddComponent<PlayerInput> ();
		_player = gameObject.GetComponent<Player> ();
	}

	void Start(){
		_myPlayerInput.RightKeyPressed += _player.MoveRight;
		_myPlayerInput.LeftKeyPressed += _player.MoveLeft;
		_myPlayerInput.JumpKeyPressed += _player.Jump;
		_myPlayerInput.DownKeyPressed += _player.FallDown;
		_myPlayerInput.ActionKeyPressed += _player.DoAction;
		_myPlayerInput.NoKeyPressed += _player.OnNoKeyPressed;
	}

	public void SetKeys(string playerHorizontalAxis,string playerVerticalAxis,string playerActionKey, string playerJumpKey = "Null")
	{
		_horizontalAxis = playerHorizontalAxis;
		_verticalAxis = playerVerticalAxis;
		_actionKey = playerActionKey;
		_jumpKey = playerJumpKey;
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
	public string jumpKey{
		get {
			return _jumpKey;
		}
		set {
			_jumpKey = value;
		}
	}
}
