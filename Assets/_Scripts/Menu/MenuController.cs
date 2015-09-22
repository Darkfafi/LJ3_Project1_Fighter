using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	public const string MENU_SCREEN = "MENENESCREEN";
	public const string RULES_SCREEN = "RULELESCREEN";
	public const string CHARACTER_SELECT_SCREEN = "CHARARASCREEN";
	public const string LEVEL_SELECT_SCREEN = "LEVEVESCREEN";

	private Dictionary<string,GameObject> _allScreens = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Awake() {
		AddScreen("MenuScreen",MENU_SCREEN,true);
		AddScreen("GameRulesSelectScreen",RULES_SCREEN);
		AddScreen("CharacterSelectScreen",CHARACTER_SELECT_SCREEN);
		AddScreen("LevelSelectScreen",LEVEL_SELECT_SCREEN);
	}
	
	public void GoToScreen(string keyName){
		foreach(KeyValuePair<string, GameObject> screen in _allScreens)
		{
			screen.Value.SetActive(false);
		}
		_allScreens [keyName].SetActive (true);
	}

	private void AddScreen(string gameObjectName,string keyName,bool setActive = false){
		GameObject screen = GameObject.Find(gameObjectName);
		screen.SetActive(setActive);
		_allScreens.Add (keyName, screen);
	}
}
