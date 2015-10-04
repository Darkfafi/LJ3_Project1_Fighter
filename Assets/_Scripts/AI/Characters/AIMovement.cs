using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour {

	private Grid _grid;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetLevelGrid(Grid grid){
		_grid = grid;
		Debug.Log (AStar.Search (_grid, _grid.WorldPosToCellPos (transform.position), _grid.GetCell (2, 2).position).Count);
	}
}
