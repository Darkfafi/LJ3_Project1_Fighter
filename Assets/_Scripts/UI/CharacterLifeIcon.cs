using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterLifeIcon : MonoBehaviour {
		
	private Image _image;

	private List<GameObject> _allLives = new List<GameObject> ();

	private Player _player;
	private GameController gController;


	private void Awake(){
		_image = gameObject.AddComponent<Image> ();
		_image.rectTransform.sizeDelta = new Vector2 (95,90);

		gController = GameObject.FindGameObjectWithTag (Tags.GAMECONTROLLER).GetComponent<GameController> ();
		gController.OnDeath += PlayerDied;
		transform.localScale = new Vector3 (0.8f, 0.8f, 1);
		UpdateStocks (gController.playerTotalLives);
	}

	void PlayerDied(Player player, int lives){
		if (player == _player) {
			if(lives >= 0){
				UpdateStocks(lives);
			}
		}
	}

	private void UpdateStocks(int amountOfStocks){
		GameObject stockObject;
		Texture2D texture;
		Image imageStock;
		while (_allLives.Count != amountOfStocks) {
			if (_allLives.Count > amountOfStocks) {
				Destroy(_allLives[_allLives.Count - 1]);
				_allLives.RemoveAt(_allLives.Count - 1);
			} else if (_allLives.Count < amountOfStocks) {
				// create a stock
				stockObject = new GameObject();
				stockObject.transform.SetParent(transform);
				texture = Resources.Load("UI/stock") as Texture2D;
				imageStock = stockObject.AddComponent<Image>();
				imageStock.sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));

				imageStock.rectTransform.sizeDelta = new Vector2(20,20);
				imageStock.color = Color.white;

				stockObject.transform.position = new Vector3(_image.rectTransform.rect.width / 1.7f, _image.rectTransform.rect.height / 2 - (imageStock.rectTransform.rect.height * _allLives.Count),stockObject.transform.position.z);
				_allLives.Add(stockObject);
			}
		}
	}

	private void SetIcon(string playerType){
		Sprite characterIcon;
		Texture2D textureIcon;

		textureIcon = Resources.Load ("UI/characterIcon" + playerType) as Texture2D; //TODO de art moet geset worden aan de hand van de character
		characterIcon = Sprite.Create(textureIcon,new Rect(0,0,textureIcon.width,textureIcon.height),new Vector2(0.5f,0.5f));

		_image.sprite = characterIcon;
	}

	public void SetColor(Color color){
		_image.color = new Color(color.r,color.g,color.b,0.5f);
	}
	public void SetPlayer(Player player){
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
