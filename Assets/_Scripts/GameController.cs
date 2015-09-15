using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static bool isPaused = false;
	public delegate void pauseGame();
	public event pauseGame PauseGame;
	public event pauseGame ResumeGame;

	public List<Transform> currentSpawnPoints = new List<Transform>();
	public List<Transform> currentItemSpawnPoints = new List<Transform>();
	public List<GameObject> currentPlayers = new List<GameObject>();

	private GameObject _playerPrefab;

	public void Start()
	{
		FindAllSpawnPoints();
		InitializePlayers();
	}

	private void InitializePlayers()
	{
		//TODO: create playerprefs!
		//Retrieving game information
		int playersPlaying = PlayerPrefs.GetInt("PlayerCount");
		string playerCharacter;
		string playerHorizontalAxis;
		string playerVerticalAxis;
		string playerActionKey;
		for (int i = 0; i < playersPlaying; i++) 
		{
			//get information
			playerCharacter = PlayerPrefs.GetString("Character-" + i);
			playerHorizontalAxis = PlayerPrefs.GetString("Horizontal-" + i);
			playerVerticalAxis = PlayerPrefs.GetString("Vertical-" + i);
			playerActionKey = PlayerPrefs.GetString("Action-" + i);
			
			//spawnplayer
			GameObject newPlayer = PlayerFactory.CreatePlayer(playerCharacter);//Instantiate(playerPrefab,new Vector3(0,0,0), Quaternion.identity) as GameObject;
			Player newPlayerScript = newPlayer.GetComponent<Player>();
			
			//add player information
			newPlayerScript.SetCharacter(playerCharacter);
			newPlayerScript.SetKeys(playerHorizontalAxis,playerVerticalAxis,playerActionKey);
			currentPlayers.Add(newPlayer);
		}
		//positioning players
		for (int i = 0; i < currentPlayers.Count; i++) 
		{
			currentPlayers[i].transform.position = currentSpawnPoints[i].transform.position;
		}
	}
	private void FindAllSpawnPoints()
	{
		//getting all spawnpoints in level
		GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.SPAWN);
		foreach(GameObject spawnpoint in allSpawnPoints)
		{
			currentSpawnPoints.Add(spawnpoint.transform);
		}
		//getting all itemspawnpoints in level
		GameObject[] allItemSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.ITEMSPAWN);
		foreach(GameObject spawnpoint in allItemSpawnPoints)
		{
			currentItemSpawnPoints.Add(spawnpoint.transform);
		}
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(isPaused)
			{
				isPaused = false;
				ResumeGame();
			} else {
				isPaused = true;
				PauseGame();
			}
		}
	}
}
