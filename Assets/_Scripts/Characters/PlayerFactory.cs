using UnityEngine;
using System.Collections;

public class PlayerFactory {

	public static GameObject CreatePlayer(string playerConstString, int playerID){

		GameObject playerObject = new GameObject ();
		Rigidbody2D rb2D;
		Player player;
		Animator anim;
		string animatorName = "";
		SpriteRenderer spr;
		BoxCollider2D boxCollider;


		//* Placeholder art

		Texture2D phSprite = Resources.Load ("placeholderPlayer") as Texture2D;
		spr = playerObject.AddComponent<SpriteRenderer> ();
		spr.sprite = Sprite.Create(phSprite,new Rect(0,0,phSprite.width,phSprite.height),new Vector2(0.5f,0.5f));

		// ---

		rb2D = playerObject.AddComponent<Rigidbody2D> ();
		rb2D.gravityScale = 2.5f;
		rb2D.freezeRotation = true;

		anim = playerObject.AddComponent<Animator> ();
		anim.speed = 5;

		boxCollider = playerObject.AddComponent<BoxCollider2D> ();

		boxCollider.offset = new Vector2 (-0.01f, 0.94f);
		boxCollider.size = new Vector2 (0.74f, 1.86f);

		player = playerObject.AddComponent<Player> ();

		player.playerType = playerConstString;
		player.playerID = playerID;


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

		PlayerArrow newPlayerArrow = playerObject.AddComponent<PlayerArrow>();
		newPlayerArrow.Init();

		newPlayerArrow.SetColor (CharDB.GetColorByID (playerID));

		anim.runtimeAnimatorController = Resources.Load ("Animators/Players/" + animatorName) as RuntimeAnimatorController;

		player.SetCharacter (playerConstString);
		return playerObject;
	}
}
