using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RetData : MonoBehaviour
{
	Text highscore, gamesplayed;

	// Use this for initialization
	void Start ()
	{
		highscore = transform.GetChild (0).GetComponent<Text> ();
		gamesplayed = transform.GetChild (1).GetComponent<Text> ();

	}

	void Update ()
	{
		// Both stats are updated in GameController
		highscore.text = "HIGHSCORE : " + StatsManager.stats.highscore.ToString ("0");
		gamesplayed.text = "GAMES PLAYED : " + StatsManager.stats.gamesplayed.ToString ("0");
	}
	

}
