using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid {

	private List<List<Cell>> _grid;
	private int _width;
	private int _height;

	public Grid(int width, int height){
		_width = width;
		_height = height;
		_grid = new List<List<Cell>> ();

		for (int xRow = 0; xRow < _width; xRow++) {

			_grid[xRow] = new List<Cell>();

			for(int yRow = 0; yRow < _height; yRow++){
				_grid[xRow][yRow] = new Cell(xRow,yRow);
			}
		}
	}

	public Cell GetCell(int x, int y){
		Cell cellToReturn = null;
		if (_grid[x] != null && _grid [x][y] != null) {
			cellToReturn = _grid[x][y];
		}

		return cellToReturn;
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
				currentCell.isClosed = false;
				currentCell.isOpen = false;
				currentCell.parent = null;
			}
		}
	}
}
