using UnityEngine;
using System.Collections;

public class AICombat : MonoBehaviour {

	private Player _player;
	private AIPlayer _aiPlayer;
	private AIMovement _aiMovement;
	private PlatformerMovement _movement;

	void Awake(){
		_player = GetComponent<Player> ();
		_aiPlayer = GetComponent<AIPlayer> ();
		_aiMovement = GetComponent<AIMovement> ();
		_movement = GetComponent<PlatformerMovement> ();
	}

	// Update is called once per frame
	void Update () {
		Combat ();
	}

	void Combat(){
		GameObject target = _aiPlayer.target;
		Player playerTarget = null;
		bool attackTarget = true;
		float attackDist = 3;

		if (_player.playerStats.transformed) {
			attackDist = _player.specialAttack.rangeAttack;
		}

		if (!_aiMovement.canDubbleJump) {
			_aiMovement.canDubbleJump = true;
		}
		if (target != null && target != gameObject) {

			_aiMovement.moveTarget = new Vector2 (target.transform.position.x, target.transform.position.y);

			if(target.GetComponent<Player>() != null){
				playerTarget = target.GetComponent<Player>();

				if(playerTarget.CheckIfInBusyAction(Player.IN_STUNNED)){
					if(playerTarget.GetComponent<Rigidbody2D>().velocity.x < 0.1f){
						_aiMovement.moveTarget = new Vector2(target.transform.position.x, target.transform.position.y + 2.3f);
						attackTarget = false;
					}
				}
			}

			if(attackTarget){
				float dist = Mathf.Abs((target.transform.position - transform.position).magnitude);
				if(dist <= attackDist){ // dist < x vervangen met attack distance die ik uit de current attack ga halen.
					Vector2 dir;

					if(Mathf.Abs(transform.localScale.x) == transform.localScale.x){
						dir = Vector2.right;
					}else{
						dir = Vector2.left;
					}

					RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + (Mathf.Abs(transform.localScale.x) / transform.localScale.x) * 0.75f,transform.position.y + 1.5f),dir,attackDist);

					if(attackTarget && !_movement.inWallSlide && hit.collider != null && hit.collider == attackTarget){
						_player.DoAction();
					}else if(playerTarget != null && playerTarget.CheckIfInBusyAction(Player.IN_STUNNED)){
						_aiMovement.canDubbleJump = false;
					}
				}
			}
		}
	}
}
