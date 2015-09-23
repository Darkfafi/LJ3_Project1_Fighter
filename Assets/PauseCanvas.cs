using UnityEngine;
using System.Collections;

public class PauseCanvas : MonoBehaviour {
	public GameObject _optionsPanel;
	public GameObject _mainPanel;

	private GameController _gameController;
	// Use this for initialization
	void Awake () {
		_gameController = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<GameController>();
		_gameController.PauseGame += ShowMe;
		_gameController.ResumeGame += HideMe;
	}

	void Start()
	{
		HideMe();
	}
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
