using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Button : MonoBehaviour {


	[SerializeField] private Font _buttonFont;

	[SerializeField] private Sprite _idleImage;
	[SerializeField] private Sprite _hoverImage;
	[SerializeField] private Sprite _pressImage;

	[SerializeField] private string _idleButtonText;
	[SerializeField] private string _hoverButtonText;
	[SerializeField] private string _pressButtonText;

	[SerializeField] private Color _idleTextColor;
	[SerializeField] private Color _hoverTextColor;
	[SerializeField] private Color _pressTextColor;

	private Image _buttonImage;
	private Text _buttonText;

	// Use this for initialization
	public void Awake () {
		//gameObject.tag = Tags.BUTTON_MENU;
		_buttonImage = gameObject.AddComponent<Image> ();
		GameObject textFieldGo = new GameObject (); 
		textFieldGo.transform.SetParent (gameObject.transform);
		textFieldGo.transform.localScale = new Vector3 (1, 1, 1);

		_buttonText = textFieldGo.AddComponent<Text> ();

		if (_buttonFont == null) {
			_buttonText.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
		} else {
			_buttonText.font = _buttonFont;
		}

		_buttonText.text = "Button Text.";
		_buttonText.alignment = TextAnchor.MiddleCenter;
		if (_idleTextColor.a == 0) {
			_buttonText.color = Color.black;
		} else {
			_buttonText.color = _idleTextColor;
		}

		if (_hoverTextColor.a == 0) {
			_hoverTextColor = _idleTextColor;
		}

		if (_pressTextColor.a == 0) {
			_pressTextColor = _idleTextColor;
		}

		if (_pressButtonText == null) {
			_pressButtonText = _idleButtonText;
		}
		if (_hoverButtonText == null) {
			_hoverButtonText = _idleButtonText;
		}
		Idle ();
	}
	
	private void Idle(){
		changeButton(_idleImage,_idleButtonText,_idleTextColor);
	}

	private void Hover(){
		changeButton(_hoverImage,_hoverButtonText,_hoverTextColor);
	}
	private void Press(){
		changeButton (_pressImage, _pressButtonText, _pressTextColor);
	}

	private void changeButton(Sprite art, string text, Color color){
		_buttonImage.sprite = art;
		_buttonText.text = text;
		_buttonText.color = color;
	}
}
