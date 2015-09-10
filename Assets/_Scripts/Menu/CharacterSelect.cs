using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour 
{
	private RectTransform _rectTransform;
	private string _verticalAxis;
	private string _actionKey;
	private bool _busy;
	private bool _ready;
	private int _picHeight;
	private int _counter = 0;
	private float _moveSpeed;

	void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}
	void Start()
	{
		_picHeight = 100;
		_moveSpeed = 15;
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
			else if(Input.GetButtonDown(_actionKey))
			{
				ReadyUp();
			}
		} 
		else if(_busy)
		{

			Vector3 oldPos = _rectTransform.localPosition;
			Vector3 newPos = _rectTransform.localPosition;
			newPos.y = _picHeight * _counter;

			_rectTransform.localPosition = Vector3.Lerp(oldPos,newPos, _moveSpeed * Time.deltaTime);

			if(_rectTransform.localPosition.y == newPos.y)
				_busy = false;
		}
	}
	private void CharacterUp()
	{
		if(_counter < 3)
		{
			_counter++;
			_busy = true;
		}
	}
	private void CharacterDown()
	{
		if(_counter > 0)
		{
			_counter--;
			_busy = true;
		}
	}
	private void ReadyUp()
	{
		_ready = true;
	}
	public void SetControls(string controls)
	{
		List<string> playerControls = Controls.GetControls(controls);
		//HorizontalAxis = playerControls[0];
		_verticalAxis = playerControls[1];
		_actionKey = playerControls[2];
	}
}
