using UnityEngine;
using System.Collections;

public class PlatformerMovement : MonoBehaviour {

	// public constants
	public static int DIR_RIGHT = 1;
	public static int DIR_LEFT = -1;
	public static int DIR_DOWN = -1;
	public static int DIR_UP = 1;

	// public delegates and events
	public delegate void GameObjectDelegate (GameObject obj);
	public delegate void NormDelegate ();

	public event GameObjectDelegate LandedOnGround;
	public event GameObjectDelegate ReleasedFromGround;

	public event GameObjectDelegate StartedWallSlide;
	public event GameObjectDelegate EndedWallSlide;

	public event NormDelegate Jumped;
	public event NormDelegate DoubleJumped;
	public event NormDelegate StartedRunning;
	public event NormDelegate StoppedRunning;

	// Current state.
	private bool _inWallSlide = false;
	private bool _onGround = false;
	private bool _isRunning = false;

	// Bool for double jump
	private bool _doubleJumped = false;

	// Collider variables
	private BoxCollider2D colliderBox;
	private Vector2 centerCollider;
	private Vector2 sizeCollider;

	// Remember Data
	private GameObject _preGround;
	private GameObject _prePlatform;
	private GameObject _preWall;
	private float _oldGravityScale;

	//Physics
	private Rigidbody2D _rigidbody;

