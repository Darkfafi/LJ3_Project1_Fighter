using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellPrefabInfo : MonoBehaviour {

	public delegate void CellInfoDelegate(CellPrefabInfo cellInfo);
	public event CellInfoDelegate cellInfo;

	private bool _isBlocked;
	private bool _isGround;
	private bool _isPassableGround;
	private bool _isWall;

	private Vector2 _cellSize; 

	private List<Cell> _linkedCells = new List<Cell> ();

	//debug
	private Color _trueColor;

	void Start(){

		RaycastHit2D hitLeft = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y), Vector2.left, 0.002f); 
		RaycastHit2D hitRight = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y), Vector2.right, 0.002f);
		RaycastHit2D hitUp = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y), Vector2.up, 0.002f); 
		RaycastHit2D hitDown = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y), Vector2.down, 0.002f);

		if (hitLeft.collider != null && hitLeft.collider.tag == Tags.ENVIREMENT 
			&& hitRight.collider != null && hitRight.collider.tag == Tags.ENVIREMENT
			&& hitUp.collider != null && hitUp.collider.tag == Tags.ENVIREMENT
			&& hitDown.collider != null && hitDown.collider.tag == Tags.ENVIREMENT) {

			GetComponent<SpriteRenderer> ().color = Color.black;
			_isBlocked = true;

		} else {

			//send raycast to check if its above a blocked area.
			hitDown = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y - _cellSize.y / 2), Vector2.down, 0.15f); // vector down or up... check what is better in tests.

			if (hitDown.collider != null) {
				if (hitDown.collider.gameObject.tag == Tags.ENVIREMENT) {
					//TODO GROUND OBJECT: Create logic!
					GetComponent<SpriteRenderer> ().color = Color.red;
					_isGround = true;
				} else if (hitDown.collider.gameObject.tag == Tags.PASSABLE) {
					//TODO PASSABLE OBJECT: Create logic!
					GetComponent<SpriteRenderer> ().color = Color.yellow; 
					_isPassableGround = true;
				}
			}
		}


		if (_isBlocked) {
			hitLeft = Physics2D.Raycast (new Vector2(transform.position.x - _cellSize.x,transform.position.y - 0.05f), Vector2.left, 0.002f); 
			hitRight = Physics2D.Raycast (new Vector2(transform.position.x + _cellSize.x,transform.position.y - 0.05f), Vector2.right, 0.002f);

			if(//(hitLeft.collider == null && hitRight.collider == null) 
			   (hitLeft.collider != null && hitLeft.collider.gameObject.tag == Tags.ENVIREMENT && hitRight.collider == null) 
			   || (hitLeft.collider == null  && hitRight.collider != null && hitRight.collider.gameObject.tag == Tags.ENVIREMENT))
			   //|| ((hitLeft.collider != null && hitLeft.collider.tag != Tags.ENVIREMENT) &&(hitRight.collider != null && hitRight.collider.tag != Tags.ENVIREMENT)))
		   {
				//TODO Wall (Wall Jumpable) OBJECT: Create logic! 
				GetComponent<SpriteRenderer> ().color = Color.blue;
				_isWall = true;
			}
		}

		_trueColor = gameObject.GetComponent<SpriteRenderer> ().color;
	}
	

	public bool isBlocked{
		set{
			_isBlocked = value;
			UpdateLinkedCells();
		}
		get{return _isBlocked;}
	}

	public bool isGround{
		set{
			_isGround = value;
			UpdateLinkedCells();
		}
		get{return _isGround;}
	}
	public bool isPassableGround{
		set{
			_isPassableGround = value;
			UpdateLinkedCells();
		}
		get{return _isPassableGround;}
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

				_linkedCells[i].isBlocked = _isBlocked;
				_linkedCells[i].isGround = _isGround;
				_linkedCells[i].isPassableGround = _isPassableGround;
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

	public void DebugColor(bool on,bool reached = false){
		if (!on) {
			GetComponent<SpriteRenderer>().color = _trueColor;
		} else {
			if(reached){
				GetComponent<SpriteRenderer>().color = Color.white;
			}else{
				GetComponent<SpriteRenderer>().color = Color.green;
			}
		}
	}
}
