using UnityEngine;
using System.Collections;

public class DestroyParticleAfterLifeTime : MonoBehaviour {
	void Start () {
		ParticleSystem myParticleSystem = GetComponent<ParticleSystem>();
		Destroy(this.gameObject, myParticleSystem.startLifetime);
	}
}
