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
	private string wallGlideSoundName;
	// Use this for initialization
	void Awake () {
		_audioPlayer = GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<AudioPlayer>();
	}

	void Start()
	{
		jumpSoundName = "jump1";
		runSoundName = "steps";
		doubleJumpSoundName = "jump1";
		dashSoundName = "dash";
		clashSoundName = "orb_hit";
		hitSoundName = "Hit_bod_5";
		transformSoundName = "Sound";
		deathSoundName = "death";
		wallGlideSoundName = "glide_wall";

		PlatformerMovement myPlatformerMovement = GetComponent<PlatformerMovement>();
		myPlatformerMovement.Jumped += PlayJumpSound;
		myPlatformerMovement.DoubleJumped += PlayDoubleJumpSound;
		myPlatformerMovement.StartedRunning += PlayRunSound;
		myPlatformerMovement.StoppedRunning += StopRunSound;
		myPlatformerMovement.StartedWallSlide += PlayWallGlideSound;
		myPlatformerMovement.EndedWallSlide += StopWallGlideSound;

		Player myPlayerScript = GetComponent<Player>();
		myPlayerScript.StartedDying += PlayDeathSound;
		myPlayerScript.StartStunned += PlayHitSound;

		GetComponent<ClashAble>().Clashed += PlayClashSound;

		GetComponent<BasicStunAttack>().AttackStarted += PlayDashSound;

		GetComponent<PlayerTransformer>().StartedTransformCharacter += PlayTransformSound;
	}

	void PlayWallGlideSound(GameObject obj)
	{
		_audioPlayer.PlayAudio(wallGlideSoundName, true, this);
	}

	void StopWallGlideSound(GameObject obj)
	{
		_audioPlayer.StopAudio(wallGlideSoundName, this);
	}

	void PlayDashSound() {
		_audioPlayer.PlayAudio(dashSoundName);
	}

	void PlayClashSound() {
		_audioPlayer.PlayAudio(clashSoundName);
	}

	void PlayDeathSound() {
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
