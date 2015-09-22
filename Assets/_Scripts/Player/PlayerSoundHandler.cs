using UnityEngine;
using System.Collections;

public class PlayerSoundHandler : MonoBehaviour {
	private AudioPlayer _audioPlayer;

	private string jumpSoundName;
	private string runSoundName;
	private string doubleJumpSoundName;
	private string dashSoundName;
	private string clashSoundName;
	private string hitSoundName;
	private string transformSoundName;
	private string deathSoundName;
	// Use this for initialization
	void Awake () {
		_audioPlayer = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<AudioPlayer>();
	}

	void Start()
	{
		jumpSoundName = "Sound";
		runSoundName = "Sound";
		doubleJumpSoundName = "Sound";
		dashSoundName = "Sound";
		clashSoundName = "Sound";
		hitSoundName = "Sound";
		transformSoundName = "Sound";
		deathSoundName = "Sound";

		PlatformerMovement myPlatformerMovement = GetComponent<PlatformerMovement>();
		myPlatformerMovement.Jumped += PlayJumpSound;
		myPlatformerMovement.DoubleJumped += PlayDoubleJumpSound;
		myPlatformerMovement.StartedRunning += PlayRunSound;
		myPlatformerMovement.StoppedRunning += StopRunSound;

		Player myPlayerScript = GetComponent<Player>();
		myPlayerScript.GotKilled += PlayDeathSound;
		myPlayerScript.StartStunned += PlayHitSound;

		GetComponent<ClashAble>().Clashed += PlayClashSound;

		GetComponent<BasicStunAttack>().AttackStarted += PlayDashSound;

		GetComponent<PlayerTransformer>().StartedTransformCharacter += PlayTransformSound;
	}

	void PlayDashSound() {
		_audioPlayer.PlayAudio(dashSoundName);
	}

	void PlayClashSound() {
		_audioPlayer.PlayAudio(clashSoundName);
	}

	void PlayDeathSound(Player player, GameObject attacker) {
		_audioPlayer.PlayAudio(deathSoundName);
	}

	void PlayJumpSound () {
		_audioPlayer.PlayAudio(jumpSoundName);
	}

	void PlayDoubleJumpSound () {
		_audioPlayer.PlayAudio(doubleJumpSoundName);
	}
	void PlayRunSound() {
		_audioPlayer.PlayAudio(runSoundName, true, this);
	}
	void StopRunSound() {
		_audioPlayer.StopAudio(runSoundName, this);
	}
	void PlayTransformSound()
	{
		_audioPlayer.PlayAudio(transformSoundName);
	}
	void PlayHitSound()
	{
		_audioPlayer.PlayAudio(hitSoundName);
	}
}
