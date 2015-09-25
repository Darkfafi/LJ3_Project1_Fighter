using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterLifeIcon : MonoBehaviour {

	private Image _image;
	private Sprite _icon;

	private Color _colorIcon;
	private List<Sprite> _allLives = new List<Sprite> ();

	private void Awake(){
		_image = gameObject.AddComponent<Image> ();
		_image.sprite = _icon;
	}

	public Sprite icon{
		set{_icon = value;}
	}
	public void SetColor(Color color){
		_colorIcon = color;
		_image.color = color;
	}

	public float width{
		get{return _image.rectTransform.rect.width;}
	}

	public float height{
		get{return _image.rectTransform.rect.height;}
	}
}
