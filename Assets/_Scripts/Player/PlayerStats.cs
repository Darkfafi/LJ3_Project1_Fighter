using UnityEngine;
using System.Collections;

public class PlayerStats {

	// Transformed or Not?
	private bool _transformed = false;

	// Base stats (For every player the same)
	private float _movementSpeed;
	private float _jumpForce;
	private float _fallSpeed;
	private float _stunPower;
	private float _pushPower;
	private float _dashForce;

	//Mod stats (Change on transformation. For every player different) 
	private float _modMovementSpeed = 0f;
	private float _modJumpForce = 0f;
	private float _modFallSpeed = 0f;
	private float _modStunPower = 0f;
	private float _modPushPower = 0f;
	private float _modDashForce = 0f;

	public PlayerStats(float baseMovementSpeed, float baseJumpForce, float baseFallSpeed, float baseStunPower, float basePushPower,float baseDashForce){
		_movementSpeed = baseMovementSpeed;
		_jumpForce = baseJumpForce;
		_fallSpeed = baseFallSpeed;
		_stunPower = baseStunPower;
		_pushPower = basePushPower;
		_dashForce = baseDashForce;
	}

	public void ModCharacter(bool transformed,float moveSpeedMod, float jumpForceMod, float fallSpeedMod, float stunPowerMod, float pushPowerMod, float dashForceMod){
		_modMovementSpeed = moveSpeedMod;
		_modJumpForce = jumpForceMod;
		_modFallSpeed = fallSpeedMod;
		_modStunPower = stunPowerMod;
		_modPushPower = pushPowerMod;
		_modDashForce = dashForceMod;
		_transformed = transformed;
	}

	public float movementSpeed{
		get{return _movementSpeed + _modMovementSpeed;}
	}
	public float jumpForce{
		get{return _jumpForce + _modJumpForce;}
	}
	public float fallSpeed{
		get{return _fallSpeed + _modFallSpeed;}
	}
	public float stunPower{
		get{return _stunPower + _modStunPower;}
	}
	public float pushPower{
		get{return _pushPower + _modPushPower;}
	}
	public float dashForce{
		get{return _dashForce + _modDashForce;}
	}
	public bool transformed{
		get{return _transformed;}
	}

}
