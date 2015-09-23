using UnityEngine;
using System.Collections;

public class DestroyAfterAnimDone : MonoBehaviour {
	private Animator _myAnimator;
	void Awake () {
		_myAnimator = GetComponent<Animator>();
	}
	void Start()
	{
		float animLength = _myAnimator.runtimeAnimatorController.animationClips[0].length / 5; // speed is 5
		Destroy(this.gameObject,animLength - Time.deltaTime);
	}
}
