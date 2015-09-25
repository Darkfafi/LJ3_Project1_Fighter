using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonsControllerSupport : MonoBehaviour {

	private bool _usingController = false;

	private List<Button> _allButtons = new List<Button>();

	/*
	public void SetButtonsForNewScreen(){
		_allButtons = new List<Button> ();
		GameObject[] allFoundButtons = GameObject.FindGameObjectsWithTag (Tags.BUTTON_MENU);

		for (int i = 0; i < allFoundButtons.Length; i++) {
			_allButtons.Add(allFoundButtons[i].GetComponent<Button>());
		}
	}
*/
	void Update(){
		//gebruik using controller om te switchen tussen mouse controll of controller controll.
	}
}
