using UnityEngine;
using System.Collections;

public class PlayerTransformer : MonoBehaviour {

	public const string SPECIAL_MOD = "SuperSaiyajinGoderu!";
	public const string NORMAL_MOD = "YouAreNoRealSuperSand";


	// de transform time logica staat in de attack voor het geval je verschillende specials  had dan zou de ene korter duren dan de ander. <-- vorige idee..
	protected float _timeInTransformation = 10;
	protected ComTimer _transformBackTimer;


	public StatsHolder transformStats;

	//private Player _player;

	void Awake(){
		_transformBackTimer = gameObject.AddComponent<ComTimer> ();
		_transformBackTimer.TimerEnded += EndTransformTime;
	}
	

	public void TransformCharacter(Player player,string transformModConst){
		if(transformModConst == SPECIAL_MOD){
			//TODO call global transform special animation.
			player.playerStats.ModCharacter(true,transformStats.movementSpeed,
			                                 transformStats.jumpForce,
			                                 transformStats.fallSpeed,
			                                 transformStats.stunPower,
			                                 transformStats.pushPower,
			                                 transformStats.dashForce);
			_transformBackTimer.StartTimer (_timeInTransformation);
		}else if(transformModConst == NORMAL_MOD){
			//TODO call global transform back animation.
			player.playerStats.ModCharacter(false,0f,0f,0f,0f,0f,0f);
			_transformBackTimer.StopTimer();
		}
	}

	void EndTransformTime(){
		TransformCharacter (gameObject.GetComponent<Player> (), NORMAL_MOD);
	}
}
