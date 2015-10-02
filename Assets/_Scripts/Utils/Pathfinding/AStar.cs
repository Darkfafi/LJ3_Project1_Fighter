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


		openList.Add (grid.GetCell ((int)start.x, (int)start.y));

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

			neighbors = currentCell.neighbors;

			int l = neighbors.Count;

			for(int i = 0; i < l; i++){
				neighbor = neighbors[i];

				if(neighbor.isClosed || currentCell.isWall){
					continue; // ignore cell
				}

				if(IsDiagonal(currentCell,neighbor)){
					gScore = currentCell.g + diagonalScore;
				}else{
					gScore = currentCell.g + horizontalScore;
				}

				gScoreIsBest = false;

				if(!neighbor.isOpen){
					gScoreIsBest = true;

					neighbor.h = Heuristic(neighbor.position,end);

					openList.Add(neighbor);
					neighbor.isOpen = true;
				}else if(gScore < neighbor.g){
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
		}
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
}
