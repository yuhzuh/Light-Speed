using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController maingame;

	public PointController pointcon;
	
	// All UI canvas gameobjects
	public GameObject infopanel;
	public Canvas ongoingcanvas;
	public Canvas endcanvas;
	public Canvas scorecanvas;
	public Canvas startcanvas;
	// Canvas groups for further alteration
	CanvasGroup ongoingcg;
	CanvasGroup endcg;
	public CanvasGroup scorecg; //Public cause needed to be reference in replay
	
	// Different speeds for fades
	float speed1 = 0.05f;
	float speed2 = 0.02f;
	
	// Game states
	public enum gameState
	{
		Start,
		End,
		Ongoing,
		Lightspeed,
		Molten
	}
	;
	
	// Current game state
	public gameState curState;
	
	void Awake ()
	{
		curState = gameState.Start;
		maingame = this;
	}

	// Use this for initialization
	void Start ()
	{
		//Max 60 fps
		//Application.targetFrameRate = 30;
		
		// Only set start canvas to true
		ongoingcanvas.enabled = false;
		endcanvas.enabled = false;
		startcanvas.enabled = true;
		infopanel.SetActive (false);

		ongoingcg = ongoingcanvas.GetComponent<CanvasGroup> ();
		endcg = endcanvas.GetComponent<CanvasGroup> ();
		scorecg = scorecanvas.GetComponent<CanvasGroup> ();


		StartCoroutine (UIManager ());
		
		// Clear console
		Debug.ClearDeveloperConsole ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Quit application on back button if info screen is not active
		if (Input.GetKeyDown (KeyCode.Escape) && !infopanel.activeInHierarchy) {
			Application.Quit ();
		} 
	}
	
	// Enable/disable UI canvas gameobjects based on gamestate
	IEnumerator UIManager ()
	{
		while (curState == gameState.Start) {
			yield return null;
		}
		if (curState == gameState.Ongoing) {
			ongoingcanvas.enabled = true;
			StartCoroutine (FadeIn (ongoingcg, speed1));

			StartCoroutine (FadeOut (startcanvas, speed1));
			StartCoroutine (FadeOut (scorecanvas, speed1));
			//scorecanvas.enabled = false;
		}

		while (curState == gameState.Ongoing) {
			yield return null;
		}

		//Light Speed Stage
		while (curState == gameState.Lightspeed) {
			yield return null;
		}

		if (curState == gameState.End) {
			StartCoroutine (FadeOut (ongoingcanvas, speed1));
			//ongoingcanvas.enabled = false;

			endcanvas.enabled = true;
			StartCoroutine (FadeIn (endcg, speed2));

			scorecanvas.enabled = true;
			StartCoroutine (FadeIn (scorecg, speed2));
		}

		// Update persistent stats;
		if (pointcon.curpoints > StatsManager.stats.highscore) {
			StatsManager.stats.highscore = pointcon.curpoints;
		} 
		StatsManager.stats.gamesplayed++;
		StatsManager.stats.Save ();
		StatsManager.stats.ClearConsole ();

		yield break;
	}

	IEnumerator FadeIn (CanvasGroup other, float speed)
	{
		other.alpha = 0;

		while (other.alpha < 1) {
			other.alpha += speed;
			yield return null;
		}
		yield break;
	}

	// Takes in canvas or else it'll disable before the fade starts
	public IEnumerator FadeOut (Canvas other, float speed)
	{
		other.GetComponent<CanvasGroup> ().alpha = 1;

		while (other.GetComponent<CanvasGroup> ().alpha > 0) {
			other.GetComponent<CanvasGroup> ().alpha -= speed;
			yield return null;
		}

		other.enabled = false;
		yield break;
	}
}
