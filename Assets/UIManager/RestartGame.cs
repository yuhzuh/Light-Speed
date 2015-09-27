using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestartGame : MonoBehaviour
{
	Button replaybutton;

	// Stop double clicks
	bool clicked;

	CanvasGroup endSpecific;

	// Use this for initialization
	void Start ()
	{
		replaybutton = GetComponent<Button> ();
		replaybutton.onClick.AddListener (() => {
			restart ();});

		clicked = false;

		endSpecific = GameController.maingame.endcanvas.transform.GetChild (0).transform.GetChild (0).GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void restart ()
	{
		if (!clicked) {
			StartCoroutine (fadeEnd ());
			clicked = true;
		}
	}

	IEnumerator fadeEnd ()
	{
		StartCoroutine (GameController.maingame.FadeOut (GameController.maingame.scorecanvas, 0.05f));
		StartCoroutine (fadeOutSpec (endSpecific));

		while (GameController.maingame.scorecg.alpha > 0) {
			yield return null;
		}

		Application.LoadLevel (0);

		yield break;
	}

	IEnumerator fadeOutSpec (CanvasGroup other)
	{
		other.alpha = 1;
		
		while (other.alpha > 0) {
			other.alpha -= 0.05f;
			yield return null;
		}

		yield break;
	}
}
