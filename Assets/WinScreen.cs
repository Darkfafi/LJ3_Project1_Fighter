using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	public Text winText;
	public RectTransform winScreenPanelTransform;

	private float slideSpeed = 25;
	private float standardY;
	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag(Tags.GAMECONTROLLER).GetComponent<GameController>().Win += ShowWinScreen;
		standardY = winScreenPanelTransform.anchoredPosition.y;
		winScreenPanelTransform.anchoredPosition = new Vector2(winScreenPanelTransform.anchoredPosition.x,-500);
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 newPos = new Vector2(winScreenPanelTransform.anchoredPosition.x, standardY);
		if(Vector2.Distance(winScreenPanelTransform.anchoredPosition, newPos) > 1f)
		{
			winScreenPanelTransform.anchoredPosition = Vector2.Lerp(winScreenPanelTransform.anchoredPosition, newPos, slideSpeed * Time.deltaTime);
		}
	}

	void ShowWinScreen(Player playerwon, int kills, int deaths)
	{
		this.gameObject.SetActive(true);
		winText.text = playerwon.name + " WINS!" + "\n";
		winText.text += "Kills - " + kills.ToString() + "\n";
		winText.text += "Deaths - " + deaths.ToString();
	}
}
