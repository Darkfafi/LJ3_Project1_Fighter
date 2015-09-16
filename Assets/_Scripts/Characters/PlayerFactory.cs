using UnityEngine;
using System.Collections;

public class PlayerFactory {

	public static GameObject CreatePlayer(string playerConstString){

		GameObject playerObject = new GameObject ();
		Rigidbody2D rb2D;
		Player player;
		Animator anim;
		string animatorName = "";
		SpriteRenderer spr;


		//* Placeholder art

		Texture2D phSprite = Resources.Load ("placeholderPlayer") as Texture2D;
		spr = playerObject.AddComponent<SpriteRenderer> ();
		spr.sprite = Sprite.Create(phSprite,new Rect(0,0,phSprite.width,phSprite.height),new Vector2(0.5f,0.5f));

		// ---

		rb2D = playerObject.AddComponent<Rigidbody2D> ();
		rb2D.gravityScale = 2.5f;
		rb2D.freezeRotation = true;

		anim = playerObject.AddComponent<Animator> ();

		playerObject.AddComponent<BoxCollider2D> ();

		player = playerObject.AddComponent<Player> ();


		// set animators and special attacks for characters
		switch(playerConstString)
		{
		case CharDB.CHARACTER01:
			animatorName = "BirdAnimator";
			playerObject.AddComponent<ThrowRangedSpecialAttack>();
			break;
		case CharDB.CHARACTER02:
			animatorName = "BirdAnimator";
			playerObject.AddComponent<SpecialBasicAttack>();
			break;
		case CharDB.CHARACTER03:
			animatorName = "BirdAnimator";
			playerObject.AddComponent<ThrowRangedSpecialAttack>();
			break;
		case CharDB.CHARACTER04:
			animatorName = "BirdAnimator";
			playerObject.AddComponent<DeathTouchSpecial>();
			break;
		}

		anim.runtimeAnimatorController = Resources.Load ("Animators/Players/" + animatorName) as RuntimeAnimatorController;

		player.SetCharacter (playerConstString);

		return playerObject;
	}
}
