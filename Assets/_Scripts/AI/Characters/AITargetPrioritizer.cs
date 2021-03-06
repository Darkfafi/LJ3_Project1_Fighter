﻿using UnityEngine;
using System.Collections;

public class AITargetPrioritizer : MonoBehaviour {

	private AIPlayer _aiPlayer;

	// Use this for initialization
	void Awake () {
		_aiPlayer = GetComponent<AIPlayer> ();
	}
	
	// Update is called once per frame
	void Update () {
		CheckForBestTarget ();
	}

	void CheckForBestTarget(){
		GameObject[] players = GameObject.FindGameObjectsWithTag (Tags.PLAYER);
		GameObject specialItem = GameObject.FindGameObjectWithTag (Tags.SPECIAL_ITEM);

		GameObject bestPlayer = gameObject;
		GameObject bestTarget = null;


		for (int i = players.Length - 1; i >= 0; i--) {
			if(players[i].activeSelf && !players[i].GetComponent<Player>().CheckIfInBusyAction(Player.IN_DEATH) && !players[i].gameObject.Equals(gameObject)){
				if(bestPlayer.gameObject.Equals(gameObject)){
					bestPlayer = players[i];
				}else if(players[i].GetComponent<Player>().CheckIfInBusyAction(Player.IN_STUNNED)){
					bestPlayer = players[i];
				}else if(Mathf.Abs((players[i].transform.position - transform.position).magnitude) < Mathf.Abs((bestPlayer.transform.position - transform.position).magnitude)){
					bestPlayer = players[i];
				}
			}
		}
		bestTarget = bestPlayer;

		if (specialItem != null && Mathf.Abs ((specialItem.transform.position - transform.position).magnitude) < 4
		    && Mathf.Abs ((specialItem.transform.position - transform.position).magnitude) < Mathf.Abs ((bestPlayer.transform.position - transform.position).magnitude) + 2) {
			bestTarget = specialItem;
		}

		_aiPlayer.target = bestTarget;
	}
}
