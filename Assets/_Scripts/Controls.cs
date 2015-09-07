using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
	public const string PLAYER01 = "Player01";
	public const string PLAYER02 = "Player02";
	public const string PLAYER03 = "Player03";
	public const string PLAYER04 = "Player04";

	public const string JUMPKEY = "Jump";
	public const string DOWNKEY = "Down";
	public const string LEFTKEY = "Left";
	public const string RIGHTKEY = "Right";
	public const string ACTIONKEY = "Action";

	private Dictionary<string, KeyCode> _controls01 = new Dictionary<string, KeyCode>();
	private Dictionary<string, KeyCode> _controls02 = new Dictionary<string, KeyCode>();
	private Dictionary<string, KeyCode> _controls03 = new Dictionary<string, KeyCode>();
	private Dictionary<string, KeyCode> _controls04 = new Dictionary<string, KeyCode>();
	
	private Dictionary<string,Dictionary<string, KeyCode>> _controls = new Dictionary<string, Dictionary<string, KeyCode>>();

	// Use this for initialization
	void Awake () {
		_controls.Add(PLAYER01, _controls01);
		_controls.Add(PLAYER02, _controls02);
		_controls.Add(PLAYER03, _controls03);
		_controls.Add(PLAYER04, _controls04);

		//player01 basic keyinputs.
		ChangeKeys(_controls01, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.L);

		//player02 basic keyinputs.
		ChangeKeys(_controls02, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.J);
	}
	
	public void ChangeKeys(Dictionary<string, KeyCode> player ,KeyCode up, KeyCode down, KeyCode left, KeyCode right, KeyCode action)
	{
		player.Add(JUMPKEY, up);
		player.Add(LEFTKEY, left);
		player.Add(DOWNKEY, down);
		player.Add(RIGHTKEY, right);
		player.Add(ACTIONKEY, action);
	}

	public Dictionary<string,Dictionary<string, KeyCode>> controls {
		get{
			return _controls;
		}
	}
}
