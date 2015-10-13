using UnityEngine;
using System.Collections;

public class PlayerAnimationHandler : MonoBehaviour {

	Player player;
	PlayerStats playerStats;
	Animator animator;

	void Start(){
		player = gameObject.GetComponent<Player> ();
		playerStats = player.playerStats;
		animator = gameObject.GetComponent<Animator> ();
		animator.speed = 5;
	}

	public void PlayAnimation(string animationName){
		string addOnString = "";
		if (animationName != "") {
			if (playerStats.transformed) {
				addOnString = "Transform";
			}
			animator.Play (addOnString + animationName);
		}
	}
}
