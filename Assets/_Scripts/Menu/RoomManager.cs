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
		//Application.LoadLevel(1);

		GotoCharacterLevelSelect (MenuController.LEVEL_SELECT_SCREEN);
	}

	private void GotoCharacterLevelSelect(string levelKeyConst){
		GameObject.Find ("CanvasMenu").GetComponent<MenuController> ().GoToScreen(levelKeyConst);
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
			bool controlsAlreadySet = false;
			for (int i = 0; i < _controlsInUse.Count; i++) {
				if(_controlsInUse[i] == "")
				{
					_controlsInUse[i] = controls;
					controlsAlreadySet = true;
					break;
				}
			}

			if(!controlsAlreadySet)
				_controlsInUse.Add(controls);

			int playerID = _controlsInUse.IndexOf(controls);
			List<string> playerControls = Controls.GetControls(controls);
			PlayerPrefs.SetString("Horizontal-" + playerID, playerControls[0]);
			PlayerPrefs.SetString("Vertical-" + playerID, playerControls[1]);
			PlayerPrefs.SetString("Action-" + playerID, playerControls[2]);
			PlayerPrefs.SetString("Jump-" + playerID, playerControls[3]);
			PlayerPrefs.SetString("Back-" + playerID, playerControls[4]);
			ActivatePanel(controls, playerID);
			_playerCount++;
		}
	}
	private void ActivatePanel(string controls, int id)
	{
		//activate character select panel
		playerTexts[id].text = "Player-" + id;
		playerPanels[id].SetActive(true);
		playerPanels[id].GetComponent<CharacterSelect>().SetPlayer(id,controls);
	}
	public void DeactivatePanel(CharacterSelect charSelect, int playerID)
	{
		for (int i = 0; i < playerPanels.Length; i++) {
			if(playerPanels[i].GetComponent<CharacterSelect>() == charSelect)
			{
				playerPanels[i].SetActive(false);
				playerTexts[i].text = "Press action key";
			}
		}
		_controlsInUse[playerID] = "";
		_playerCount--;
	}
}
