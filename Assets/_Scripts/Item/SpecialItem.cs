using UnityEngine;
using System.Collections;

public class SpecialItem : MonoBehaviour {
	public delegate void NormDelegate();
	public event NormDelegate StartBall;

	public PhysicsMaterial2D bouncyMaterial;

	private float _hitsTillBreak;
	// Use this for initialization
	void Start () {
		GetComponent<AttackCather>().OnStunAttackCatch += GetHit;
		_hitsTillBreak = Random.Range(1,4);
		StartBall += AddCollider;
		Invoke("Begin", 3f);
	}
	void Begin()
	{
		StartBall();
	}
	void AddCollider()
	{
		Collider2D col = gameObject.AddComponent<CircleCollider2D>();
		col.sharedMaterial = bouncyMaterial;

	}
	void GetHit (float stunPower, GameObject objHitBy, float pushPower) {
		_hitsTillBreak--;
		if(_hitsTillBreak <= 0)
		{
			objHitBy.GetComponent<Player>().TransformPlayer(PlayerTransformer.SPECIAL_MOD);
			Destroy(this.gameObject);
		}
	}
}
