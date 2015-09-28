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

		GameObject[] allIcons = GameObject.FindGameObjectsWithTag (Tags.PLAYER_LIFE_ICON);

		for (int i = 0; i < allIcons.Length; i++) {
			allIcons[i].SetActive(false);
		}

		for (int i = 0; i < _allPlayers.Count; i++) {

			allIcons[allIcons.Length - 1 - i].GetComponent<CharacterLifeIcon>().SetPlayer(_allPlayers[i]);
			allIcons[allIcons.Length - 1 - i].SetActive(true);
		}
		gameObject.AddComponent<MatchCountDown> ();
		timer = GameObject.Find("GameUIClock");
		timer.SetActive (false);
	}
}
