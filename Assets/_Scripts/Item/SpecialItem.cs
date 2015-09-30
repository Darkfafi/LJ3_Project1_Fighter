using UnityEngine;
using System.Collections;

public class SpecialItem : MonoBehaviour {
	public delegate void NormDelegate();
	public event NormDelegate StartBall;

	public PhysicsMaterial2D bouncyMaterial;

	private int _hitsTillBreak;
	private float _timeTillDestroy;

	private SpriteRenderer _sprtRenderer;

	// Use this for initialization
	void Start () {
		GetComponent<AttackCather>().OnStunAttackCatch += GetHit;
		_timeTillDestroy = 25f;
		_hitsTillBreak = Random.Range(1,4);
		StartBall += AddCollider;
		_sprtRenderer = gameObject.GetComponent<SpriteRenderer> ();
		_sprtRenderer.color = new Color (0.8f, 0.8f, 0.8f, 0.5f);
		Invoke("Begin", 3f);
	}
	void Begin()
	{
		StartBall();
		_sprtRenderer.color = GetColorByHitsLeft (_hitsTillBreak);
		Destroy(this.gameObject, _timeTillDestroy);
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
		_sprtRenderer.color = GetColorByHitsLeft (_hitsTillBreak);
	}

	Color GetColorByHitsLeft(int hitsLeft){
		Color colorToGive = Color.white;
		switch (hitsLeft) {
		case 3:
			colorToGive = Color.green;
			break;
		case 2:
			colorToGive = Color.yellow;
			break;
		case 1:
			colorToGive = Color.red;
			break;
		}
		return colorToGive;
	}
}
