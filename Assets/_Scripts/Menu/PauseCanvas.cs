using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseCanvas : MonoBehaviour {
	public GameObject _optionsPanel;
	public GameObject _mainPanel;
	
	private GameController _gameController;
	//private List<List<string>> allControlls = new List<List<string>>();
	//private float keyCooldown;
	//private float currentKeyCooldown;
	//private int currentButton;
	// Use this for initialization
	void Awake () {
		_gameController = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<GameController>();
		_gameController.PauseGame += ShowMe;
		_gameController.ResumeGame += HideMe;
	}

	void Start()
	{
		//keyCooldown = 0.25f;
		HideMe();
	}
	/*
	void Update()
	{
		if(allControlls.Count == 0)
		{
			foreach(KeyValuePair<string, List<string>> entry in Controls.controls)
			{
				allControlls.Add(entry.Value);
			}
		}
		for (int i = 0; i < allControlls.Count; i++) {
			if(Time.time > currentKeyCooldown)
			{
				if(Input.GetAxis(allControlls[i][1]) > 0)
				{
					GoUp();
					currentKeyCooldown = Time.time + keyCooldown;
				} 
				else if(Input.GetAxis(allControlls[i][1]) < 0)
				{
					GoDown();
					currentKeyCooldown = Time.time + keyCooldown;
				} 
				else if(Input.GetButtonDown(allControlls[i][2]))
				{
					PressButton();
					currentKeyCooldown = Time.time + keyCooldown;
				}
			}
		}
	}
	void GoUp()
	{
		Debug.Log("Going up!");
		currentButton++;
	}
	void GoDown()
	{
		Debug.Log("Going down!");
		currentButton--;
	}
	void PressButton()
	{
		Debug.Log("pressing button!");
	} */

	void ShowMe () 
	{
		gameObject.SetActive(true);		
		_mainPanel.SetActive(true);
		_optionsPanel.SetActive(false);
	}

	void HideMe ()
	{
		gameObject.SetActive(false);
	}

	public void ResumeButtonPressed()
	{
		_gameController.SwitchPause();
		HideMe();
	}

	public void QuitButtonPressed()
	{
		Application.LoadLevel(0);
	}
}
