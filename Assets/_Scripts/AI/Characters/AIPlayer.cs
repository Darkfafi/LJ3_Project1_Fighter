﻿using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour {

	//Level knowledge
	private AISystemManager _AISystem;

	//Pathfinding
	private Grid _grid;

	// Movement
	private AIMovement _movement;

	// Use this for initialization
	void Start () {

		_AISystem = GameObject.FindGameObjectWithTag (Tags.GAMECONTROLLER).GetComponent<AISystemManager> ();

		_grid = new Grid (_AISystem.prefabGridList);

		_movement = gameObject.AddComponent<AIMovement> ();
		_movement.SetLevelGrid (_grid);
	}


	public Grid grid{
		get{return _grid;}
	}
	public AISystemManager AISystem{
		get{return _AISystem;}
	}
}