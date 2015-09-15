using UnityEngine;
using System.Collections;

public class TouchAnimation : MonoBehaviour {
	public string animationName;
	private Animator _myAnimator;
	void Awake()
	{
		_myAnimator = GetComponent<Animator>();
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.transform.tag == Tags.PLAYER)
		{
			_myAnimator.Play(animationName);
		}
	}
}
