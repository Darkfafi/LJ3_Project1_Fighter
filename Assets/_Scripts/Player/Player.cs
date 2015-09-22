using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public delegate void PlayerGameObjectDelegate(Player player, GameObject attacker);
	public event PlayerGameObjectDelegate GotKilled;

	public delegate void NormDelegate();
	public event NormDelegate StartStunned;
	public event NormDelegate StopStunned;

	//Stats
	private PlayerTransformer _playerTransformer;

	//private PlayerStats _playerStats = new PlayerStats(6f,10f,3f,5f,10f,12f); // <--- Idee
	private PlayerStats _playerStats = new PlayerStats (5f, 10f, 2f, 5f, 10f, 10f); // set all base stats

	public bool busyAction = false; 
	

	// Combat
	private BasicStunAttack _basicAttack;
	private SpecialAttack _specialAttack;
	private AttackCather _attackCatcher;
	private ClashAble _clashAble;


	private Timer _stunTimer;
	
	// Input
	private PlayerInput _myPlayerInput;
	private string _horizontalAxis = "HorizontalPlayer1";
	private string _verticalAxis = "VerticalPlayer1";
	private string _actionKey = "ActionKeyPlayer1";
	private string _jumpKey = "Null";

	// Utils
	private PlatformerMovement _myPlatformerMovement;
	private PlayerAnimationHandler _playerAnimHandler;

	private Rigidbody2D rigidBody;

	void Awake()
	{
		this.transform.tag = Tags.PLAYER;

		_myPlayerInput = gameObject.AddComponent<PlayerInput>();
		_myPlatformerMovement = gameObject.AddComponent<PlatformerMovement>();

		_attackCatcher = gameObject.AddComponent<AttackCather> ();
		_clashAble = gameObject.AddComponent<ClashAble> ();
		_basicAttack = gameObject.AddComponent<BasicStunAttack> ();
		_playerTransformer = gameObject.AddComponent<PlayerTransformer> ();
		gameObject.AddComponent<TouchDetector2D> ();
		gameObject.AddComponent<LandOnTopKill> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		_playerAnimHandler = gameObject.AddComponent<PlayerAnimationHandler> ();

		gameObject.AddComponent<PlayerEffects>();
		gameObject.AddComponent<PlayerSoundHandler>();

		_attackCatcher.OnStunAttackCatch += OnStunHit;
		_attackCatcher.OnStunKillAttackCatch += OnStunKillHit; // if Jump on my head hit while in stun
		_attackCatcher.OnKillAttackCatch += OnKillHit;
	}

	void Start()
	{
		_myPlayerInput.RightKeyPressed += MoveRight;
		_myPlayerInput.LeftKeyPressed += MoveLeft;
		_myPlayerInput.JumpKeyPressed += Jump;
		_myPlayerInput.DownKeyPressed += FallDown;
		_myPlayerInput.ActionKeyPressed += DoAction;
		_myPlayerInput.NoKeyPressed += OnNoKeyPressed;

		_myPlatformerMovement.ReleasedFromGround += ReleasedGround;
	}

	// Movement
	void MoveRight()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_RIGHT, _playerStats.movementSpeed);
			if(_myPlatformerMovement.onGround){
				_playerAnimHandler.PlayAnimation("Run");
			}
		}
	}

	public void SetCharacter(string characterName)
	{
		CharDBInfo characterInfo = CharDB.GetCharacterDataBaseInfo (characterName);
		_playerTransformer.transformStats = characterInfo.transformationStatsBuff;
		_specialAttack = gameObject.GetComponent<SpecialAttack> ();
	}

	public void SetKeys(string playerHorizontalAxis,string playerVerticalAxis,string playerActionKey, string playerJumpKey = "Null")
	{
		_horizontalAxis = playerHorizontalAxis;
		_verticalAxis = playerVerticalAxis;
		_actionKey = playerActionKey;
		_jumpKey = playerJumpKey;
	}

	public void TransformPlayer(string playerTransformerConst){
		_playerTransformer.TransformCharacter (this,playerTransformerConst);
	}

	void MoveLeft()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT, playerStats.movementSpeed);
			if(_myPlatformerMovement.onGround){
				_playerAnimHandler.PlayAnimation("Run");
			}
		}
	}
	void OnNoKeyPressed(){
		if (!busyAction && _myPlatformerMovement.onGround) {
			_myPlatformerMovement.StopRunning();
			_playerAnimHandler.PlayAnimation("Idle");
		}
	}
	void Jump()
	{
		if (!busyAction) {
			_myPlatformerMovement.Jump(_playerStats.jumpForce);
			_playerAnimHandler.PlayAnimation("Jump");
		}
	}

	void ReleasedGround(GameObject obj){
		if (!busyAction) {
			if (rigidBody.velocity.y > 0) {
				_playerAnimHandler.PlayAnimation ("Jump");
			}
		}
	}

	void Update(){
		if (!busyAction && !_myPlatformerMovement.onGround && !_myPlatformerMovement.inWallSlide) {
			if (rigidBody.velocity.y < -0.2) {
				_playerAnimHandler.PlayAnimation ("Fall");
			}
		}
	}

	void FallDown()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveVertical (PlatformerMovement.DIR_DOWN, _playerStats.fallSpeed);
		}
	}

	void DoAction(){
		if (!busyAction) {
			AttackBase currentAttack = _basicAttack;
			if(!_playerStats.transformed){
				currentAttack = _basicAttack;
			}else{
				currentAttack = _specialAttack;
			}
			_playerAnimHandler.PlayAnimation("Attack");
			currentAttack.Attack(this); //geef stats class, player class of gameobject mee zodat de infor gegeven globaal kan blijven.
		}
	}

	// Hit by attacks (MAYBE CODE IN A DIFFERENT COMPONENT)
	void OnStunHit(float stunPower, GameObject attacker, float pushPower){
		//TODO CALL STUN FUNCTION
		if (_stunTimer == null || !_stunTimer.IsRunning ()) {
			GetStunned(stunPower);
		}
	}

	void OnStunKillHit(GameObject attacker, float pushPower){
		//TODO IF STUNNED THEN CALL DEAD FUNCTION
		if (_stunTimer != null && _stunTimer.IsRunning ()) {
			GetKilled(attacker);
		}
	}
	void OnKillHit(GameObject attacker, float pushPower){
		//TODO CALL DEAD FUNCTION
		GetKilled (attacker);
	}

	void GetStunned(float stunPower){
		_playerAnimHandler.PlayAnimation("Stunned");
		_stunTimer = new Timer ((int)(500 * stunPower));
		_stunTimer.TimerEnded += StunTimerEnded;
		_stunTimer.Start ();
		StartStunned();
		busyAction = true;
	}
	void HealStun(){
		busyAction = false;
		StopStunned();
		_stunTimer.Stop ();
	}
	void GetKilled(GameObject attacker){
		// Die
		this.gameObject.SetActive(false);
		//TODO: Spawn kill animation
		if (GotKilled != null) {
			GotKilled (this,attacker);
		}
	}

	void StunTimerEnded(){
		HealStun ();
	}

	// GETTERS and SETTERS
	public PlayerStats playerStats{
		get{return _playerStats;}
	}

	public PlayerAnimationHandler playerAnimHandler{
		get{return _playerAnimHandler;}
	}

	public ClashAble clasher{
		get{return _clashAble;}
	}

	//public variables for keyinputs
	public string horizontalAxis{
		get {
			return _horizontalAxis;
		}
		set {
			_horizontalAxis = value;
		}
	}
	public string verticalAxis{
		get {
			return _verticalAxis;
		}
		set {
			_verticalAxis = value;
		}
	}
	public string actionKey{
		get {
			return _actionKey;
		}
		set {
			_verticalAxis = value;
		}
	}
	public string jumpKey{
		get {
			return _jumpKey;
		}
		set {
			_jumpKey = value;
		}
	}
}
