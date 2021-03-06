﻿using UnityEngine;
using System.Collections;

public class ClashAble : MonoBehaviour {
	public delegate void NormDelegate();
	public event NormDelegate Clashed;

	public bool clashAble = false;
	public bool clashed = false;

	public void Clash(GameObject otherClashAbleObject,float clashPower = 100f){
		if (otherClashAbleObject.GetComponent<ClashAble> () != null) {
			if(Mathf.Abs(otherClashAbleObject.transform.localScale.x) / otherClashAbleObject.transform.localScale.x != Mathf.Abs(gameObject.transform.localScale.x) / gameObject.transform.localScale.x){
				otherClashAbleObject.GetComponent<Rigidbody2D>().velocity = otherClashAbleObject.GetComponent<Rigidbody2D>().velocity.normalized * -clashPower; 
				if(!otherClashAbleObject.GetComponent<ClashAble>().clashed)
				{
					if(Clashed != null)
						Clashed();
					clashed = true;
				}
			}
		}
	}
}
