using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {
	public GameObject specialItem;
	private int _spawnPercentage;
	private int _spawnTimer;
	private GameObject _currentSpecialItem;
	private bool _isSpawningItem;

	void Start()
	{
		StartCoroutine(CheckSpawnItem());
		_spawnPercentage = 10; //10%
		_spawnTimer = 0;
	}

	IEnumerator CheckSpawnItem () 
	{
		if(_currentSpecialItem == null && !_isSpawningItem)
		{
			if(Random.Range(0,100) >= _spawnPercentage) //% change to spawn item per second
			{
				StartSpawningItem();
			}
		}
		yield return new WaitForSeconds(1);
	}
	private void StartSpawningItem()
	{
		_isSpawningItem = true;
	}
	private void SpawnItem()
	{
		Vector3 eulerSpawnItemRot = new Vector3(0,Random.Range(0,360), 0); //randomize the rotation of the item
		_currentSpecialItem = Instantiate(specialItem, this.transform.position, Quaternion.identity) as GameObject;
		_currentSpecialItem.transform.eulerAngles = eulerSpawnItemRot;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.transform.tag == Tags.PLAYER && _isSpawningItem)
		{
			_spawnTimer += 1;
			//TODO: update visual
			if(_spawnTimer == 100)
			{
				SpawnItem();
				_isSpawningItem = false;
				_spawnTimer = 0;
			}
		}
	}
}
