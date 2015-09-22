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
	public event KeyPressed NoKeyPressed;

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
		bool aKeyIsPressed = false;
		if(Input.GetAxis(_horizontalAxis) > 0)
		{
			//send right event
			aKeyIsPressed = true;
			if(RightKeyPressed != null)
				RightKeyPressed();
		} 
		else if(Input.GetAxis(_horizontalAxis) < 0)
		{
			//send left event
			aKeyIsPressed = true;
			if(LeftKeyPressed != null)
				LeftKeyPressed();
		}
		if(Input.GetAxis(_verticalAxis) < 0)
		{
			//send down event
			//aKeyIsPressed = true; <-- did this else idle would not be called when holding down this button. We need a release key function. Sorry
			if(DownKeyPressed != null)
				DownKeyPressed();
		}
		if(Input.GetButtonDown(_jumpKey))
		{
			//send up event
			if(JumpKeyPressed != null)
				JumpKeyPressed();
		}

		if(Input.GetButtonDown(_actionKey))
		{
			//send action event
			aKeyIsPressed = true;
			if(ActionKeyPressed != null)
					ActionKeyPressed();
		}
		if(!aKeyIsPressed)
		{
			if(NoKeyPressed != null)
				NoKeyPressed();
		}
	}
}
