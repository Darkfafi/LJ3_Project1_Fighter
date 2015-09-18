using UnityEngine;
using System.Collections;

public class DestroyAfterAnimDone : MonoBehaviour {
	private Animator _myAnimator;

	void Awake () {
		_myAnimator = GetComponent<Animator>();
	}
	void Start () {
		Destroy(this.gameObject, _myAnimator.GetCurrentAnimatorStateInfo(0).length);
	}
}
