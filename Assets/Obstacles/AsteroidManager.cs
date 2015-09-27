using UnityEngine;
using System.Collections;

public class AsteroidManager : MonoBehaviour
{
	// Two different asteroids
	public GameObject[] astswap;
	int counter;

	public GameObject asteroid;
	GameController mainstate;

	// Max/min velocity
	public float speedmin;
	public float speedmax;

	// Current number of asteroids
	int curasteroids;
	
	// Camera details
	Vector2 campos;
	//Vector2 endpos;
	Vector2 startpos;
	Vector2 instpos;	// position for instantiation
	float xDist, yDist, yMax, xMax;

	// Use this for initialization
	void Start ()
	{
		counter = 0;

		mainstate = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		// Camera details
		campos = Camera.main.transform.position;
		xDist = Camera.main.aspect * Camera.main.orthographicSize;
		yDist = Camera.main.orthographicSize;
		xMax = campos.x + xDist;
		yMax = campos.y + yDist;

		//endpos = new Vector2 (campos.x, yMin - 0.1f);
		startpos = new Vector2 (campos.x, yMax + 0.1f);

		// Just in case
		transform.position = startpos;
		curasteroids = 0;

		// Initial speeds
		speedmin = -1;
		speedmax = -3;

		StartCoroutine (newAsteroids ());
		StartCoroutine (speedUp ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Speed up asteroids
	IEnumerator speedUp ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}
		while (mainstate.curState == GameController.gameState.Ongoing) {
			yield return new WaitForSeconds (5f);
			speedmin -= 0.9f;
			//Debug.Log ("speedmin " + speedmin);
			speedmax -= 0.9f;
			//Debug.Log ("speedmax " + speedmax);
			yield return null;
		}
		curasteroids = 0;
		while (mainstate.curState == GameController.gameState.Lightspeed) {
			yield return null;
		}

		yield return null;
	}

	// Creates new asteroids
	IEnumerator newAsteroids ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}
		while (mainstate.curState == GameController.gameState.Ongoing) {
			if (curasteroids < 5) {
				instpos.x = Random.Range (-xMax + 0.1f, xMax - 0.1f);
				instpos.y = yMax + 0.3f;
				asteroid = (GameObject)Instantiate (astswap [counter % 2], instpos, Quaternion.identity);
				asteroid.transform.SetParent (transform, true);
				curasteroids++;
				counter++;
				yield return new WaitForSeconds (2f);
			} else {
				yield break;
			}
		}
		while (mainstate.curState == GameController.gameState.Lightspeed) {
			yield return null;
		}
	}
}
