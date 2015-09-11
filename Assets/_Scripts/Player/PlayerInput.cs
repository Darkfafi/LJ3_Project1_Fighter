using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {
	public delegate void KeyPressed();
	public event KeyPressed RightKeyPressed;
	public event KeyPressed JumpKeyPressed;
	public event KeyPressed LeftKeyPressed;
	public event KeyPressed DownKeyPressed;
	public event KeyPressed ActionKeyPressed;

	private string _horizontalAxis;
	private string _verticalAxis;
	private string _actionKey;
	private string _jumpKey; //this is only available for joystick users

	void Start()
	{
		Player myPlayerScript = GetComponent<Player>();
		_horizontalAxis = myPlayerScript.horizontalAxis;
		_verticalAxis = myPlayerScript.verticalAxis;
		_actionKey = myPlayerScript.actionKey;
		_jumpKey = myPlayerScript.jumpKey;
	}

	void Update () 
	{
		if(!GameController.isPaused)
			Inputs ();
	}

	private void Inputs()
	{
		if(Input.GetAxis(_horizontalAxis) > 0)
		{
			//send right event
			if(RightKeyPressed != null)
				RightKeyPressed();
		} 
		else if(Input.GetAxis(_horizontalAxis) < 0)
		{
			//send left event
			if(LeftKeyPressed != null)
				LeftKeyPressed();
		}
		if(Input.GetAxis(_verticalAxis) < 0)
		{
			//send down event
			if(DownKeyPressed != null)
				DownKeyPressed();
		}
		if(Input.GetAxis(_verticalAxis) > 0 || Input.GetButtonDown(_jumpKey))
		{
			//send up event
			if(JumpKeyPressed != null)
				JumpKeyPressed();
		}

		if(Input.GetButtonDown(_actionKey))
		{
			//send action event
			if(ActionKeyPressed != null)
					ActionKeyPressed();
		}
	}
}
