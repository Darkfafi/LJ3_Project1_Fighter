using UnityEngine;
using System.Collections;

public class PlayerSoundHandler : MonoBehaviour {
	private AudioList _audioList;

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
		_audioList = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<AudioList>();
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
		_audioList.PlayAudio(dashSoundName);
	}

	void PlayClashSound() {
		_audioList.PlayAudio(clashSoundName);
	}

	void PlayDeathSound(Player player, GameObject attacker) {
		_audioList.PlayAudio(deathSoundName);
	}

	void PlayJumpSound () {
		_audioList.PlayAudio(jumpSoundName);
	}

	void PlayDoubleJumpSound () {
		_audioList.PlayAudio(doubleJumpSoundName);
	}
	void PlayRunSound() {
		_audioList.PlayAudio(runSoundName, true);
	}
	void StopRunSound() {
		_audioList.StopAudio(runSoundName);
	}
	void PlayTransformSound()
	{
		_audioList.PlayAudio(transformSoundName);
	}
	void PlayHitSound()
	{
		_audioList.PlayAudio(hitSoundName);
	}
}
