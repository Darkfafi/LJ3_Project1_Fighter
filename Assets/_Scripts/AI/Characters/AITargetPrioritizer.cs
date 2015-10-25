using UnityEngine;
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

		for (int i = players.Length - 1; i >= 0; i--) {
			if(!players[i].GetComponent<Player>().CheckIfInBusyAction(Player.IN_DEATH)){
				if(bestPlayer.gameObject.Equals(gameObject)){
					bestPlayer = players[i];
				}else if(players[i].GetComponent<Player>().CheckIfInBusyAction(Player.IN_STUNNED)){
					bestPlayer = players[i];
				}else if(Mathf.Abs((players[i].transform.position - transform.position).magnitude) < Mathf.Abs((bestPlayer.transform.position - transform.position).magnitude)){
					bestPlayer = players[i];
				}
			}
		}

		_aiPlayer.target = bestPlayer;
	}
}
