using UnityEngine;
using System.Collections;

public class ClashAble : MonoBehaviour {

	public bool clashAble = false;

	public void Clash(GameObject otherClashAbleObject,float clashPower = 100f){
		if (otherClashAbleObject.GetComponent<ClashAble> () != null) {
			Debug.Log(otherClashAbleObject);
			//otherClashAbleObject.GetComponent<Rigidbody2D>().velocity = otherClashAbleObject.GetComponent<Rigidbody2D>().velocity.normalized * -clashPower; 
		}
	}
}
