using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour 
{
	private RectTransform _rectTransform;
	private RoomManager _roomManager;

	private string _verticalAxis = "";
	private string _actionKey = "";
	private string _backKey = "";
	private string _jumpKey = "";

	private bool _busy;
	private bool _ready;

	private int _picHeight;
	private int _characterID = 0;
	private int _playerID;

	private float _standardY;
	private float _moveSpeed;

	private List<GameObject> _myChildren = new List<GameObject>();

	void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_roomManager = GameObject.FindGameObjectWithTag(Tags.ROOMMANAGER).GetComponent<RoomManager>();
		for (int i = 0; i < transform.childCount; i++) {
			_myChildren.Add(transform.GetChild(i).gameObject);
		}
	}

	void FadeAllOtherCharacters()
	{
		for (int i = 0; i < _myChildren.Count; i++) {
			if(i != _characterID)
			{
				_myChildren[i].GetComponent<FadeInOut>().SetAlpha(0);
			}
		}
	}

	void ColorCurrentCharacter(Color color)
	{
		_myChildren[_characterID].GetComponent<Image>().color = color;
	}

	private void OnEnable()
	{
		FadeAllOtherCharacters();
	}

	void Start()
	{
		FadeAllOtherCharacters();
		_standardY = _rectTransform.localPosition.y;
		_picHeight = 100;
		_moveSpeed = 10;
		_roomManager.UnReadyPlayers += UnReady;
	}

	void Update()
	{
		if(!_ready && !_busy)
		{
			if(Input.GetAxis(_verticalAxis) > 0)
			{
				CharacterUp();
			} 
			else if(Input.GetAxis(_verticalAxis) < 0)
			{
				CharacterDown();
			} 
		} 
		else
		{
			Vector3 newPos = _rectTransform.localPosition;
			newPos.y = _standardY + _picHeight * _characterID;

			_rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition,newPos, _moveSpeed * Time.deltaTime);

			if(Vector3.Distance(_rectTransform.localPosition,newPos) < 0.5f)
				_busy = false;
		}

		if(_jumpKey != "")
		{
			if(Input.GetButtonDown(_actionKey) && !_ready || Input.GetButtonDown(_jumpKey) && !_ready)
			{
				ReadyUp();
			} 
		} 
		else 
		{
			if(Input.GetButtonDown(_actionKey) && !_ready)
			{
				ReadyUp();
			} 
		}

		if(Input.GetButtonDown(_backKey) && _ready)
		{
			UnReady();
		} 
		else if(Input.GetButtonDown(_backKey) && !_ready)
		{
			RemoveMe();
		}
	}
	private void CharacterUp()
	{
		if(_characterID < 1)
		{
			_characterID++;
			_busy = true;
			_myChildren[_characterID-1].GetComponent<FadeInOut>().Fade(0, 0.1f);
			_myChildren[_characterID].GetComponent<FadeInOut>().Fade(1, 0.1f);
		}
	}
	private void CharacterDown()
	{
		if(_characterID > 0)
		{
			_characterID--;
			_busy = true;
			_myChildren[_characterID+1].GetComponent<FadeInOut>().Fade(0, 0.1f);
			_myChildren[_characterID].GetComponent<FadeInOut>().Fade(1, 0.1f);
		}
	}
	private void ReadyUp()
	{
		_ready = true;
		ColorCurrentCharacter(CharDB.GetColorByID(_playerID));
		PlayerPrefs.SetString("Character-" + _playerID, CharDB.GetCharacterByInt(_characterID));
		_roomManager.AddPlayerReady(this);
	}

	private void UnReady()
	{
		_ready = false;
		ColorCurrentCharacter(Color.white);
		_roomManager.RemovePlayerReady(this);
	}

	public void SetPlayer(int playerid,string controls)
	{
		_playerID = playerid;
		List<string> playerControls = Controls.GetControls(controls);
		//HorizontalAxis = playerControls[0];
		_verticalAxis = playerControls[1];
		_actionKey = playerControls[2];
		//keyboard does not need the jump key
		if(controls != Controls.keyboard01 && controls != Controls.keyboard02)
			_jumpKey = playerControls[3];

		_backKey = playerControls[4];
	}

	private void RemoveMe()
	{
		_characterID = 0;
		FadeAllOtherCharacters();
		_roomManager.DeactivatePanel(this, _playerID);
	}

	public bool isReady
	{
		get{
			return _ready;
		}
	}
}
