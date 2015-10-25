using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {
	public GameObject specialItem;
	private int _spawnPercentage;
	private int _spawnTimer;
	private int _percentageCounter;
	private int _standardSpawnPercentage;
	private GameObject _currentSpecialItem;
	private bool _isSpawningItem;

	void Start()
	{
		StartCoroutine("CheckSpawnItem");
		_isSpawningItem = false;
		_standardSpawnPercentage = 5; //5%
		_spawnPercentage = _standardSpawnPercentage;
		_spawnTimer = 0;
		_percentageCounter = 0;

	}

	IEnumerator CheckSpawnItem () 
	{
		while(true)
		{
			if(_currentSpecialItem == null && !_isSpawningItem)
			{
				if(Random.Range(0,100) <= _spawnPercentage) //% change to spawn item per second
				{
					StartSpawningItem();
					_spawnPercentage = _standardSpawnPercentage;
				}
				else
				{
					_percentageCounter++;
					if(_percentageCounter == 10)
					{
						_spawnPercentage += 1;
						_percentageCounter = 0;
					}
				}
			} 
			yield return new WaitForSeconds(1);
		}
	}
	private void StartSpawningItem()
	{
		_isSpawningItem = true;
		GetComponent<SpriteRenderer>().color = Color.red;
	}
	private void SpawnItem()
	{
		Vector3 eulerSpawnItemRot = new Vector3(0, 0, Random.Range(0,360)); //randomize the rotation of the item
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
				GetComponent<SpriteRenderer>().color = Color.white;
				_isSpawningItem = false;
				_spawnTimer = 0;
			}
		}
	}
}
