using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectAnim : MonoBehaviour {
	private Animator _myAnimator;
	private List<GameObject> _myChildren = new List<GameObject>();
	// Use this for initialization
	void Awake()
	{
		_myAnimator = GetComponent<Animator>();
		for (int i = 0; i < transform.childCount; i++) {
			_myChildren.Add(transform.GetChild(i).gameObject);
		}
	}
	void Start () {
		_myAnimator.SetTrigger("Hide");
	}
	public void StartAnimation()
	{
		_myAnimator.SetTrigger("Show");
		float animLength = _myAnimator.runtimeAnimatorController.animationClips[0].length;
		Invoke("ShowCharacterSelect", animLength);
	}
	private void OnEnable()
	{
		if(!_myChildren[0].activeInHierarchy)
		{
			_myAnimator.SetTrigger("Hide");
		}
	}
	private void ShowCharacterSelect()
	{
		foreach(GameObject child in _myChildren)
		{
			child.SetActive(true);
		}
	}

	public void HideCharacterSelect()
	{
		_myAnimator.SetTrigger("Hide");
		float animLength = _myAnimator.runtimeAnimatorController.animationClips[1].length;

		foreach(GameObject child in _myChildren)
		{
			child.SetActive(false);
		}
	}
}
