using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterLifeIcon : MonoBehaviour {

	private Image _image;
	private Sprite _icon;

	private Color _colorIcon;
	private List<Sprite> _allLives = new List<Sprite> ();

	private Player _player;

	private void Awake(){
		_image = gameObject.AddComponent<Image> ();
		_image.sprite = _icon;
		_image.rectTransform.sizeDelta = new Vector2 (95,90);
	}

	private void SetIcon(string playerType){
		Sprite characterIcon;
		Texture2D textureIcon;

		textureIcon = Resources.Load ("UI/characterIcon" + playerType) as Texture2D; //TODO de art moet geset worden aan de hand van de character
		characterIcon = Sprite.Create(textureIcon,new Rect(0,0,textureIcon.width,textureIcon.height),new Vector2(0.5f,0.5f));

		_image.sprite = characterIcon;
	}

	public void SetColor(Color color){
		_colorIcon = color;
		_image.color = new Color(color.r,color.g,color.b,0.5f);
	}
	public void SetPlayer(Player player){
		//TODO set Icon with player data maybe.
		_player = player;
		SetIcon (player.playerType);
		SetColor (CharDB.GetColorByID (player.playerID));
	}

	public float width{
		get{return _image.rectTransform.rect.width;}
	}

	public float height{
		get{return _image.rectTransform.rect.height;}
	}
}
