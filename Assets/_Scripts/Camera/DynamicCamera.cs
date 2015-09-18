using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicCamera : MonoBehaviour {

	private List<GameObject> _allPlayers = new List<GameObject>();

	private GameObject _boundsRepGo;

	private float _boundLeftLocation;
	private float _boundRightLocation;
	private float _boundTopLocation;
	private float _boundBottomLocation;


	Camera _camera;

	void Awake(){

		SpriteRenderer rndr;

		_camera = gameObject.GetComponent<Camera> ();
		_camera.orthographic = true;

		_boundsRepGo = GameObject.FindGameObjectWithTag (Tags.SCREEN_BOUND_OBJECT);

		rndr = _boundsRepGo.gameObject.GetComponent<SpriteRenderer> ();

		_boundRightLocation = _boundsRepGo.transform.position.x + (rndr.bounds.size.x / 2);
		_boundLeftLocation = _boundsRepGo.transform.position.x - (rndr.bounds.size.x / 2);
		_boundTopLocation = _boundsRepGo.transform.position.y + (rndr.bounds.size.y / 2);
		_boundBottomLocation = _boundsRepGo.transform.position.y - (rndr.bounds.size.y / 2);

	}

	void LateUpdate(){
		if (_allPlayers.Count == 0) {
			FindPlayers();
		}

		CorrectCameraPlacement ();
	}

	void CorrectCameraPlacement(){
		float camHeightHalfSize = Camera.main.orthographicSize;
		float camWidthHalfSize = camHeightHalfSize * Screen.width / Screen.height;

		Vector2 newCamPos = new Vector2(0,0); 

		if (_camera.transform.position.x + camWidthHalfSize > _boundRightLocation) {
			newCamPos.x = _boundRightLocation - camWidthHalfSize;
		} else if(_camera.transform.position.x - camWidthHalfSize < _boundLeftLocation){
			newCamPos.x = _boundLeftLocation + camWidthHalfSize;
		}

		if (_camera.transform.position.y + camHeightHalfSize > _boundTopLocation) {
			newCamPos.y = _boundTopLocation - camHeightHalfSize;
		} else if (_camera.transform.position.y - camHeightHalfSize < _boundBottomLocation) {
			newCamPos.y = _boundBottomLocation + camHeightHalfSize;
		}

	}

	void FindPlayers(){
		GameObject[] plrs = GameObject.FindGameObjectsWithTag (Tags.PLAYER);
		for (int i = 0; i < plrs.Length; i++) {
			_allPlayers.Add(plrs[i]);
		}
	}
}
