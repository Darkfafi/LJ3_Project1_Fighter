using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameUI : MonoBehaviour {
	
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
		GameObject iconGo;
		CharacterLifeIcon charIcon;
		GameObject timer;

		for (int i = 0; i < _allPlayers.Count; i++) {


			iconGo = new GameObject ();
			iconGo.transform.SetParent (transform);
			charIcon = iconGo.AddComponent<CharacterLifeIcon> ();
			charIcon.SetPlayer(_allPlayers[i]);
			charIcon.transform.position = new Vector3((-(Screen.width / 100) / 2 + charIcon.width) + ((charIcon.width + 50) * i),charIcon.height / 1.3f,transform.position.z);
		}

		timer = new GameObject ();
		timer.transform.SetParent (transform);
		timer.AddComponent<GameUIClock> ();
		timer.transform.position = new Vector3 (Screen.width / 2, Screen.height / 1.1f, timer.transform.position.z);
	}
}
