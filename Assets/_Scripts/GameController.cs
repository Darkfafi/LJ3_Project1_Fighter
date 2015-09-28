using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	private static bool _isPaused = false;
	private static bool _physicsPaused = false;
	private static bool _lockedPause = false;

	public delegate void NormDelegate();
	public delegate void PlayerKillsDeathsDelegate(Player player, int kills, int deaths);
	public delegate void PlayerLivesDelegate(Player player, int lives);
	public delegate void IntDelegate(int value);

	public event NormDelegate PauseGame;
	public event NormDelegate ResumeGame;
	public event IntDelegate CountDownTik;

	public event PlayerKillsDeathsDelegate Win;
	public event PlayerLivesDelegate OnDeath;

	private GameObject _specialItem;
	private Transform _itemSpawnPoint;

	private GameObject _currentSpecialItem;
	private int _spawnPercentage;
	private int _percentageCounter;
	private int _standardSpawnPercentage;

	private List<Transform> _currentSpawnPoints = new List<Transform>();

	private List<Player> _currentPlayers = new List<Player>();
	private List<int> _currentPlayerLives = new List<int>();

	private Dictionary<GameObject, float> _playersToSpawnWithCounter = new Dictionary<GameObject, float>();
	private Dictionary<Player, int> _playerKills = new Dictionary<Player, int>(); //to store the kills made by wich player

	private int spawnTime = 3; //time to spawn player in seconds
	private int _playerLives; //stocks
	private int playTime; //for when time game mode is added

	private float _levelBorderMinY = -10f;
	private float _levelBorderMaxY = 10f;
	private float _levelBorderMinX = -10f;
	private float _levelBorderMaxX = 10f;

	private ComTimer _timer;

	private bool _suddenDeath;

	private bool _movingCamera = true;

	public void Awake()
	{
		CreateLevel ();
		FindAllSpawnPoints();
		_timer = gameObject.AddComponent<ComTimer>();
		Physics2D.IgnoreLayerCollision(8,8, true);
		InitGame();
		GameObject.Find ("UI").AddComponent<InGameUI> ();
		SetPause(true,false,true);
		CountDown ();
	}

	private void CountDown(){
		ComTimer comTimer = gameObject.AddComponent<ComTimer> ();
		comTimer.TimerTik += EndCountDown;
		comTimer.StartTimer (1, 3);
		Destroy (comTimer, 5);
	}

	private void EndCountDown(int amountOfRepeats){
		if (CountDownTik != null) {
			CountDownTik (amountOfRepeats);
		}
		switch (amountOfRepeats) {
		case 1:
			Debug.Log ("READY");
			break;
		case 2:
			Debug.Log ("SET");
			break;
		case 3:
			Debug.Log ("FIGHT");
			SetPause(false);
			if(PlayerPrefs.GetInt("TimeValue") != 0){
				_timer.StartTimer(playTime);
			}
			break;
		}
	}

	private void CreateLevel(){
		string levelString = PlayerPrefs.GetString ("LevelChosen");
		GameObject level = Resources.Load("Prefabs/Levels/"+levelString + "Level") as GameObject;
		Instantiate (level, new Vector3 (0, 0, 0), Quaternion.identity);
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

		GameObject.FindGameObjectWithTag (Tags.CAMERA).GetComponent<Camera> ().orthographicSize = 5.4f * GameObject.FindGameObjectWithTag(Tags.CURRENT_LEVEL).transform.localScale.x;
		if (_movingCamera) {
			GameObject.FindGameObjectWithTag(Tags.CAMERA).AddComponent<AllFollowCamera>();
		}


		_specialItem = Resources.Load("Prefabs/SpecialItem", typeof(GameObject)) as GameObject;
		_itemSpawnPoint = GameObject.FindGameObjectWithTag(Tags.ITEMSPAWN).transform;
		StartCoroutine("CheckSpawnItem");

		Win += InvokeBackToMenu;
	}
	private void InvokeBackToMenu(Player playerwon, int playerKills, int deaths)
	{
		foreach(Player player in _currentPlayers)
		{
			player.gameObject.SetActive(false);
		}
		_playersToSpawnWithCounter.Clear();
		Invoke("BackToMenu", 5);
	}
	void BackToMenu()
	{
		Application.LoadLevel(0);
	}
	IEnumerator CheckSpawnItem () 
	{
		_standardSpawnPercentage = 5; //5%
		_spawnPercentage = _standardSpawnPercentage;
		_percentageCounter = 0;
		while(true)
		{
			if(_currentSpecialItem == null)
			{
				if(Random.Range(0,100) <= _spawnPercentage) //% change to spawn item per second
				{
					SpawnItem();
					_spawnPercentage = _standardSpawnPercentage;
				}
				else
				{
					_percentageCounter++;
					if(_percentageCounter == 10)
					{
						_spawnPercentage += 5;
						_percentageCounter = 0;
					}
				}
			} 
			yield return new WaitForSeconds(1);
		}
	}
	private void SpawnItem()
	{
		Vector3 eulerSpawnItemRot = new Vector3(0, 0, Random.Range(0,360)); //randomize the rotation of the item
		_currentSpecialItem = Instantiate(_specialItem, _itemSpawnPoint.position, Quaternion.identity) as GameObject;
		_currentSpecialItem.transform.eulerAngles = eulerSpawnItemRot;
	}

	private void SetGameRules()
	{
		int stockValue = PlayerPrefs.GetInt("StockValue");
		if(stockValue != 0)
			_playerLives = stockValue;
		int timeValue = PlayerPrefs.GetInt("TimeValue");
		if(timeValue != 0)
		{
			playTime = timeValue * 60;
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
			_currentPlayerLives.Add(_playerLives);
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
		if(Input.GetKeyDown(KeyCode.Escape) && !_lockedPause)
		{
			SwitchPause();
		}
		if(!_isPaused)
		{
			if(_playersToSpawnWithCounter.Count > 0)
			{
				//create arrays of _playertospawnwithcounter so we are not itterating
				List<float> playersSpawnTime = new List<float>(_playersToSpawnWithCounter.Values);
				List<GameObject> playersToSpawn = new List<GameObject>(_playersToSpawnWithCounter.Keys);
				for (int i = 0; i < playersToSpawn.Count; i++) {
					CheckSpawnPlayer(playersSpawnTime[i], playersToSpawn[i]);
				} 
			}
			foreach(Player player in _currentPlayers)
			{
				if(player.gameObject.activeInHierarchy)
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
	}

	public void SwitchPause()
	{
		if(_isPaused)
		{
			SetPause(false);
			if(ResumeGame != null)
				ResumeGame();
		} else {
			SetPause(true);
			if(PauseGame != null)
				PauseGame();
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
		player.TransformPlayer(PlayerTransformer.NORMAL_MOD);
		_currentPlayerLives[playerIndex] -= 1;
		if(OnDeath != null){
			OnDeath(player,_currentPlayerLives[playerIndex]);
		}

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
		if(!_playersToSpawnWithCounter.ContainsKey(player.gameObject))
			_playersToSpawnWithCounter.Add(player.gameObject, spawnTime + Time.time);
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
			Player playerWon = _currentPlayers[playerIDWon];
			int kills = _playerKills[playerWon];
			int deaths = _playerLives - _currentPlayerLives[_currentPlayers.IndexOf(playerWon)];
			if(Win != null){
				Win(playerWon, kills, deaths);
			}
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
				if(!playersTied.Contains(entry.Key))
					playersTied.Add(entry.Key);
			}
		}
		if(playersTied.Count > 1)
		{
			SuddenDeath(playersTied);
		} 
		else 
		{
			int deaths = _playerLives - _currentPlayerLives[_currentPlayers.IndexOf(playerWon)];
			int kills = oldPlayerKills;
			if(Win != null){
				Win(playerWon, kills, deaths);
			}
		}
	}

	private void SuddenDeath(List<Player> playersTied)
	{
		//TODO: create image sudden death
		_suddenDeath = true;
		spawnTime = 10;
		foreach (Player player in _currentPlayers) {
			if(playersTied.Contains(player))
			{
				player.gameObject.transform.position = _currentSpawnPoints[playersTied.IndexOf(player)].position;
				player.TransformPlayer(PlayerTransformer.SPECIAL_MOD);
			} else {
				player.gameObject.SetActive(false);
			}
		}
	}
	private void CheckSpawnPlayer(float spawntime, GameObject player)
	{
		if(spawntime < Time.time)
		{
			player.transform.position = _currentSpawnPoints[Random.Range(0,_currentSpawnPoints.Count)].position;
			player.SetActive(true);
			_playersToSpawnWithCounter.Remove(player);
		}
	} 

	public static void SetPause(bool value,bool pausePhysics = true,bool lockedPause = false){
		_isPaused = value;
		_physicsPaused = pausePhysics;
		_lockedPause = lockedPause;
	}

	public static bool isPaused{
		get{return _isPaused;}
	}

	public static bool physicsPaused{
		get{return _physicsPaused;}
	}

	public int playerTotalLives{
		get{return _playerLives;}
	}
	public ComTimer gameTimer {
		get{ return _timer;}
	}
}
