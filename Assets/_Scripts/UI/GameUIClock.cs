using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIClock : MonoBehaviour {

	
	private GameController _gController;
	private Text _textfield;


	// Use this for initialization
	void Start () {
		_gController = GameObject.FindGameObjectWithTag (Tags.GAMECONTROLLER).GetComponent<GameController> ();

		_textfield = gameObject.AddComponent<Text> ();
		_textfield.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
		_textfield.fontSize = 40;
		_textfield.alignment = TextAnchor.MiddleCenter;
	}

	// Update is called once per frame
	void Update () {
		if (!_gController.gameTimer.paused) {
			_textfield.text = _gController.gameTimer.GetHumanTimeString();
		}
	}
}
