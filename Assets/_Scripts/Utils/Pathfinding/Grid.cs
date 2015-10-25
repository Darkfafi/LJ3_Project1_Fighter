using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid {

	private List<List<Cell>> _grid;
	private List<Cell> _allCells;
	private int _width;
	private int _height;

	public Grid(List<List<GameObject>> prefabGridList){
		Cell currentCell;
		CellPrefabInfo currentCellInfo;

		_grid = new List<List<Cell>> ();
		_allCells = new List<Cell> ();
		for (int xRow = 0; xRow < prefabGridList.Count; xRow++) {

			_grid.Add(new List<Cell>());

			for(int yRow = 0; yRow < prefabGridList[xRow].Count; yRow++){
				currentCellInfo = prefabGridList[xRow][yRow].GetComponent<CellPrefabInfo>();
				currentCell = new Cell(xRow,yRow);

				currentCell.worldPosition = new Vector2(currentCellInfo.gameObject.transform.position.x,currentCellInfo.gameObject.transform.position.y);

				currentCell.isBlocked = currentCellInfo.isBlocked;
				currentCell.isGround = currentCellInfo.isGround;
				currentCell.isPassableGround = currentCellInfo.isPassableGround;
				currentCell.isWall = currentCellInfo.isWall;

				currentCell.infoCell = currentCellInfo;

				currentCell.cellSize = currentCellInfo.cellSize;
				currentCellInfo.linkedCells.Add(currentCell); // for if it happens to change into a wall then all AIs will be updated.

				_grid[xRow].Add(currentCell);
				_allCells.Add(currentCell);
			}
		}
	}

	public Cell GetCell(int x, int y){
		Cell cellToReturn = null;
		if(x >= 0 && y >= 0 && x < _grid.Count && y < _grid[x].Count){
			if (_grid[x] != null && _grid [x][y] != null) {
				cellToReturn = _grid[x][y];
			}
		}

		return cellToReturn;
	}

	public List<Cell> allCells{
		get{ return _allCells;}
	}

	public void Reset(){
		Cell currentCell;
		int l = _grid.Count;

		for (int xRow = 0; xRow < l; xRow++) {
			for(int yRow = 0; yRow < _grid[xRow].Count; yRow++){
				currentCell = _grid[xRow][yRow];
				currentCell.f = 0;
				currentCell.g = 0;
				currentCell.h = 0;
				currentCell.j = 0;
				currentCell.isClosed = false;
				currentCell.isOpen = false;
				currentCell.parent = null;
				if(currentCell.tempGroundTile){
					currentCell.isGround = false;
					currentCell.isWall = true;
				}
			}
		}
	}

	public Vector2 WorldPosToCellPos(Vector2 position){
		Vector2 vector = new Vector2(0,0);
		for(int i = 0; i < _allCells.Count; i++) {
			Cell cell = _allCells[i];
			if(cell.worldPosition.x - (cell.cellSize.x / 2) < position.x 
			   && cell.worldPosition.x + (cell.cellSize.x / 2) > position.x 
			   && cell.worldPosition.y - (cell.cellSize.y / 2) < position.y 
			   && cell.worldPosition.y + (cell.cellSize.y / 2) > position.y)
			{
				vector = cell.position;
				break;
			}
		}
		return vector;
	}

	public bool CellAboveOrSelfGround(Cell cell, int maxHeight = 0){
		bool result = false;
		int xRow = (int)cell.position.x;
		Cell currentCellCheck;

		if (maxHeight > 0 && maxHeight < (int)cell.position.y) {
			maxHeight = (int)cell.position.y - maxHeight;
		} else {
			maxHeight = 0;
		}


		if (cell.isGround || cell.isPassableGround || cell.isWall) {
			result = true;
		} else {
			for(int yRow = (int)cell.position.y; yRow > maxHeight; yRow--){
				currentCellCheck = _grid[xRow][yRow];
				if(currentCellCheck.isGround || currentCellCheck.isPassableGround){ //TODO iswall moet er nog bij (met fix)
					//Debug.Log("yes " + currentCellCheck.position +" < normal | world > "+ currentCellCheck.worldPosition);
					//currentCellCheck.infoCell.GetComponent<SpriteRenderer>().color = Color.yellow;

					result = true;
					break;
				}
			}
		}

		return result;
	}
}
