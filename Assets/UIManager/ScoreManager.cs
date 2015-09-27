using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public PointController curpctrl;
	float points;

	Text text;

	// Use this for initialization
	void Start ()
	{
		text = GetComponent<Text> ();
		StartCoroutine (LSColor ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		text.text = curpctrl.curpoints.ToString ("00");
	}

	IEnumerator LSColor ()
	{
		while (GameController.maingame.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}

		Color orig = text.color;
		Color newc = Color.blue;
		newc.a = text.color.a;

		for (float i = 0; i < 300; i++) {
			text.color = Color.Lerp (orig, newc, i / 300);
			if (GameController.maingame.curState == GameController.gameState.End)
				break;
			yield return null;
		}

		while (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			yield return null;
		}

		yield return null;
		text.color = Color.white;

	}
}
