using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchCountDown : MonoBehaviour {

	private GameObject _textFieldObject;
	private Text _textfield; 

	void Start () {
		GameController gController = GameObject.FindGameObjectWithTag (Tags.GAMECONTROLLER).GetComponent<GameController>();
		gController.CountDownTik += CountDownTik;

		_textFieldObject = GameObject.Find("UICountDown");//Instantiate(Resources.Load("Prefabs/UI/UICountDown") as GameObject,new Vector3(0,0,0),Quaternion.identity) as GameObject;

		//_textFieldObject.transform.SetParent (gameObject.transform,true);
		_textfield = _textFieldObject.GetComponent<Text> ();	
		_textfield.alignment = TextAnchor.MiddleCenter;
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
