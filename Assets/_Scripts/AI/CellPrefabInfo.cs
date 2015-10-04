using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellPrefabInfo : MonoBehaviour {

	public delegate void CellInfoDelegate(CellPrefabInfo cellInfo);
	public event CellInfoDelegate cellInfo;

	private bool _isWall;
	private Vector2 _cellSize; 

	private List<Cell> _linkedCells = new List<Cell> ();
	
	void Start(){
		//send raycast to check if its above a wall
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.up, 0.01f);

		if (hit.collider != null) {
			if(hit.collider.gameObject.tag == Tags.ENVIREMENT){
				Debug.Log("ddfg");
				GetComponent<SpriteRenderer>().color = Color.red;
				_isWall = true;
			}
		}

	}
	

	public bool isWall{
		set{
			_isWall = value;
			UpdateLinkedCells();
		}
		get{return _isWall;}
	}

	public Vector2 cellSize{
		get{return _cellSize;}
		set{
			_cellSize = value;
			UpdateLinkedCells();
		}
	}

	public void UpdateLinkedCells(){
		for(int i = _linkedCells.Count - 1; i >= 0; i--){
			if(_linkedCells[i] != null){
				_linkedCells[i].isWall = _isWall;
				_linkedCells[i].worldPosition = new Vector2(transform.position.x,transform.position.y);
				_linkedCells[i].cellSize = _cellSize;
			}else{
				_linkedCells.RemoveAt(i);
			}
		}
	}

	public List<Cell> linkedCells{
		get{return _linkedCells;}
	}
}
