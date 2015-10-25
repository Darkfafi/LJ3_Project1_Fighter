using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour {

	private Player _player;
	private GameObject _target;


	//Level knowledge
	private AISystemManager _AISystem;

	//Pathfinding
	private Grid _grid;

	// Movement
	private AIMovement _movement;
	private AICombat _combat;
	private AITargetPrioritizer _targetLocator;

	// Use this for initialization
	void Start () {

		_player = GetComponent<Player> ();

		_AISystem = GameObject.FindGameObjectWithTag (Tags.GAMECONTROLLER).GetComponent<AISystemManager> ();

		_grid = new Grid (_AISystem.prefabGridList);

		_movement = gameObject.AddComponent<AIMovement> ();
		_combat = gameObject.AddComponent<AICombat> ();
		_targetLocator = gameObject.AddComponent<AITargetPrioritizer> ();

		_movement.SetLevelGrid (_grid);
	}


	public Grid grid{
		get{return _grid;}
	}
	public AISystemManager AISystem{
		get{return _AISystem;}
	}

	public Player player{
		get{return _player;}
	}

	public GameObject target{
		get{return _target;}
		set{_target = value;}
	}
}
