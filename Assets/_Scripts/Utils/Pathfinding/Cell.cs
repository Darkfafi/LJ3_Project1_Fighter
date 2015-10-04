using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell{

	public float g = 0; // dist score to start point
	public float h = 0; // dist score to end point
	public float f = 0; // g + h (start distance + end distance (from this point))

	public bool isWall = false; //Is this cell able to be crossed.

	public bool isOpen = false; 
	public bool isClosed = false;

	public Cell parent;
	public List<Cell> neighbors;

	private Vector2 _position;
	private Vector2 _worldPosition;
	private Vector2 _cellSize;

	public Cell(int x, int y){
		_position = new Vector2(x,y);
	}

	public Vector2 worldPosition{
		get{return _worldPosition;}
		set{_worldPosition = value;}
	}

	public Vector2 position{
		get{return _position;}
	}
	public Vector2 cellSize{
		get{return _cellSize;}
		set{_cellSize = value;}
	}

	public string ToString(){
		return "[x: " + _position.x + ", y:" + _position.y + ", f: " + f + "]";  
	}
}
