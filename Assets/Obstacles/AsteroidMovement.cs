using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour
{
	AsteroidManager curasteroidstate;
	GameController mainstate;

	//Sotre rigidbody
	Rigidbody2D curRig;

	// Asteroid details
	Vector2 speed;
	float turnspeed;

	// Scaling Asteroids Randomly
	// Main camera
	Camera main;
	// Camera height and width
	float camheight, camwidth;
	// Temporary storage for sprite component
	Sprite temp;
	// Store temp's world units
	float unitWidth;//, unitHeight;
	// Random scaling
	float randomscale;

	// New position
	Vector2 targetpos;
	bool invis;

	// Asteroid Collision
	Vector2 colvec;
	bool incol;

	// Use this for initialization
	void Start ()
	{
		incol = false;
		invis = false;

		mainstate = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		curasteroidstate = GetComponentInParent<AsteroidManager> ();
		curRig = GetComponent<Rigidbody2D> ();

		// Scaling
		// Get main camera component
		main = Camera.main;
		// Main camera height and width
		camheight = main.orthographicSize * 2;
		camwidth = camheight * main.aspect;
		// Set temp to ship sprite
		temp = GetComponent<SpriteRenderer> ().sprite;
		// How big bg1 is in world units
		unitWidth = temp.textureRect.width / temp.pixelsPerUnit;
		//unitHeight = temp.textureRect.height / temp.pixelsPerUnit;

		// Asteroid Details
		randomscale = Random.Range (7, 12);
		speed.x = 0;
		speed.y = Random.Range (curasteroidstate.speedmin, curasteroidstate.speedmax);
		turnspeed = Random.Range (-4, 4);
		
		// Scale ship according to camera size
		transform.localScale = new Vector3 (camwidth / (unitWidth * randomscale), camwidth / (unitWidth * randomscale));

		//Asteroid Initial speeds
		curRig.velocity = speed;
		curRig.AddTorque (turnspeed);
	}

	void OnBecameInvisible ()
	{
		// Move asteroid to start at random x position when invisible
		if (!invis) {
			if (mainstate.curState == GameController.gameState.Lightspeed) {
				Destroy (gameObject);
			}

			StartCoroutine (newPos ());
			invis = true;
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		// Behaviour when asteroids collide with eachother
		if (other.gameObject.tag == "Asteroid" && !incol) {
			colvec = curRig.velocity.normalized;
			curRig.velocity = (colvec + speed) * 1.05f;
			curRig.AddTorque (-turnspeed * 2f);
			incol = true;
		}
		// Gamestate = End when asteroid collides with ship
		if (other.gameObject.tag == "Ship" && mainstate.curState != GameController.gameState.End) {
			mainstate.curState = GameController.gameState.End;
			//Debug.Log (mainstate.curState);
		}
	}

	void OnCollisionExit2D (Collision2D other)
	{
		// incol used to prevent multiple collisions
		if (other.gameObject.tag == "Asteroid" && incol) {
			incol = false;
		}
	}

	// Determine new position for asteroid
	IEnumerator newPos ()
	{
		yield return new WaitForSeconds (Random.Range (0f, 2f));

		targetpos.x = Random.Range (-main.orthographicSize * main.aspect + 0.12f, main.orthographicSize * main.aspect - 0.12f);
		targetpos.y = main.orthographicSize + 0.3f;
		transform.position = targetpos;
		                                
		// Asteroid Details, random speed, random size
		randomscale = Random.Range (7, 12);
		speed.x = 0;
		speed.y = Random.Range (curasteroidstate.speedmin, curasteroidstate.speedmax);
		turnspeed = Random.Range (-4, 4);
		                                
		// Randomly scale asteroid
		transform.localScale = new Vector3 (camwidth / (unitWidth * randomscale), camwidth / (unitWidth * randomscale));
		                                
		//Asteroid Initial speeds
		curRig.velocity = speed;
		curRig.AddTorque (turnspeed);
		
		// asteriod comes into view again
		invis = false;

		yield break;
	}
}
