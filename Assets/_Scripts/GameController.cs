using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static bool isPaused = false;
	
	public delegate void pauseGame();
	public event pauseGame PauseGame;
	public event pauseGame ResumeGame;
	
	private List<Transform> _currentSpawnPoints = new List<Transform>();

	private List<GameObject> _currentPlayers = new List<GameObject>();
	private List<int> _currentPlayerLives = new List<int>();

	private Dictionary<float, GameObject> _playersToSpawnWithCounter = new Dictionary<float, GameObject>();
	private Dictionary<int, Player> _playerKills = new Dictionary<int, Player>(); //to store the kills made by wich player

	private int spawnTime = 3; //time to spawn player in seconds
	private int playerLives; //stocks
	private int playTime; //for when time game mode is added

	private float _levelBorderMinY = -10f;
	private float _levelBorderMaxY = 10f;
	private float _levelBorderMinX = -10f;
	private float _levelBorderMaxX = 10f;


	public void Start()
	{
		FindAllSpawnPoints();
		InitGame();
	}
	private void InitGame()
	{
		string gameMode = PlayerPrefs.GetString("GameMode");
		int modeValue = PlayerPrefs.GetInt("ModeValue");
		spawnTime = PlayerPrefs.GetInt("SpawnTime");
		//Setup if gamemodes are still being added
		if(gameMode == GameRules.STOCKMODE) 
		{
			playerLives = modeValue;
		}
		else 
		{
			playTime = modeValue;
			playerLives = 255;
		}
		//init players
		InitializePlayers();
	}
	private void InitializePlayers()
	{
		//Retrieving game information
		int playersPlaying = PlayerPrefs.GetInt("PlayerCount");
		string playerCharacter;
		string playerHorizontalAxis;
		string playerVerticalAxis;
		string playerActionKey;
		string playerJumpKey;
		for (int i = 0; i < playersPlaying; i++) 
		{
			//get information
			playerCharacter = PlayerPrefs.GetString("Character-" + i);
			playerHorizontalAxis = PlayerPrefs.GetString("Horizontal-" + i);
			playerVerticalAxis = PlayerPrefs.GetString("Vertical-" + i);
			playerActionKey = PlayerPrefs.GetString("Action-" + i);
			playerJumpKey = PlayerPrefs.GetString("Jump-" + i);
			
			//spawnplayer
			GameObject newPlayer = PlayerFactory.CreatePlayer(playerCharacter, i);
			Player newPlayerScript = newPlayer.GetComponent<Player>();
			
			//add player information
			newPlayerScript.SetCharacter(playerCharacter);
			newPlayerScript.SetKeys(playerHorizontalAxis,playerVerticalAxis,playerActionKey, playerJumpKey);
			newPlayerScript.GotKilled += PlayerDied;
			_currentPlayers.Add(newPlayer);
			_currentPlayerLives.Add(playerLives);
		}
		//positioning players
		for (int i = 0; i < _currentPlayers.Count; i++) 
		{
			_currentPlayers[i].transform.position = _currentSpawnPoints[i].transform.position;
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
		if(!isPaused)
		{
			if(_playersToSpawnWithCounter.Count > 0)
			{
				//create arrays of _playertospawnwithcounter so we are not itterating
				List<float> playersSpawnTime = new List<float>(_playersToSpawnWithCounter.Keys);
				List<GameObject> playersToSpawn = new List<GameObject>(_playersToSpawnWithCounter.Values);
				for (int i = 0; i < playersToSpawn.Count; i++) {
					CheckSpawnPlayer(playersSpawnTime[i], playersToSpawn[i]);
				} 
			}
			foreach(GameObject player in _currentPlayers)
			{
				if(player.transform.position.y < _levelBorderMinY || player.transform.position.y > _levelBorderMaxY || player.transform.position.x > _levelBorderMaxX || player.transform.position.x < _levelBorderMinX)
				{
					player.SetActive(false);
					player.transform.position = new Vector3(0,0,0); //reset pos
					PlayerDied(player.GetComponent<Player>());
				}
			}
		}
	}

	private void FindAllSpawnPoints()
	{
		//getting all spawnpoints in level
		GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.SPAWN);
		foreach(GameObject spawnpoint in allSpawnPoints)
		{
			_currentSpawnPoints.Add(spawnpoint.transform);
		}
	}
	void PlayerDied(Player player)
	{
		int playerIndex = _currentPlayers.IndexOf(player.gameObject);
		_currentPlayerLives[playerIndex] -= 1;
		if(_currentPlayerLives[playerIndex] != 0)
		{
			_playersToSpawnWithCounter.Add(spawnTime + Time.time, player.gameObject);
		} 
		else 
		{
			CheckPlayersAlive();
		}
	}

	void CheckPlayersAlive()
	{
		int playersAlive = 0;
		int playerIDWon = 0;
		foreach(int playerLives in _currentPlayerLives)
		{
			if(playerLives != 0)
			{
				playersAlive++;
				playerIDWon = _currentPlayerLives.IndexOf(playerLives);
			}
		}
		if(playersAlive <= 1)
		{
			PlayerWon(playerIDWon);
		}
	}

	void PlayerWon(int playerID)
	{
		//TODO: generate win screen with player that won
		Debug.Log("Player: " + playerID + " WON!");
	}
	
	void CheckSpawnPlayer(float spawntime, GameObject player)
	{
		if(spawntime < Time.time)
		{
			player.transform.position = _currentSpawnPoints[Random.Range(0,_currentSpawnPoints.Count)].position;
			player.SetActive(true);
			_playersToSpawnWithCounter.Remove(spawntime);
		}
	} 
}
