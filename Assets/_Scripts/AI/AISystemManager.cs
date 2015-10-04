using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISystemManager : MonoBehaviour {

	private GameObject levelObject; 
	private Vector2 levelBounds;

	private List<List<GameObject>> _prefabGrid;

	// Use this for initialization
	void Start () {

		PlaceAIGrid ();
	}

	void PlaceAIGrid(){
		Vector3 tempBounds;
		GameObject cellsHolder = new GameObject ();
		cellsHolder.name = "CellsHolder";

		GameObject cell = Resources.Load("Prefabs/AI/Cell") as GameObject;
		_prefabGrid = new List<List<GameObject>> ();
		List<GameObject> listRow;

		levelObject = GameObject.FindGameObjectWithTag (Tags.SCREEN_BOUND_OBJECT);
		tempBounds = levelObject.GetComponent<SpriteRenderer> ().bounds.size;

		levelBounds = new Vector2 (tempBounds.x, tempBounds.y);

		float cellWidth = cell.GetComponent<SpriteRenderer> ().bounds.size.x;
		float cellHeight = cell.GetComponent<SpriteRenderer> ().bounds.size.y;

		int widthGrid = (int)(levelBounds.x / cellWidth);
		int heightGrid = (int)(levelBounds.y / cellHeight);

		for (int xRow = 0; xRow < widthGrid; xRow++) {
			listRow = new List<GameObject>();
			_prefabGrid.Add(listRow);
			for(int yRow = 0; yRow < heightGrid; yRow++){
				cell = Instantiate((GameObject)Resources.Load("Prefabs/AI/Cell"),new Vector3(levelObject.transform.position.x + ((cellWidth / 2) + (xRow * cellWidth)) - (levelBounds.x / 2),levelObject.transform.position.y + ((cellHeight / 2) + (yRow * cellHeight)) - (levelBounds.y / 2),1),Quaternion.identity) as GameObject;
				cell.transform.SetParent(cellsHolder.transform);
				cell.GetComponent<CellPrefabInfo>().cellSize = new Vector2(cellWidth,cellHeight);
				listRow.Add(cell);
			}
		}
	}

	public List<List<GameObject>> prefabGridList{
		get{return _prefabGrid;}
	}
}
