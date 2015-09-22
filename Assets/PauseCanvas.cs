using UnityEngine;
using System.Collections;

public class PauseCanvas : MonoBehaviour {
	public GameObject _optionsPanel;
	public GameObject _mainPanel;

	// Use this for initialization
	void Awake () {
		GameController gameController = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<GameController>();
		gameController.PauseGame += ShowMe;
		gameController.ResumeGame += HideMe;
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
}
