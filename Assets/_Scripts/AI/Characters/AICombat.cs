using UnityEngine;
using System.Collections;

public class AICombat : MonoBehaviour {

	private Player _player;
	private AIPlayer _aiPlayer;
	private AIMovement _aiMovement;

	void Awake(){
		_player = GetComponent<Player> ();
		_aiPlayer = GetComponent<AIPlayer> ();
		_aiMovement = GetComponent<AIMovement> ();
	}

	// Update is called once per frame
	void Update () {
		Combat ();
	}

	void Combat(){
		GameObject target = _aiPlayer.target;
		Player playerTarget = null;
		bool attackTarget = true;

		if (!_aiMovement.canDubbleJump) {
			_aiMovement.canDubbleJump = true;
		}

		if (target != null && target != gameObject) {
			_aiMovement.moveTarget = new Vector2 (target.transform.position.x, target.transform.position.y);

			if(target.GetComponent<Player>() != null){
				playerTarget = target.GetComponent<Player>();

				if(playerTarget.CheckIfInBusyAction(Player.IN_STUNNED)){
					if(playerTarget.GetComponent<Rigidbody2D>().velocity.x < 0.2f){
						_aiMovement.moveTarget = new Vector2(target.transform.position.x, target.transform.position.y + 2.3f);
						attackTarget = false;
					}
				}
			}

			if(attackTarget){
				if(Mathf.Abs((target.transform.position - transform.position).magnitude) < 2){
					if(attackTarget){
						_player.DoAction();
					}else if(playerTarget != null && playerTarget.CheckIfInBusyAction(Player.IN_STUNNED)){
						_aiMovement.canDubbleJump = false;
					}
				}
			}
		}
	}
}
