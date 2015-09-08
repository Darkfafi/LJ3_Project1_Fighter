using UnityEngine;
using System.Collections;

public class CharDB : MonoBehaviour {
	public const string CHARACTER01 = "Character01";
	public const string CHARACTER02 = "Character02";
	public const string CHARACTER03 = "Character03";
	public const string CHARACTER04 = "Character04";

	public static Animator GetCharacterAnimator(string character)
	{
		switch(character)
		{
		case CHARACTER01:
			//TODO: return character1 Animator.
			break;
		case CHARACTER02:
			//TODO: return character2 Animator.
			break;
		case CHARACTER03:
			//TODO: return character3 Animator.
			break;
		case CHARACTER04:
			//TODO: return character4 Animator.
			break;
		}
		return null;
	}
}
