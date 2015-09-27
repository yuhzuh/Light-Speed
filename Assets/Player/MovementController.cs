using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
	public GameObject infopanel;

	public GameObject ship;
	SpriteRenderer shiprend;
	GameObject tail;
	ParticleSystem tailparticle;
	GameController mainstate;

	public GameObject explosion;

	// Store player sprite
	SpriteRenderer player;
	Color alpha;

	// mouse position
	Vector2 mousepos;
	// new ship position
	Vector2 shippos;
	// Distance between ship and player
	Vector3 offset;

	// Direction pos for animator
	float hor;
	Animator shipanim;

	public float speed;
	float newspeed;

	// Store vector for boundary
	Vector2 boundary;
	Vector2 campos;
	float xDist, yDist, xMax, yMax, yMin;

	// Use this for initialization
	void Start ()
	{
		ship = GameObject.FindGameObjectWithTag ("Ship");
		shiprend = ship.GetComponent<SpriteRenderer> ();
		shipanim = ship.GetComponent<Animator> ();
		tail = ship.transform.GetChild (0).gameObject;
		tailparticle = tail.GetComponent<ParticleSystem> ();

		mainstate = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		player = GetComponent<SpriteRenderer> ();
		alpha = player.color;

		offset = new Vector3 (0, 1.2f, 0);
		speed = 4f;

		// Boundary setup
		campos = Camera.main.transform.position;
		xDist = Camera.main.aspect * Camera.main.orthographicSize;
		yDist = Camera.main.orthographicSize;
		xMax = campos.x + xDist - 0.3f;
		yMax = campos.y + yDist - 1.6f;
		yMin = campos.y - yDist + 0.3f;


		StartCoroutine (speedUp ());
		StartCoroutine (TailLightSpd ());
		StartCoroutine (explode ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Store input from mouse
		mousepos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		hor = Input.GetAxisRaw ("Mouse X");

		EnforceBounds ();

		if (mainstate.curState != GameController.gameState.Start && mainstate.curState != GameController.gameState.End) {
			// Move ship to player based on speed
			shippos = transform.position + offset;
			ship.transform.position = Vector2.MoveTowards (ship.transform.position, shippos, speed * Time.deltaTime);

			// Player automatically moves to where you touched
			//if (Input.GetMouseButtonDown (0)) {
			//	transform.position = mousepos;
			//}
		}
	}

	void OnMouseDown ()
	{
		// Game state changes to ongoing, game has started
		if (mainstate.curState == GameController.gameState.Start) {
			mainstate.curState = GameController.gameState.Ongoing;
			tail.SetActive (true);
		}
	}

	void OnMouseDrag ()
	{
		transform.position = mousepos; 
		alpha.a = 0.2f;
		player.color = alpha;

		if (hor > 0) {
			shipanim.SetBool ("Right", true);
			shipanim.SetBool ("Left", false);
			shipanim.SetBool ("Idle", false);
		} else if (hor < 0) {
			shipanim.SetBool ("Right", false);
			shipanim.SetBool ("Left", true);
			shipanim.SetBool ("Idle", false);
		} else {
			shipanim.SetBool ("Right", false);
			shipanim.SetBool ("Left", false);
			shipanim.SetBool ("Idle", true);
		}
	}

	void OnMouseUp ()
	{
		alpha.a = 1;
		player.color = alpha;

		shipanim.SetBool ("Right", false);
		shipanim.SetBool ("Left", false);
		shipanim.SetBool ("Idle", true);
	}

	IEnumerator speedUp ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}
		while (mainstate.curState == GameController.gameState.Ongoing) {
			if (speed < 20) {
				yield return new WaitForSeconds (5f);
				newspeed = speed + 1;
				while (speed < newspeed) {
					speed += 0.05f;
					yield return null;
				}
			} else {
				yield return null;
			}
			if (speed >= 20) {
				mainstate.curState = GameController.gameState.Lightspeed;
				yield return null;
			}
		}
		while (mainstate.curState == GameController.gameState.Lightspeed) {
			yield return null;
		}

		yield break;
	}

	void EnforceBounds ()
	{
		boundary = mousepos;

		if (boundary.x < -xMax || boundary.x > xMax) {
			boundary.x = Mathf.Clamp (boundary.x, -xMax, xMax);
		}
		if (boundary.y < yMin || boundary.y > yMax) {
			boundary.y = Mathf.Clamp (boundary.y, yMin, yMax);
		}

		mousepos = boundary;
	}

	IEnumerator TailLightSpd ()
	{
		while (mainstate.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
		// Tail size increases for lightspeed
		if (mainstate.curState == GameController.gameState.Lightspeed) {
			float SSJTail = tailparticle.startLifetime + 1f;
			float i = 0f;

			Color orig = tailparticle.startColor;
			Color newcl = new Color ();
			newcl.r = 0;
			newcl.b = 225;
			newcl.g = 192;
			newcl.a = 225;

			while (tailparticle.startLifetime < SSJTail) {
				tailparticle.startLifetime += 0.002f;
				tailparticle.startColor = Color.Lerp (orig, newcl, i / 500);
				i++;
				yield return null;
			}
		}
		while (mainstate.curState == GameController.gameState.Lightspeed) {
			yield return null;
		}
	}

	IEnumerator explode ()
	{
		while (mainstate.curState != GameController.gameState.End) {
			yield return null;
		}

		Color next = shiprend.color;

		tail.SetActive (false);

		while (shiprend.color.a > 0) {
			next.a -= 0.2f;
			shiprend.color = next;
			yield return null;
		}

		explosion = (GameObject)Instantiate (explosion, ship.transform.position, Quaternion.identity);
		yield break;
	}

	void OnEnable ()
	{
		StartCoroutine (speedUp ());
		StartCoroutine (TailLightSpd ());
		StartCoroutine (explode ());
	}
}
