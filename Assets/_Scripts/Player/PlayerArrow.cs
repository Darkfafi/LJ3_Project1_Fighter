using UnityEngine;
using System.Collections;

public class PlayerArrow : MonoBehaviour {
	private GameObject _myArrow;

	public void Init()
	{
		GameObject newArrow = Resources.Load("Prefabs/arrow", typeof(GameObject)) as GameObject;
		_myArrow = Instantiate(newArrow, this.transform.position + new Vector3(0,2.7f,0), newArrow.transform.rotation) as GameObject;
		_myArrow.transform.parent = this.transform;
		_myArrow.transform.localScale *= 0.25f;
	}

	public void SetColor(Color color)
	{
		SpriteRenderer mySpriteRenderer = _myArrow.GetComponent<SpriteRenderer>();
		color.a = 0.5f;
		mySpriteRenderer.color = color;
	}
}
