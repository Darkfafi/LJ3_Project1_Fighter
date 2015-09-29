using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterLifeIcon : MonoBehaviour {
		
	private Image _image;

	[SerializeField] private GameObject[] _allLives;

	private Player _player;
	private GameController gController;


	public void CreateIconStart(){
		_image = gameObject.GetComponent<Image> ();

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

		for (int i = 0; i < _allLives.Length; i++) {
			_allLives[i].SetActive(false);
		}
		for (int i = 0; i < amountOfStocks; i++) {
			_allLives[i].SetActive(true);
		}

		if (amountOfStocks == 0) {
			transform.localScale = new Vector3 (0.6f, 0.6f, 1);
			_image.color = new Color(0.8f,0.8f,0.8f,0.5f);
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
