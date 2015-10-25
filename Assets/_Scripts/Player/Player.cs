using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int playerID = 404;
	public string playerType = "NoType";

	public delegate void PlayerGameObjectDelegate(Player player, GameObject attacker);
	public event PlayerGameObjectDelegate GotKilled;

	public delegate void NormDelegate();
	public event NormDelegate StartStunned;
	public event NormDelegate StopStunned;
	public event NormDelegate StartedDying;

	public event NormDelegate StartedAction;

	//Stats
	private PlayerTransformer _playerTransformer;

	//private PlayerStats _playerStats = new PlayerStats(6f,10f,3f,5f,10f,12f); // <--- Idee
	private PlayerStats _playerStats = new PlayerStats (5f, 10f, 2f, 5f, 10f, 10f); // set all base stats

	private List<string> _busyAction = new List<string>();

	public const string IN_STUNNED = "InStunned";
	public const string IN_DEATH = "InDeath";
 

	// Combat
	private BasicStunAttack _basicAttack;
	private SpecialAttack _specialAttack;
	private AttackCather _attackCatcher;
	private ClashAble _clashAble;


	private ComTimer _stunTimer;
	private FadeInOut _fader;
	private GameObject _lastKiller;
	private ComTimer _spawnInvulnerableTimer;
	
	// Utils
	private PlatformerMovement _myPlatformerMovement;
	private PlayerAnimationHandler _playerAnimHandler;

	private Rigidbody2D rigidBody;

	void Awake()
	{
		this.transform.tag = Tags.PLAYER;

		_myPlatformerMovement = gameObject.AddComponent<PlatformerMovement>();

		_attackCatcher = gameObject.AddComponent<AttackCather> ();
		_clashAble = gameObject.AddComponent<ClashAble> ();
		_basicAttack = gameObject.AddComponent<BasicStunAttack> ();
		_playerTransformer = gameObject.AddComponent<PlayerTransformer> ();
		gameObject.AddComponent<TouchDetector2D> ();
		gameObject.AddComponent<LandOnTopKill> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		_playerAnimHandler = gameObject.AddComponent<PlayerAnimationHandler> ();
		gameObject.AddComponent<RigidbodyUtil2D>();

		_fader = gameObject.AddComponent<FadeInOut> ();

		gameObject.AddComponent<PlayerEffects>();
		gameObject.AddComponent<PlayerSoundHandler>();


		_stunTimer = gameObject.AddComponent<ComTimer> ();

		_stunTimer.TimerEnded += StunTimerEnded;

		_attackCatcher.OnStunAttackCatch += OnStunHit;
		_attackCatcher.OnStunKillAttackCatch += OnStunKillHit; // if Jump on my head hit while in stun
		_attackCatcher.OnKillAttackCatch += OnKillHit;
	}

	void Start()
	{
		if (GetComponent<AIPlayer> () == null && GetComponent<PlayerInput> () != null) {

		}

		_myPlatformerMovement.ReleasedFromGround += ReleasedGround;
		_myPlatformerMovement.StartedWallSlide += StartWallSlide;

	}

	// Movement
	public void MoveRight()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveHorizontal (PlatformerMovement.DIR_RIGHT, _playerStats.movementSpeed);
			if(_myPlatformerMovement.onGround){
				_playerAnimHandler.PlayAnimation("Run");
			}
		}
	}

	void StartWallSlide(GameObject wallObject){
		if (!busyAction) {
			_playerAnimHandler.PlayAnimation ("WallSlide");
		}
	}

	public void SetCharacter(string characterName)
	{
		CharDBInfo characterInfo = CharDB.GetCharacterDataBaseInfo (characterName);
		_playerTransformer.transformStats = characterInfo.transformationStatsBuff;
		_specialAttack = gameObject.GetComponent<SpecialAttack> ();
	}

	public void TransformPlayer(string playerTransformerConst){
		_specialAttack.UseCounterReset ();
		_playerTransformer.TransformCharacter (this,playerTransformerConst);
	}

	public void MoveLeft()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveHorizontal(PlatformerMovement.DIR_LEFT, playerStats.movementSpeed);
			if(_myPlatformerMovement.onGround){
				_playerAnimHandler.PlayAnimation("Run");
			}
		}
	}
	public void OnNoKeyPressed(){
		if (!busyAction && _myPlatformerMovement.onGround) {
			_myPlatformerMovement.StopRunning();
			_playerAnimHandler.PlayAnimation("Idle");
		}
	}
	public void Jump()
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
				playerAnimHandler.PlayAnimation ("Fall");
			}
		}
	}

	public void FallDown()
	{
		if (!busyAction) {
			_myPlatformerMovement.MoveVertical (PlatformerMovement.DIR_DOWN, _playerStats.fallSpeed);
		}
	}

	public void DoAction(){
		if (!busyAction) {
			if(StartedAction != null){
				StartedAction();
			}
			AttackBase currentAttack = _basicAttack;
			if(!_playerStats.transformed){
				currentAttack = _basicAttack;
			}else{
				currentAttack = _specialAttack;
			}
			if(!currentAttack.cooldownTimer.running){
				_playerAnimHandler.PlayAnimation("Attack");
				currentAttack.Attack(this); //geef stats class, player class of gameobject mee zodat de infor gegeven globaal kan blijven.
			}
		}
	}

	void OnStunHit(float stunPower, GameObject attacker, float pushPower){
		if (!_stunTimer.running) {
			GetStunned(stunPower);
		}
	}

	void OnStunKillHit(GameObject attacker, float pushPower){
		if (_stunTimer.running) {
			StartDeath(attacker);
		}
	}
	void OnKillHit(GameObject attacker, float pushPower){
		StartDeath (attacker);
	}

	void GetStunned(float stunPower){
		_playerAnimHandler.PlayAnimation("Stunned");
		_stunTimer.StartTimer ((int)(0.45f * stunPower));
		if (StartStunned != null) {
			StartStunned ();
		}
		_myPlatformerMovement.StopSliding();
		AddBusyAction (IN_STUNNED);
	}
	void HealStun(){
		RemoveBusyAction (IN_STUNNED);
		_stunTimer.StopTimer();
		if (StopStunned != null) {
			StopStunned ();
		}
	}
	void StartDeath(GameObject killer){
		if(StartedDying != null)
			StartedDying();
		HealStun ();
		AddBusyAction (IN_DEATH);
		SetInvulnerable (true);
		_lastKiller = killer;
		_fader.OnFadeEnd += DeathFadeEnd;
		_playerAnimHandler.PlayAnimation("Death");
		_fader.Fade (0,0.008f);
		_myPlatformerMovement.StopSliding();
	}
	void DeathFadeEnd(float valueFade){
		GetKilled (_lastKiller);
	}
	void GetKilled(GameObject attacker){
		this.gameObject.SetActive(false);
		_fader.SetAlpha (0.5f);
		_fader.SetAlpha (1,false);
		_fader.OnFadeEnd -= DeathFadeEnd;
		SetInvulnerable(false);
		ResetBusyAction ();
		if (GotKilled != null) {
			GotKilled (this,attacker);
		}
	}

	void StunTimerEnded(){
		HealStun ();
	}

	public void SetInvulnerable(bool invulnerable){
		_attackCatcher.catcherOn = !invulnerable;
	}

	// Spawn Protection (called by game controller)

	public void ActivateIdleProtection(int timeProtectedInFullSeconds){
		SetInvulnerable (true);

		_spawnInvulnerableTimer = gameObject.AddComponent<ComTimer> ();
		_spawnInvulnerableTimer.StartTimer (0.2f,(timeProtectedInFullSeconds * 5));
		_spawnInvulnerableTimer.TimerTik += ProtectionTik;

		StartedAction += EndProtection;
	}

	private void ProtectionTik(int repeat){

		Color color1 = new Color(1,1,1,0.8f);
		Color color2 = new Color(0.8f,0.8f,0.8f,0.8f);

		SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer> ();
		if (repeat == _spawnInvulnerableTimer.timesGivenToRepeat) {
			EndProtection ();
		} else {
			if(rend.color != color1){
				rend.color = color1;
			}else{
				rend.color = color2;
			}
		}
	}

	private void EndProtection(){
		SetInvulnerable (false);
		gameObject.GetComponent<SpriteRenderer> ().color = new Color(1,1,1);
		_spawnInvulnerableTimer.TimerTik -= ProtectionTik;
		StartedAction -= EndProtection;

		Destroy (_spawnInvulnerableTimer);
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
	public bool busyAction{
		get{return _busyAction.Count > 0;}
	}
	public void AddBusyAction(string actionString){
		if (!_busyAction.Contains (actionString)) {
			_busyAction.Add(actionString);
		}
	}
	public void RemoveBusyAction(string actionString){
		if (_busyAction.Contains (actionString)) {
			_busyAction.Remove(actionString);
		}
	}

	public bool CheckIfInBusyAction(string actionString){
		if (_busyAction.Contains (actionString)) {
			return true;
		} else {
			return false;
		}
	}

	public void ResetBusyAction(){
		_busyAction.Clear ();
	}

	public SpecialAttack specialAttack{
		get{return _specialAttack;}
	}
}
