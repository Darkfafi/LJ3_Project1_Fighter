using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameUi : MonoBehaviour {

	private List<Player> _allPlayers = new List<Player>();

	void Start(){
		FindPlayers ();
		PlaceUI ();
	}

	private void FindPlayers(){
		GameObject[] playersFound = GameObject.FindGameObjectsWithTag (Tags.PLAYER);
		
		for (int i = 0; i < playersFound.Length; i++) {
			_allPlayers.Add(playersFound[i].GetComponent<Player>());
		}
	}

	private void PlaceUI(){
		Texture2D textureIcon;
		Sprite characterIcon;
		GameObject iconGo;
		CharacterLifeIcon charIcon;

		for (int i = 0; i < _allPlayers.Count; i++) {
			textureIcon = Resources.Load ("UI/characterIcon") as Texture2D; //TODO de art moet geset worden aan de hand van de character
			characterIcon = Sprite.Create(textureIcon,new Rect(0,0,textureIcon.width,textureIcon.height),new Vector2(0.5f,0.5f));

			iconGo = new GameObject ();
			iconGo.transform.SetParent (transform);
			charIcon = iconGo.AddComponent<CharacterLifeIcon> ();
			charIcon.icon = characterIcon;
			charIcon.transform.position = new Vector3((-(Screen.width / 100) / 2 + charIcon.width) + ((charIcon.width + 50) * i),charIcon.height / 1.3f,transform.position.z);
			charIcon.SetColor(Color.red); //TODO set color to player
		}
	}
}
