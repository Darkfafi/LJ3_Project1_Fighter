using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllFollowCamera : MonoBehaviour {
	//This script is made by Djamali Jones and modified by Menno Jongejan
	//Thanks djamali for the help
	
	private List<GameObject> allPlayers = new List<GameObject>();
	
	private float boundingBoxPadding = 1f;
	
	private float minimumOrthographicSize = 4.5f;

	private float zoomSpeed = 20f;

	private float _originalSizeCam;

	private GameObject _boundsRepGo;
	
	private float _boundLeftLocation;
	private float _boundRightLocation;
	private float _boundTopLocation;
	private float _boundBottomLocation;


	Camera _camera;
	void Awake()
	{
		SpriteRenderer rndr;

		_camera = GetComponent<Camera>();
		_camera.orthographic = true;

		_originalSizeCam = _camera.orthographicSize;
		_boundsRepGo = GameObject.FindGameObjectWithTag (Tags.SCREEN_BOUND_OBJECT);

		rndr = _boundsRepGo.gameObject.GetComponent<SpriteRenderer> ();
		
		_boundRightLocation = _boundsRepGo.transform.position.x + ((rndr.bounds.size.x * _boundsRepGo.transform.localScale.x) / 2);
		_boundLeftLocation = _boundsRepGo.transform.position.x - ((rndr.bounds.size.x * _boundsRepGo.transform.localScale.x) / 2);
		_boundTopLocation = _boundsRepGo.transform.position.y + ((rndr.bounds.size.y * _boundsRepGo.transform.localScale.y) / 2);
		_boundBottomLocation = _boundsRepGo.transform.position.y - ((rndr.bounds.size.y * _boundsRepGo.transform.localScale.y) / 2);
	}

	void LateUpdate()
	{
		if(allPlayers.Count == 0)
		{
			FindAllPlayers();
		}

		Rect boundingBox = CalculatePlayersBoundingBox();
		//transform.position = CalculateCameraPosition(boundingBox);
		_camera.orthographicSize = CalculateOrthographicSize(boundingBox);
		CorrectCameraPlacement (CalculateCameraPosition(boundingBox));
	}

	Rect CalculatePlayersBoundingBox()
	{
		float minX = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity;
		float minY = Mathf.Infinity;
		float maxY = Mathf.NegativeInfinity;

		foreach(GameObject player in allPlayers)
		{
			Vector3 position = player.transform.position;

			minX = Mathf.Min(minX, position.x);
			minY = Mathf.Min(minY, position.y);
			maxX = Mathf.Max(maxX, position.x);
			maxY = Mathf.Max(maxY, position.y);
		}

		return Rect.MinMaxRect(minX - boundingBoxPadding,
		                       maxY + boundingBoxPadding,
		                       maxX + boundingBoxPadding,
		                       minY - boundingBoxPadding);
	}

	Vector3 CalculateCameraPosition (Rect boundingBox)
	{
		Vector2 boundingBoxCenter = boundingBox.center;
		
		return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, this.transform.position.z);
	}

	void CorrectCameraPlacement(Vector3 positionToPlace){
		float camHeightHalfSize = Camera.main.orthographicSize;
		float camWidthHalfSize = camHeightHalfSize * Screen.width / Screen.height;

		Vector2 newCamPos = new Vector2(positionToPlace.x,positionToPlace.y); 


		if (positionToPlace.x + camWidthHalfSize > _boundRightLocation) {
			newCamPos.x = _boundRightLocation - camWidthHalfSize;
		} else if(positionToPlace.x - camWidthHalfSize < _boundLeftLocation){
			newCamPos.x = _boundLeftLocation + camWidthHalfSize;
		}
		
		if (positionToPlace.y + camHeightHalfSize > _boundTopLocation) {
			newCamPos.y = _boundTopLocation - camHeightHalfSize;
		} else if (positionToPlace.y - camHeightHalfSize < _boundBottomLocation) {
			newCamPos.y = _boundBottomLocation + camHeightHalfSize;
		}
	
		_camera.transform.position = Vector3.Lerp(_camera.transform.position,new Vector3(newCamPos.x,newCamPos.y,_camera.transform.position.z),0.2f);
	}

	float CalculateOrthographicSize (Rect boundingBox)
	{
		float orthographicSize = _camera.orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topRightAsViewport = _camera.WorldToViewportPoint(topRight);
		
		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = Mathf.Abs(boundingBox.width) / _camera.aspect / 2f;
		else
			orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

		if (orthographicSize > _originalSizeCam) {
			orthographicSize = _originalSizeCam;
		}

		return Mathf.Clamp(Mathf.Lerp(_camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
	}

	void FindAllPlayers()
	{
		GameObject[] currentPlayers = GameObject.FindGameObjectsWithTag(Tags.PLAYER);
		foreach (var player in currentPlayers) {
			allPlayers.Add(player);
		}
	}
}