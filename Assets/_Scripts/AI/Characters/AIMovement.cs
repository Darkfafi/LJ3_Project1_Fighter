using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour {

	private PlatformerMovement _platformMovement;

	// Waypint System
	private Grid _grid;
	private GameObject _target;
	private List<Cell> _waypoints = new List<Cell> ();

	void Awake(){
		_platformMovement = gameObject.GetComponent<PlatformerMovement> ();
	}

	// Update is called once per frame
	void Update () {
		if (_waypoints.Count > 0) {
			Movement();
		}
	}

	public void SetLevelGrid(Grid grid){
		_grid = grid;
		_target = GameObject.FindGameObjectWithTag (Tags.PLAYER);
		SetWaypoints ();
	}

	private void Movement(){
		Cell currentWaypointCell = _waypoints [0];

		if (currentWaypointCell.worldPosition.x < transform.position.x) {
			_platformMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT,4);
		} else if (currentWaypointCell.worldPosition.x > transform.position.x) {
			_platformMovement.MoveHorizontal(PlatformerMovement.DIR_RIGHT,4);
		}

		if (currentWaypointCell.worldPosition.y > transform.position.y + 0.2f) {
			_platformMovement.Jump(11);
		}

		if (currentWaypointCell.worldPosition.y < transform.position.y - 0.2f) {
			_platformMovement.MoveVertical(PlatformerMovement.DIR_DOWN,4f);
		}

		if (Mathf.Abs (currentWaypointCell.worldPosition.x - transform.position.x) +  Mathf.Abs (currentWaypointCell.worldPosition.y - transform.position.y)< 1) {
			currentWaypointCell.infoCell.DebugColor(false);
			_waypoints.Remove(currentWaypointCell);
		}
	}

	private void SetWaypoints(){
		foreach(Cell cell in _waypoints){
			cell.infoCell.DebugColor(false);
		}

		_waypoints = AStar.Search (_grid, _grid.WorldPosToCellPos (transform.position), _grid.WorldPosToCellPos(new Vector3(_target.transform.position.x,_target.transform.position.y,_target.transform.position.z)));

		foreach(Cell cell in _waypoints){
			cell.infoCell.DebugColor(true);
		}

		Invoke ("SetWaypoints", 2f);
	}
}
