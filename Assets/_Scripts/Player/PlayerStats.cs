using UnityEngine;
using System.Collections;

public class PlayerStats {

	// Transformed or Not?
	private bool _transformed = false;

	// Base stats (For every player the same)
	private StatsHolder _baseStats = new StatsHolder ();

	//Mod stats (Change on transformation. For every player different) 
	private StatsHolder _modStats = new StatsHolder();

	public PlayerStats(float baseMovementSpeed, float baseJumpForce, float baseFallSpeed, float baseStunPower, float basePushPower,float baseDashForce){
		_baseStats.movementSpeed = baseMovementSpeed;
		_baseStats.jumpForce = baseJumpForce;
		_baseStats.fallSpeed = baseFallSpeed;
		_baseStats.stunPower = baseStunPower;
		_baseStats.pushPower = basePushPower;
		_baseStats.dashForce = baseDashForce;
	}

	public void ModCharacter(bool transformed,float moveSpeedMod, float jumpForceMod, float fallSpeedMod, float stunPowerMod, float pushPowerMod, float dashForceMod){
		_modStats.movementSpeed = moveSpeedMod;
		_modStats.jumpForce = jumpForceMod;
		_modStats.fallSpeed = fallSpeedMod;
		_modStats.stunPower = stunPowerMod;
		_modStats.pushPower = pushPowerMod;
		_modStats.dashForce = dashForceMod;
		_transformed = transformed;
	}

	public float movementSpeed{
		get{return _baseStats.movementSpeed + _modStats.movementSpeed;}
	}
	public float jumpForce{
		get{return _baseStats.jumpForce + _modStats.jumpForce;}
	}
	public float fallSpeed{
		get{return _baseStats.fallSpeed + _modStats.fallSpeed;}
	}
	public float stunPower{
		get{return _baseStats.stunPower + _modStats.stunPower;}
	}
	public float pushPower{
		get{return _baseStats.pushPower + _modStats.pushPower;}
	}
	public float dashForce{
		get{return _baseStats.dashForce + _modStats.dashForce;}
	}
	public bool transformed{
		get{return _transformed;}
	}

}
