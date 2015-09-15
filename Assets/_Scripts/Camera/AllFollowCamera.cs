using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllFollowCamera : MonoBehaviour {
	//This script is made by Djamali Jones and modified by Menno Jongejan
	//Thanks djamali for the help
	
	private List<GameObject> allPlayers = new List<GameObject>();
	
	private float boundingBoxPadding = 1f;
	
	private float minimumOrthographicSize = 5f;

	private float zoomSpeed = 20f;

	Camera camera;
	void Awake()
	{
		camera = GetComponent<Camera>();
		camera.orthographic = true;
	}

	void LateUpdate()
	{
		if(allPlayers.Count == 0)
		{
			FindAllPlayers();
		}

		Rect boundingBox = CalculatePlayersBoundingBox();
		transform.position = CalculateCameraPosition(boundingBox);
		camera.orthographicSize = CalculateOrthographicSize(boundingBox);
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

	float CalculateOrthographicSize (Rect boundingBox)
	{
		float orthographicSize = camera.orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topRightAsViewport = camera.WorldToViewportPoint(topRight);
		
		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = Mathf.Abs(boundingBox.width) / camera.aspect / 2f;
		else
			orthographicSize = Mathf.Abs(boundingBox.height) / 2f;
		
		return Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
	}

	void FindAllPlayers()
	{
		GameObject[] currentPlayers = GameObject.FindGameObjectsWithTag(Tags.PLAYER);
		foreach (var player in currentPlayers) {
			allPlayers.Add(player);
		}
	}
}