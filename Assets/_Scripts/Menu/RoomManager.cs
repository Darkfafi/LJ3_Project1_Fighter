using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {
	public GameObject[] playerPanels = new GameObject[0];
	public Text[] playerTexts = new Text[0];
	private List<string> _controlsInUse = new List<string>();
	private List<CharacterSelect> _playersReady = new List<CharacterSelect>();
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
		} else if(Input.GetButtonDown("JoystickActionKey1") || Input.GetButtonDown("JoystickJumpKey1"))
		{
			AddPlayer(Controls.joystick01);
		} else if(Input.GetButtonDown("JoystickActionKey2") || Input.GetButtonDown("JoystickJumpKey2"))
		{
			AddPlayer(Controls.joystick02);
		} else if(Input.GetButtonDown("JoystickActionKey3") || Input.GetButtonDown("JoystickJumpKey3"))
		{
			AddPlayer(Controls.joystick03);
		} else if(Input.GetButtonDown("JoystickActionKey4") || Input.GetButtonDown("JoystickJumpKey4"))
		{
			AddPlayer(Controls.joystick04);
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//TODO: go back to main menu + reset character selection
		}
		/*
		//FORCE START GAME
		if(Input.GetKeyDown(KeyCode.Return))
		{
			StartGame();
		} */
	}
	public void AddPlayerReady(CharacterSelect player)
	{
		_playersReady.Add(player);
		if(_playersReady.Count == _playerCount && _playerCount > 1)
		{
			StartGame();
		}
	}
	public void RemovePlayerReady(CharacterSelect player)
	{
		_playersReady.Remove(player);
	}
	private void StartGame()
	{
		PlayerPrefs.SetInt("PlayerCount", _playerCount);
		Application.LoadLevel(1);
	}
	private void AddPlayer(string controls)
	{
		//check if the controls are already in use.
		bool controlsInUse = false;
		foreach(string curControls in _controlsInUse)
		{
			if(curControls == controls)
			{
				controlsInUse = true;
				break;
			}
		}
		if(!controlsInUse)
		{
			//add player if the controls are not in use.
			_controlsInUse.Add(controls);
			List<string> playerControls = Controls.GetControls(controls);
			PlayerPrefs.SetString("Horizontal-" + _playerCount, playerControls[0]);
			PlayerPrefs.SetString("Vertical-" + _playerCount, playerControls[1]);
			PlayerPrefs.SetString("Action-" + _playerCount, playerControls[2]);
			ActivatePanel(controls);
			_playerCount++;
		}
	}
	private void ActivatePanel(string controls)
	{
		//activate character select panel
		playerTexts[_playerCount].text = "Player-" + _playerCount;
		playerPanels[_playerCount].SetActive(true);
		playerPanels[_playerCount].GetComponent<CharacterSelect>().SetPlayer(_playerCount,controls);
	}
}
