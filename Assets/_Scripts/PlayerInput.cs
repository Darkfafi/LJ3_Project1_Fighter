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

	private KeyCode _right;
	private KeyCode _left;
	private KeyCode _jump;
	private KeyCode _down;
	private KeyCode _action;
	
	void Awake () 
	{
		string playerID = GetComponent<Player>().playerControls;
		Controls controls = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<Controls>();
		Dictionary<string,KeyCode> currentControls = controls.controls[playerID];

		_right = currentControls[Controls.RIGHTKEY];
		_left = currentControls[Controls.LEFTKEY];
		_jump = currentControls[Controls.JUMPKEY];
		_down = currentControls[Controls.DOWNKEY];
		_action = currentControls[Controls.ACTIONKEY];

	}

	void Update () 
	{
		Inputs();
	}
	private void Inputs()
	{
		if(Input.GetKey(_right))
		{
			//send right event
			if(RightKeyPressed != null)
				RightKeyPressed();
		} 
		else if(Input.GetKey(_left))
		{
			//send left event
			if(LeftKeyPressed != null)
				LeftKeyPressed();
		}
		if(Input.GetKey(_down))
		{
			//send down event
			if(DownKeyPressed != null)
				DownKeyPressed();
		}
		else if(Input.GetKeyDown(_jump))
		{
			//send up event
			if(JumpKeyPressed != null)
				JumpKeyPressed();
		}
		if(Input.GetKey(_action))
		{
			//send action event
			if(ActionKeyPressed != null)
				ActionKeyPressed();
		}
	}
}
