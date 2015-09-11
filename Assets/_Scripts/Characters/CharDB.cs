﻿using UnityEngine;
using System.Collections;

public class CharDB : MonoBehaviour {
	public const string CHARACTER01 = "Character01";
	public const string CHARACTER02 = "Character02";
	public const string CHARACTER03 = "Character03";
	public const string CHARACTER04 = "Character04";

	public static CharDBInfo GetCharacterDataBaseInfo(string character)
	{
		StatsHolder transformModStats = GetCharacterTransformStats(character);
		Animator characterAnimator = null;
		SpecialAttack specialAttack = null;
		CharDBInfo charDBInfo;

		switch(character)
		{
		case CHARACTER01:
			characterAnimator = null;
			specialAttack = null;
			break;
		case CHARACTER02:
			characterAnimator = null;
			specialAttack = null;
			break;
		case CHARACTER03:
			characterAnimator = null;
			specialAttack = null;
			break;
		case CHARACTER04:
			characterAnimator = null;
			specialAttack = null;
			break;
		}

		charDBInfo = new CharDBInfo(characterAnimator,transformModStats,specialAttack);
		return charDBInfo;
	}
	// Via deze functie kan je makkelijk de stats per character invullen.
	private static StatsHolder GetCharacterTransformStats(string character){
		StatsHolder stats = new StatsHolder ();
		switch(character)
		{
		case CHARACTER01:
			stats.movementSpeed = 0f;
			stats.jumpForce = 0f;
			stats.fallSpeed = 0f;
			stats.stunPower = 0f;
			stats.pushPower = 0f;
			stats.dashForce = 0f;
			break;
		case CHARACTER02:
			stats.movementSpeed = 0f;
			stats.jumpForce = 0f;
			stats.fallSpeed = 0f;
			stats.stunPower = 0f;
			stats.pushPower = 0f;
			stats.dashForce = 0f;
			break;
		case CHARACTER03:
			stats.movementSpeed = 0f;
			stats.jumpForce = 0f;
			stats.fallSpeed = 0f;
			stats.stunPower = 0f;
			stats.pushPower = 0f;
			stats.dashForce = 0f;
			break;
		case CHARACTER04:
			stats.movementSpeed = 0f;
			stats.jumpForce = 0f;
			stats.fallSpeed = 0f;
			stats.stunPower = 0f;
			stats.pushPower = 0f;
			stats.dashForce = 0f;
			break;
		}
		return stats;
	}
	public static string GetCharacterByInt(int i)
	{
		switch(i)
		{
		case 0:
			return CHARACTER01;
		case 1:
			return CHARACTER02;
		case 2:
			return CHARACTER03;
		case 3:
			return CHARACTER04;
		}
		return "";
	}
}
