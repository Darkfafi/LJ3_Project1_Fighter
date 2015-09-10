using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {
	public GameObject[] playerPanels = new GameObject[0];
	public Text[] playerTexts = new Text[0];
	private int _playerCount = 0;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("ActionKeyPlayer1"))
		{
			AddPlayer(Controls.keyboard01);
		} else if(Input.GetButtonDown("ActionKeyPlayer2"))
		{
			AddPlayer(Controls.keyboard02);
		} else if(Input.GetButton("JoystickActionKey1"))
		{
			AddPlayer(Controls.joystick01);
		} else if(Input.GetButton("JoystickActionKey2"))
		{
			AddPlayer(Controls.joystick02);
		} else if(Input.GetButton("JoystickActionKey3"))
		{
			AddPlayer(Controls.joystick03);
		} else if(Input.GetButton("JoystickActionKey4"))
		{
			AddPlayer(Controls.joystick04);
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//TODO: go back to main menu + reset character selection
		}
	}
	private void AddPlayer(string controls)
	{
		List<string> playerControls = Controls.GetControls(controls);
		PlayerPrefs.SetString("Horizontal-" + _playerCount, playerControls[0]);
		PlayerPrefs.SetString("Vertical-" + _playerCount, playerControls[1]);
		PlayerPrefs.SetString("Action-" + _playerCount, playerControls[2]);
		ActivatePanel(controls);
		_playerCount++;
	}
	private void ActivatePanel(string controls)
	{
		playerTexts[_playerCount].text = "Player-" + _playerCount;
		playerPanels[_playerCount].SetActive(true);
	}
}
