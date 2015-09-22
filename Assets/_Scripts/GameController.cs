using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static bool isPaused = false;
	
	public delegate void pauseGame();
	public event pauseGame PauseGame;
	public event pauseGame ResumeGame;
	
	private List<Transform> _currentSpawnPoints = new List<Transform>();

	private List<Player> _currentPlayers = new List<Player>();
	private List<int> _currentPlayerLives = new List<int>();

	private Dictionary<float, GameObject> _playersToSpawnWithCounter = new Dictionary<float, GameObject>();
	private Dictionary<Player, int> _playerKills = new Dictionary<Player, int>(); //to store the kills made by wich player

	private int spawnTime = 3; //time to spawn player in seconds
	private int playerLives; //stocks
	private int playTime; //for when time game mode is added

	private float _levelBorderMinY = -10f;
	private float _levelBorderMaxY = 10f;
	private float _levelBorderMinX = -10f;
	private float _levelBorderMaxX = 10f;

	private ComTimer _timer;

	private bool _suddenDeath;

	public void Start()
	{
		FindAllSpawnPoints();
		_timer = gameObject.AddComponent<ComTimer>();
		Physics2D.IgnoreLayerCollision(8,8, true);
		InitGame();
	}
	private void InitGame()
	{
		GameObject boundsGameObject = GameObject.FindGameObjectWithTag (Tags.SCREEN_BOUND_OBJECT);
		
		SpriteRenderer rndr = boundsGameObject.gameObject.GetComponent<SpriteRenderer> ();
		
		_levelBorderMaxX = boundsGameObject.transform.position.x + ((rndr.bounds.size.x * boundsGameObject.transform.localScale.x) / 2) + 3;
		_levelBorderMinX = boundsGameObject.transform.position.x - ((rndr.bounds.size.x * boundsGameObject.transform.localScale.x) / 2) - 3;
		_levelBorderMaxY = boundsGameObject.transform.position.y + ((rndr.bounds.size.y * boundsGameObject.transform.localScale.y) / 2) + 3;
		_levelBorderMinY = boundsGameObject.transform.position.y - ((rndr.bounds.size.y * boundsGameObject.transform.localScale.y) / 2) - 3;

		//setting game rules
		SetGameRules();

		//init players
		InitializePlayers();
	}

	private void SetGameRules()
	{
		int stockValue = PlayerPrefs.GetInt("StockValue");
		if(stockValue != 0)
			playerLives = stockValue;
		int timeValue = PlayerPrefs.GetInt("TimeValue");
		if(timeValue != 0)
		{
			playTime = timeValue * 60;
			_timer.StartTimer(playTime);
			_timer.TimerEnded += EndGame;
		}
		spawnTime = PlayerPrefs.GetInt("SpawnTime");

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
			newPlayer.name = "Player-" + i;
			newPlayer.layer = 8;
			newPlayerScript.SetCharacter(playerCharacter);
			newPlayerScript.SetKeys(playerHorizontalAxis,playerVerticalAxis,playerActionKey, playerJumpKey);
			newPlayerScript.GotKilled += PlayerDied;
			_currentPlayers.Add(newPlayerScript);
			_currentPlayerLives.Add(playerLives);
			_playerKills.Add(newPlayerScript, 0);
		}
		//positioning players + ignoring each others collision
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
			foreach(Player player in _currentPlayers)
			{
				if(player.transform.position.y < _levelBorderMinY || player.transform.position.y > _levelBorderMaxY || player.transform.position.x > _levelBorderMaxX || player.transform.position.x < _levelBorderMinX)
				{
					player.gameObject.SetActive(false);
					player.transform.position = new Vector3(0,0,0); //reset pos
					PlayerDied(player.GetComponent<Player>(), null);
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

	private void PlayerDied(Player player, GameObject attacker)
	{
		int playerIndex = _currentPlayers.IndexOf(player);
		_currentPlayerLives[playerIndex] -= 1;

		if(attacker != null)
		{
			Player playerAttacker = attacker.GetComponent<Player>();
			_playerKills[playerAttacker]++;
		}

		if(_suddenDeath)
		{
			_currentPlayerLives[playerIndex] = 0;
		}

		if(_currentPlayerLives[playerIndex] != 0)
		{
			SetSpawnPlayer(player);
		} 
		else 
		{
			CheckPlayersAlive();
		}
	}

	private void SetSpawnPlayer(Player player)
	{
		_playersToSpawnWithCounter.Add(spawnTime + Time.time, player.gameObject);
	}

	private void CheckPlayersAlive()
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
			PlayerWon(_currentPlayers[playerIDWon]);
		}
	}

	private void EndGame()
	{
		int oldPlayerKills = 0;
		List<Player> playersTied = new List<Player>();
		Player playerWon = null;
		foreach(KeyValuePair<Player, int> entry in _playerKills)
		{
			if(entry.Value > oldPlayerKills)
			{
				oldPlayerKills = entry.Value;
				playerWon = entry.Key;
			}
		}
		foreach(KeyValuePair<Player, int> entry in _playerKills)
		{
			if(entry.Value == oldPlayerKills)
			{
				playersTied.Add(entry.Key);
			}
		}
		if(playersTied.Count > 1)
		{
			SuddenDeath(playersTied);
		} 
		else 
		{
			PlayerWon(playerWon);
		}
	}

	private void SuddenDeath(List<Player> playersTied)
	{
		//TODO: create image sudden death
		_suddenDeath = true;
		spawnTime = 10;
		foreach (var player in _currentPlayers) {
			player.enabled = false;
			if(playersTied.Contains(player))
			{
				SetSpawnPlayer(player);
				player.TransformPlayer(PlayerTransformer.SPECIAL_MOD);
			}
		}
	}

	private void PlayerWon(Player player)
	{
		//TODO: generate win screen with player that won
		Debug.Log("Player: " + player.name + " WON!");
	}
	
	private void CheckSpawnPlayer(float spawntime, GameObject player)
	{
		if(spawntime < Time.time)
		{
			player.transform.position = _currentSpawnPoints[Random.Range(0,_currentSpawnPoints.Count)].position;
			player.SetActive(true);
			_playersToSpawnWithCounter.Remove(spawntime);
		}
	} 
}
