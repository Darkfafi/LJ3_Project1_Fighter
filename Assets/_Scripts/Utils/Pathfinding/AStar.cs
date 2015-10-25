using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar{

	public static int horizontalScore = 1;
	public static int diagonalScore = 3;

	public static List<Cell>  Search(Grid grid, Vector2 start, Vector2 end){

		grid.Reset ();

		List<Cell> openList = new List<Cell> ();
		List<Cell> closedList = new List<Cell> ();
		Cell currentCell;
		Cell neighbor;
		float gScore;
		bool gScoreIsBest;
		List<Cell> neighbors;

		int maxJumpHeight = 6;
		int maxDubbleJumpHeight = 2;

		int currentMaxJumpHeight = maxJumpHeight;

		currentCell = grid.GetCell ((int)start.x, (int)start.y);

		if (currentCell.isWall) {
			currentCell.isGround = true;
			currentCell.isWall = false;
			currentCell.tempGroundTile = true;
		}

		openList.Add (currentCell);

		while (openList.Count > 0) {
			openList.Sort(SortOnF);

			currentCell = openList[0];

			if(currentCell.position.Equals(end)){
				List<Cell> path = new List<Cell>();

				while(currentCell.parent != null){
					path.Add(currentCell);
					currentCell = currentCell.parent;
				}

				path.Reverse();

				return path;
			}

			openList.Remove(currentCell);
			closedList.Add(currentCell);
			currentCell.isClosed = true;
			currentCell.isOpen = false;

			if(currentCell.neighbors == null){
				currentCell.neighbors = GetNeighbors(grid,currentCell);
			}

			if(currentCell.j >= maxJumpHeight && currentMaxJumpHeight == maxJumpHeight){
				currentMaxJumpHeight += maxDubbleJumpHeight;
			}

			neighbors = currentCell.neighbors;

			int l = neighbors.Count;

			for(int i = 0; i < l; i++){
				neighbor = neighbors[i];

				if(neighbor.isClosed || currentCell.isBlocked){
					continue; // ignore cell
				}

				if(IsDiagonal(currentCell,neighbor)){
					gScore = currentCell.g + diagonalScore;
				}else{
					gScore = currentCell.g + horizontalScore;
				} 


				if(currentCell.j > 0 && !neighbor.isGround && !neighbor.isPassableGround && !neighbor.isWall){
					if(GetNonDiagonalDirection(currentCell,neighbor) != Vector2.left && GetNonDiagonalDirection(currentCell,neighbor) != Vector2.right){
						neighbor.j = currentCell.j + 1;
					}else{
						neighbor.j = currentCell.j + 0.5f;
					}
				}else{
					neighbor.j = 0;
					neighbor.th = 0;
				}

				if(GetNonDiagonalDirection(currentCell,neighbor) == Vector2.up){
					//neighbor.infoCell.GetComponent<SpriteRenderer>().color = Color.cyan;
					if(currentCell.j == 0){
						neighbor.j = 2;
					}else if(currentCell.j % 2 != 0){
						neighbor.j = currentCell.j + 1;
					}

					/*else{
						neighbor.j = currentCell.j + 1;
					}*/

					if(neighbor.j > currentMaxJumpHeight){ //TODO calculated max jump height in tiles (pixels from jump height)
						//neighbor.infoCell.GetComponent<SpriteRenderer>().color = Color.cyan;
						continue;
					}
				}else if(GetNonDiagonalDirection(currentCell,neighbor) != Vector2.down){
					if(currentCell.j == 0 && !neighbor.isGround && !neighbor.isPassableGround && !neighbor.isWall){
						continue;
					}
					if(currentCell.j > 0 && Mathf.Round(currentCell.j) % 2 != 0 && currentCell.j % 1 == 0){
						continue;
					}
				}else{
					bool skip = false;
					Cell curCellNeigh;
					if(currentCell.j < currentMaxJumpHeight && currentCell.j != 0){
						continue;
					}

					for(int j = 0; j < currentCell.neighbors.Count; j++){
						curCellNeigh = currentCell.neighbors[j];
						if(GetNonDiagonalDirection(currentCell,curCellNeigh) == Vector2.left || GetNonDiagonalDirection(currentCell,curCellNeigh) == Vector2.right){
							if(curCellNeigh.isWall || curCellNeigh.isGround || curCellNeigh.isPassableGround){
								skip = true;
								break;
							}
						}
					}
					if(skip){
						continue;
					}
				}

				// neem een aanloopje voor je op een muur komt.
				if((currentCell.parent == null || currentCell.parent != null && GetNonDiagonalDirection(currentCell,currentCell.parent) != Vector2.down) && currentCell.j == 0 && neighbor.isWall && !currentCell.isWall 
				   || (currentCell.isWall && (currentCell.parent == null || (currentCell.parent != null && !currentCell.parent.isWall && GetNonDiagonalDirection(currentCell,currentCell.parent) == Vector2.down)))){

					continue;
				}

				gScore += currentCell.j;

				gScoreIsBest = false;

				if(!neighbor.isOpen){
					gScoreIsBest = true;

					neighbor.h = Heuristic(neighbor.position,end);

					openList.Add(neighbor);
					neighbor.isOpen = true;
				}else if(gScore < neighbor.g + neighbor.j){
					gScoreIsBest = true;
				}

				if(gScoreIsBest){
					neighbor.parent = currentCell;
					neighbor.g = gScore;
					neighbor.f = neighbor.g + neighbor.h;
				}
			}
		}

		return new List<Cell> ();
	}



	private static int SortOnF(Cell a, Cell b){
		if (a.f > b.f || a.f == b.f && a.h > b.h) {
			return 1;
		} else {
			return -1;
		}
	}

	private static List<Cell> GetNeighbors(Grid grid,Cell cell){

		List<Cell> neighbors = new List<Cell> ();
		int x = (int)cell.position.x;
		int y = (int)cell.position.y;

		if (grid.GetCell (x - 1, y) != null) {
			neighbors.Add(grid.GetCell(x - 1,y));
		}
		if (grid.GetCell (x + 1, y) != null) {
			neighbors.Add(grid.GetCell(x + 1, y));
		}
		if (grid.GetCell (x, y - 1) != null) {
			neighbors.Add(grid.GetCell (x, y - 1));
		}
		if (grid.GetCell (x, y + 1) != null) {
			neighbors.Add(grid.GetCell (x, y + 1));
		}
		/*
		// check voor diagonale cellen
		if(grid.GetCell(x-1, y-1) != null) {
			neighbors.Add(grid.GetCell(x-1, y-1));
		}
		if(grid.GetCell(x+1, y-1) != null) {
			neighbors.Add(grid.GetCell(x+1, y-1));
		}
		if(grid.GetCell(x+1, y+1) != null) {
			neighbors.Add(grid.GetCell(x+1, y+1));
		}
		if(grid.GetCell(x-1, y+1) != null) {
			neighbors.Add(grid.GetCell(x-1, y+1));
		}*/
		return neighbors;
	}

	private static int Heuristic(Vector2 pos0, Vector2 pos1)
	{
		// This is the Manhattan distance
		int d1 = (int)Mathf.Abs (pos1.x - pos0.x);
		int d2 = (int)Mathf.Abs (pos1.y - pos0.y);
		return d1 + d2;
	}

	private static bool IsDiagonal(Cell center,Cell neighbor) {
		if(center.position.x != neighbor.position.x && center.position.y != neighbor.position.y) {
			return true;
		}
		return false;
	}

	private static Vector2 GetNonDiagonalDirection(Cell center, Cell neighbor){
		Vector2 dir = new Vector2 (0, 0);
		if (center.position.x != neighbor.position.x && center.position.y == neighbor.position.y) {
			if (neighbor.position.x < center.position.x) {
				dir = Vector2.left;
			} else if (neighbor.position.x > center.position.x) {
				dir = Vector2.right;
			}
		} else if (center.position.x == neighbor.position.x && center.position.y != neighbor.position.y) {
			if (neighbor.position.y < center.position.y) {
				dir = Vector2.down;
			} else if (neighbor.position.y > center.worldPosition.y) {
				dir = Vector2.up;
			}
		}
		return dir;
	}
}
