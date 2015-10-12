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

		if (currentWaypointCell.worldPosition.x < transform.position.x - 0.05f) {
			_platformMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT,5);
		} else if (currentWaypointCell.worldPosition.x > transform.position.x + 0.05f) {
			_platformMovement.MoveHorizontal(PlatformerMovement.DIR_RIGHT,5);
		}
		if (currentWaypointCell.worldPosition.y > transform.position.y + 0.2f && (currentWaypointCell.j == 2 || currentWaypointCell.j >= 9)) {
			_platformMovement.Jump(10);
		}

		if (currentWaypointCell.worldPosition.y < transform.position.y - 0.2f) {
			Vector2 vec = _grid.WorldPosToCellPos(new Vector2(transform.position.x,transform.position.y));
			if(_grid.GetCell((int)vec.x,(int)vec.y).isPassableGround){
				_platformMovement.MoveVertical(PlatformerMovement.DIR_DOWN,5f);
			}
		}

		if (Mathf.Abs (currentWaypointCell.worldPosition.x - transform.position.x) +  Mathf.Abs (currentWaypointCell.worldPosition.y - transform.position.y) < currentWaypointCell.cellSize.x) {
			currentWaypointCell.infoCell.DebugColor(false);
			_waypoints.Remove(currentWaypointCell);
		}else if(currentWaypointCell.j > 0 && Mathf.Abs (currentWaypointCell.worldPosition.x - transform.position.x) +  Mathf.Abs (currentWaypointCell.worldPosition.y - transform.position.y) < currentWaypointCell.cellSize.x * 3){
			currentWaypointCell.infoCell.DebugColor(false);
			_waypoints.Remove(currentWaypointCell);
		}
	}

	private void SetWaypoints(){
		foreach(Cell cell in _waypoints){
			cell.infoCell.DebugColor(false);
		}

		Vector2 vecTarget = _grid.WorldPosToCellPos (new Vector2 (_target.transform.position.x, _target.transform.position.y));
		Vector2 vecSelf = _grid.WorldPosToCellPos (new Vector2 (transform.position.x, transform.position.y));

		if (_grid.CellAboveOrSelfGround (_grid.GetCell((int)vecTarget.x,(int)vecTarget.y)) && _grid.CellAboveOrSelfGround(_grid.GetCell((int)vecSelf.x,(int)vecSelf.y))) {
			_waypoints = AStar.Search (_grid, _grid.WorldPosToCellPos (transform.position), _grid.WorldPosToCellPos (new Vector3 (_target.transform.position.x, _target.transform.position.y, _target.transform.position.z)));
		}
		foreach(Cell cell in _waypoints){
			cell.infoCell.DebugColor(true);
		}

		Invoke ("SetWaypoints", 0.8f);
	}
}
