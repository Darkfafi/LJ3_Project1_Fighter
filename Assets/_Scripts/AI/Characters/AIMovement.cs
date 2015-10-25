using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour {

	private AIPlayer _aiPlayer;
	private PlatformerMovement _platformMovement;

	public Vector2 moveTarget;
	public bool canDubbleJump = true;

	// Waypint System
	private Grid _grid;
	private List<Cell> _waypoints = new List<Cell> ();

	Vector2 vecSelf;

	void Awake(){
		moveTarget = new Vector2(transform.position.x,transform.position.y);
		_aiPlayer = GetComponent<AIPlayer> ();
		_platformMovement = gameObject.GetComponent<PlatformerMovement> ();
	}

	// Update is called once per frame
	void Update () {
		if (_waypoints.Count > 0) {
			Movement ();
		} else {
			_aiPlayer.player.OnNoKeyPressed();
		}
	}

	public void SetLevelGrid(Grid grid){
		_grid = grid;
		//_target = GameObject.FindGameObjectWithTag (Tags.PLAYER);
		SetWaypoints ();
	}

	private void Movement(){

		vecSelf = _grid.WorldPosToCellPos (new Vector2 (transform.position.x, transform.position.y));

		Cell currentWaypointCell = _waypoints [0];

		if (currentWaypointCell.worldPosition.x < transform.position.x - 0.05f) {
			_aiPlayer.player.MoveLeft();
		} else if (currentWaypointCell.worldPosition.x > transform.position.x + 0.05f) {
			_aiPlayer.player.MoveRight();
		}
		if (currentWaypointCell.worldPosition.y > transform.position.y + currentWaypointCell.cellSize.y / 2 && ((currentWaypointCell.j == 2 || _platformMovement.inWallSlide) || (currentWaypointCell.j >= 8 && canDubbleJump))) {
			//_platformMovement.Jump(10);
			_aiPlayer.player.Jump();
		}

		if (currentWaypointCell.worldPosition.y < transform.position.y - currentWaypointCell.cellSize.x / 2) {
			Vector2 vec = _grid.WorldPosToCellPos(new Vector2(transform.position.x,transform.position.y));
			Cell inCell = _grid.GetCell((int)vec.x,(int)vec.y);
			if(inCell.isPassableGround || _platformMovement.inWallSlide){
				_aiPlayer.player.FallDown();
			}
		}

		CheckDeleteWaypoint (currentWaypointCell);
	}

	private void CheckDeleteWaypoint(Cell currentWaypointCell){
		/*
		if (Mathf.Abs (currentWaypointCell.worldPosition.x - transform.position.x) + Mathf.Abs (currentWaypointCell.worldPosition.y - transform.position.y) < currentWaypointCell.cellSize.x
		    || ((currentWaypointCell.parent != null))
		    && (currentWaypointCell.parent.position.y < currentWaypointCell.position.y && vecSelf.y > currentWaypointCell.position.y  // if the road goes down and you are already a far way down
		    || currentWaypointCell.parent.position.y > currentWaypointCell.position.y && vecSelf.y < currentWaypointCell.position.y) // if the road goed up and you are way up!
		    ) 
		{
			currentWaypointCell.infoCell.DebugColor(false);
			_waypoints.Remove(currentWaypointCell);
		}else if(currentWaypointCell.j > 0 
		         && currentWaypointCell.worldPosition.y < transform.position.y + currentWaypointCell.cellSize.y * 2
		         && Mathf.Abs(currentWaypointCell.worldPosition.x - transform.position.x) < currentWaypointCell.cellSize.x){
			//Mathf.Abs (currentWaypointCell.worldPosition.x - transform.position.x) +  Mathf.Abs (currentWaypointCell.worldPosition.y - transform.position.y) < currentWaypointCell.cellSize.x * 3
			currentWaypointCell.infoCell.DebugColor(false);
			_waypoints.Remove(currentWaypointCell);
		}*/

		//Debug.Log (currentWaypointCell.parent.position.y + " p  c" + currentWaypointCell.position.y);

		float distToNextCell = Mathf.Abs (currentWaypointCell.worldPosition.x - transform.position.x) + Mathf.Abs (currentWaypointCell.worldPosition.y - transform.position.y);

		if (distToNextCell > currentWaypointCell.cellSize.x * 5 || currentWaypointCell.j == 0 && (distToNextCell < currentWaypointCell.cellSize.x) ||
		    (currentWaypointCell.j > 0 && (distToNextCell < currentWaypointCell.cellSize.x * 2)) ||
			currentWaypointCell.parent != null && 
		    ((currentWaypointCell.parent.position.y >= currentWaypointCell.position.y && currentWaypointCell.worldPosition.y > transform.position.y && Mathf.Abs(currentWaypointCell.worldPosition.y - transform.position.y) > currentWaypointCell.cellSize.x ) || 
		 (currentWaypointCell.parent.position.y <= currentWaypointCell.position.y && currentWaypointCell.worldPosition.y < transform.position.y && Mathf.Abs(currentWaypointCell.worldPosition.y - transform.position.y) > currentWaypointCell.cellSize.x))) {

			currentWaypointCell.infoCell.DebugColor(false);
			_waypoints.Remove(currentWaypointCell);
		}

	}

	private void SetWaypoints(){
		foreach(Cell cell in _waypoints){
			cell.infoCell.DebugColor(false);
		}

		Vector2 vecTarget = _grid.WorldPosToCellPos (moveTarget);
		Vector2 vecSelf = _grid.WorldPosToCellPos (new Vector2 (transform.position.x, transform.position.y));  //&& _grid.CellAboveOrSelfGround(_grid.GetCell((int)vecSelf.x,(int)vecSelf.y));
		//Debug.Log (_grid.CellAboveOrSelfGround (_grid.GetCell ((int)vecTarget.x, (int)vecTarget.y), 7) && (_platformMovement.onGround || _platformMovement.inWallSlide));
		if (_grid.CellAboveOrSelfGround (_grid.GetCell((int)vecTarget.x,(int)vecTarget.y),7) && (_platformMovement.onGround || _grid.GetCell((int)vecSelf.x,(int)vecSelf.y).isWall)) {
			_waypoints = AStar.Search (_grid, _grid.WorldPosToCellPos (transform.position), _grid.WorldPosToCellPos (moveTarget));
		}
		foreach(Cell cell in _waypoints){
			cell.infoCell.DebugColor(true);
		}

		Invoke ("SetWaypoints", 0.6f);
	}
}
