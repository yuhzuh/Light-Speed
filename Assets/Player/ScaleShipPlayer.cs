using UnityEngine;
using System.Collections;

public class ScaleShipPlayer : MonoBehaviour
{
	public GameObject ship;
	public GameObject player;
	ParticleSystem tailparticle;
	GameObject tail;

	GameController mainstate;

	// Main camera
	Camera main;
	// Camera height and width
	float camheight, camwidth;
	// Temporary storage for sprite component
	Sprite temp;
	// Distance between ship and player during game start
	Vector2 beginoffset;
	// Store temp's world units
	float unitWidth, unitHeight;

	// Keep collider radius
	CircleCollider2D playercol;
	float radius;

	// Use this for initialization
	void Awake ()
	{
		// Get main camera component
		main = Camera.main;

		ship = (GameObject)Instantiate (ship, Vector2.zero, Quaternion.identity);

		// Main camera height and width
		camheight = main.orthographicSize * 2;
		camwidth = camheight * main.aspect;

		// Set temp to ship sprite
		temp = ship.GetComponent<SpriteRenderer> ().sprite;

		// How big bg1 is in world units
		unitWidth = temp.textureRect.width / temp.pixelsPerUnit;
		unitHeight = temp.textureRect.height / temp.pixelsPerUnit;

		// Scale ship according to camera size
		ship.transform.localScale = new Vector3 (camwidth / (unitWidth * 10f), camheight / (unitHeight * 10f));

		// Add polygon collider to ship
		//ship.AddComponent<PolygonCollider2D> ();
		// Doesn't fucking work fuck u

		// Set ship parent to ShipController
		ship.transform.SetParent (transform, true);

		// Change Particle Tail Size
		tail = ship.transform.GetChild (0).gameObject;
		tailparticle = tail.GetComponent<ParticleSystem> ();
		tailparticle.startSize *= camheight / (unitHeight * 36f);
		tailparticle.startSpeed *= camwidth / (unitWidth * 27f);
		tail.SetActive (false);


		// Set beginning offset
		beginoffset = Vector2.zero;
		beginoffset.y = -2;

		player = (GameObject)Instantiate (player, beginoffset, Quaternion.identity);

		// Set temp to player sprite
		temp = player.GetComponent<SpriteRenderer> ().sprite;

		// How big player is in world units
		unitWidth = temp.textureRect.width / temp.pixelsPerUnit;
		//unitHeight = temp.textureRect.height / temp.pixelsPerUnit;

		// Scale player according to camera size
		player.transform.localScale = new Vector3 (camwidth / (unitWidth * 10f), camwidth / (unitWidth * 10f));
		playercol = player.GetComponent<CircleCollider2D> ();
		radius = playercol.radius;

		player.transform.SetParent (transform, true);
	}

	void Start ()
	{
		mainstate = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		StartCoroutine (playerScaleUp ());
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	// When mouse clicks player, the size of the player changes
	IEnumerator playerScaleUp ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}
		for (int i = 11; i < 20; i++) {
			// Scale player according to camera size
			player.transform.localScale = new Vector3 (camwidth / (unitWidth * i), camwidth / (unitWidth * i));
			yield return null;
		}
		playercol.radius = radius * 4f;
		yield break;

	}
}