	//TouchDetector
	TouchDetector2D touch;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		colliderBox = GetComponent<BoxCollider2D>();
	}

	void Start(){
		touch = gameObject.GetComponent<TouchDetector2D> ();
		touch.TouchStarted += TouchDetectionStart;
		touch.OnTouch += InTouch;
		touch.TouchEnded += TouchDetectionEnd;

		_oldGravityScale = _rigidbody.gravityScale;
	}

	void Update()
	{
		if(inWallSlide && _rigidbody.velocity.y > -1)
		{
			_rigidbody.velocity -= new Vector2(0,0.3f);
		} else if(_inWallSlide && _rigidbody.velocity.y < -1)
		{
			_rigidbody.velocity = new Vector2(0, -1);
		}
	}
	public void StopRunning()
	{
		if(StoppedRunning != null)
			StoppedRunning();

		_isRunning = false;
	}
	public void StartSliding()
	{
		_rigidbody.gravityScale = 0f;
		//_rigidbody.velocity = new Vector2(0,-1);
	}
	public void StopSliding()
	{
		_rigidbody.gravityScale = _oldGravityScale;
	}
	public void MoveHorizontal(int directionConst, float moveSpeed){

		if (transform.localScale.x != directionConst * Mathf.Abs(transform.localScale.x)) {
			transform.localScale = new Vector3 (directionConst * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}

		if(!_isRunning && _onGround)
		{
			if(StartedRunning != null){
				StartedRunning();
				_isRunning = true; // changed
			}
		} 
		else if(!_onGround)
		{
			StopRunning();
		}

		if ((touch.IsTouchingSideGetGameObject(new Vector2(directionConst,0)) == null || touch.IsTouchingSideGetGameObject(new Vector2(directionConst,0)).tag != Tags.ENVIREMENT)) {
			transform.Translate (new Vector3 (directionConst * moveSpeed + _rigidbody.velocity.x, 0, 0) * Time.deltaTime);
			//damping the velocity so you can walljump more times
			if(_rigidbody.velocity.x != 0 && directionConst == 1)
			{
				_rigidbody.velocity += new Vector2(moveSpeed * 2, 0) * Time.deltaTime;
				if(_rigidbody.velocity.x > 0)
					_rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
			} 
			else if(_rigidbody.velocity.x != 0 && directionConst == -1)
			{
				_rigidbody.velocity -= new Vector2(moveSpeed * 2, 0) * Time.deltaTime;
				if(_rigidbody.velocity.x < 0)
					_rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
			}
		}
	}

	public void MoveVertical(int directionConst, float moveSpeed)
	{
		if (!_onGround) {
			transform.Translate (new Vector3 (0, directionConst * moveSpeed, 0) * Time.deltaTime);
		}
		if(directionConst == -1 && _onGround && _preGround.tag == Tags.PASSABLE) {
			BoxCollider2D preGroundCol = _preGround.GetComponent<BoxCollider2D>();
			Physics2D.IgnoreCollision(this.colliderBox, preGroundCol, true);
		}
	}

	public void Jump(float jumpForce)
	{
		if(_onGround)
		{
			_rigidbody.velocity = new Vector2(0,jumpForce);
			if(Jumped != null){
				Jumped();
			}
		} 
		else if(_inWallSlide)
		{
			//check wich direction you are currently sliding at + add velocity at negative direction.
			_rigidbody.velocity = new Vector2(-GetPlayerDirection() * jumpForce/2,jumpForce);
			this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			if(Jumped != null){
				Jumped();
			}
		}
		else if(!_doubleJumped)
		{
			_rigidbody.velocity = new Vector2(0,jumpForce/1.5f);
			_doubleJumped = true;
			if(DoubleJumped != null){
				DoubleJumped();
			}
		}
	}

	public int GetPlayerDirection()
	{
		int dir = 0;
		if(transform.localScale.x > 0)
		{
			dir = 1;
		}
		else 
		{
			dir = -1;
		}
		return dir;
	}
	
	void TouchDetectionStart(GameObject obj, Vector2 vec){
		if (vec == Vector2.down) {
			// we can even say if its a player then no grounded but attack if stunned else ignore.

			//Checking if player just fell down a platform or just jumped through a platform, if thats true stop ignorring collision.
			if(_preGround != null) {
				if(_preGround != obj) {
					BoxCollider2D preGroundCol = _preGround.GetComponent<BoxCollider2D>();
					if(Physics2D.GetIgnoreCollision(this.colliderBox, preGroundCol)) {
						Physics2D.IgnoreCollision(this.colliderBox, preGroundCol, false);
					}
				}
			}
			if(_prePlatform != null)
			{
				if(_prePlatform == obj)
				{
					BoxCollider2D prePlatformCol = _prePlatform.GetComponent<BoxCollider2D>();
					if(Physics2D.GetIgnoreCollision(this.colliderBox, prePlatformCol)) {
						Physics2D.IgnoreCollision(this.colliderBox, prePlatformCol, false);
					}
				}
			}

			BoxCollider2D objCol = obj.GetComponent<BoxCollider2D>();
			if(!Physics2D.GetIgnoreCollision(this.colliderBox, objCol) && (obj.tag == Tags.ENVIREMENT || obj.tag == Tags.PASSABLE)) {
				_onGround = true;
				_doubleJumped = false;

				_inWallSlide = false;
				StopSliding();
				if(EndedWallSlide != null)
					EndedWallSlide(_preWall);
			} 

			_preGround = obj;

			if(LandedOnGround != null){
				LandedOnGround(obj);
			}
		} else if (vec == Vector2.left || vec == Vector2.right) {
			if(!_onGround && obj.tag == Tags.ENVIREMENT){
				//if the object is collideable with the platformer then wallslide is true
				BoxCollider2D objCol = obj.GetComponent<BoxCollider2D>();
				if(!Physics2D.GetIgnoreCollision(this.colliderBox, objCol)) {
					_inWallSlide = true;
					_doubleJumped = false;
					_preWall = obj;
					StartSliding();
					if(StartedWallSlide != null){
						StartedWallSlide(obj);
					}
				} 
			}
		} else if (vec == Vector2.up) { //if you bump your head against a platform that is passable then stop colliding with given platform
			if(obj.tag == Tags.PASSABLE) {
				BoxCollider2D objCol = obj.GetComponent<BoxCollider2D>();
				Physics2D.IgnoreCollision(this.colliderBox, objCol, true);
				_prePlatform = obj;
			}
		}
	}

	void InTouch(GameObject go, Vector2 dirVector){

	}

	void TouchDetectionEnd(GameObject obj, Vector2 vec){
		if (vec == Vector2.down) {
			_onGround = false;
			if (ReleasedFromGround != null) {
				ReleasedFromGround (_preGround);
			}
		} else if (vec == Vector2.left || vec == Vector2.right) {
			_inWallSlide = false;
			StopSliding();
			if(EndedWallSlide != null){
				EndedWallSlide(_preWall);
			}
		}  else if(vec == Vector2.up) {
			if(_preGround != null) {
				if(obj == _preGround) { //set collision back to normal if you passed through it
					BoxCollider2D objCol = obj.GetComponent<BoxCollider2D>();
					if(Physics2D.GetIgnoreCollision(this.colliderBox, objCol)) {
						Physics2D.IgnoreCollision(this.colliderBox, objCol, false);
					}
				}
			}
		}
	}

	public bool onGround{
		get{return _onGround;}
	}
	public bool inWallSlide{
		get{return _inWallSlide;}
	}
}
