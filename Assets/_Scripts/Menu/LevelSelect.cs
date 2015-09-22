using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public void SelectLevel(string levelString){
		PlayerPrefs.SetString ("LevelChosen", levelString);
		Application.LoadLevel("RamsesScene");
	}
}
