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
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, 0.15f); // vector down or up... check what is better in tests.

		if (hit.collider != null) {
			if (hit.collider.gameObject.tag == Tags.ENVIREMENT) {
				//TODO GROUND OBJECT: Create logic!
				GetComponent<SpriteRenderer> ().color = Color.red;
				_isWall = true;
			} else if (hit.collider.gameObject.tag == Tags.PASSABLE) {
				//TODO PASSABLE OBJECT: Create logic!
				GetComponent<SpriteRenderer> ().color = Color.yellow; 
			}
		}


		if (_isWall) {
			RaycastHit2D hitLeft = Physics2D.Raycast (new Vector2(transform.position.x - _cellSize.x,transform.position.y - 0.15f), Vector2.left, _cellSize.x / 2); 
			RaycastHit2D hitRight = Physics2D.Raycast (new Vector2(transform.position.x + _cellSize.x,transform.position.y - 0.15f), Vector2.right, _cellSize.x / 2);

			if((hitLeft.collider == null && hitRight.collider == null) 
			   || (hitLeft.collider != null && hitLeft.collider.gameObject.tag == Tags.ENVIREMENT && hitRight.collider == null) 
			   || (hitLeft.collider == null  && hitRight.collider != null && hitRight.collider.gameObject.tag == Tags.ENVIREMENT)
			   || ((hitLeft.collider != null && hitLeft.collider.tag != Tags.ENVIREMENT) &&(hitRight.collider != null && hitRight.collider.tag != Tags.ENVIREMENT))){
				//TODO Wall (Wall Jumpable) OBJECT: Create logic! 
				GetComponent<SpriteRenderer> ().color = Color.blue;
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
