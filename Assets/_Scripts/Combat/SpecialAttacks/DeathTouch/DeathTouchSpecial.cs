using UnityEngine;
using System.Collections;

public class DeathTouchSpecial : SpecialAttack {

	protected override void OnAttack (Player player)
	{
		RaycastHit2D hit;

		int dir = (int)(Mathf.Abs(gameObject.transform.localScale.x) / gameObject.transform.localScale.x);

		float dist = gameObject.GetComponent<SpriteRenderer> ().bounds.size.x / 2;
		dist = dist + (dist * 0.1f);

		Vector2 startRay = new Vector2 (transform.position.x + dist, transform.position.y); 

		hit = Physics2D.Raycast (startRay,new Vector2(dir,0),0.1f);

		if (hit.collider != null && hit.collider.gameObject.GetComponent<AttackCather> () != null) {
			hit.collider.gameObject.GetComponent<AttackCather> ().CatchAttack (this.gameObject,STUN_POWER_KILL, 0);
		} else {
			EndTransform();
			player.GetComponent<AttackCather>().CatchAttack(player.gameObject,30f,0f);
		}

		base.OnAttack (player);
	}
}
