using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchCountDown : MonoBehaviour {

	private GameObject _textFieldObject;
	private Text _textfield; 

	void Start () {
		GameController gController = GameObject.FindGameObjectWithTag (Tags.GAMECONTROLLER).GetComponent<GameController>();
		gController.CountDownTik += CountDownTik;

		_textFieldObject = new GameObject ();

		_textFieldObject.transform.SetParent (gameObject.transform);
		_textfield = _textFieldObject.AddComponent<Text> ();

		_textFieldObject.transform.position = new Vector3 ((Screen.width / 2) - _textfield.rectTransform.rect.width, (Screen.height / 2) - _textfield.rectTransform.rect.height, 1);

		_textfield.rectTransform.sizeDelta = new Vector2 (117, 60);
		_textfield.font = Resources.Load ("Fonts/Ailerons") as Font;
		_textfield.alignment = TextAnchor.MiddleCenter;
		_textfield.fontSize = 50;
	}

	void CountDownTik (int counts) {
		switch (counts) {
		case 1:
			_textfield.text = "READY";
			break;
		case 2:
			_textfield.color = Color.yellow;
			_textfield.text ="SET";
			break;
		case 3:
			_textfield.color = Color.red;
			_textfield.text = "FIGHT";
			Destroy(_textFieldObject,1f);
			break;
		}
	}
}
