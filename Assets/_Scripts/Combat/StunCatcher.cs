using UnityEngine;
using System.Collections;

public class StunCatcher : MonoBehaviour {

	public delegate void FloatGoDelegate(float valueFloat, GameObject Go);
	public event FloatGoDelegate OnStunCatch;

	public void CatchStun(GameObject obj, float stunPower){
		if (OnStunCatch != null) {
			OnStunCatch(stunPower,obj);
		}
	}
}
