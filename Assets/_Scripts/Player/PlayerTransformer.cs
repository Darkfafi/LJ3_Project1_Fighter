using UnityEngine;
using System.Collections;

public class PlayerTransformer : MonoBehaviour {

	public const string SPECIAL_MOD = "SuperSaiyajinGoderu!";
	public const string NORMAL_MOD = "YouAreNoRealSuperSand";

	public StatsHolder transformStats;

	private Player _player;

	void Start(){
		_player = gameObject.GetComponent<Player> ();
	}

	public void TransformCharacter(string transformModConst){
		if(transformModConst == SPECIAL_MOD){
			//TODO call global transform special animation.
			_player.playerStats.ModCharacter(true,transformStats.movementSpeed,
			                                 transformStats.jumpForce,
			                                 transformStats.fallSpeed,
			                                 transformStats.stunPower,
			                                 transformStats.pushPower,
			                                 transformStats.dashForce);
		}else if(transformModConst == NORMAL_MOD){
			//TODO call global transform back animation.
			_player.playerStats.ModCharacter(false,0f,0f,0f,0f,0f,0f);
		}
	}
}
