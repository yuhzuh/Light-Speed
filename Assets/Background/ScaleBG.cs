using UnityEngine;
using System.Collections;

public class ScaleBG : MonoBehaviour
{
	GameController mainstate;

	// Store the background prefabs
	public GameObject bg1;
	public GameObject bg2;
	public GameObject backup;

	//Store planets
	public GameObject planet1, planet2;
	float planetspeed = 0.005f;
	float spinspeed = 0.05f;

	// Store lightspeed prefab
	public GameObject lsbg;
	SpriteRenderer lsSprite;
	ParticleSystem lspart;

	// Main camera
	Camera main;

	// Camera height and width
	float camheight, camwidth;

	//Camera details
	float xMin, xMax, yMaxPlanet, yMinPlanet;

	// Store background1 sprite renderer, and sprite
	SpriteRenderer bg1sprite;
	Sprite bg1spritetemp;
	Color lsOrigColor;

	// Storage for new bg1 position
	Vector2 position;

	// Min y coordinate of main camera
	float camend;
	// Min y coordinate of bg1 sprite
	float spriteend;
	// The difference between camend and spriteend;
	float offset;

	// Referenced in ScrollBG script
	public float spritelen;
	public float scrollspd;

	Vector2 bg2pos;

	void Awake ()
	{
		// Get main camera component
		main = Camera.main;
		// Create bg1 and set initial position as V(0,0)
		bg1 = (GameObject)Instantiate (bg1, Vector2.zero, Quaternion.identity);
		
		// Main camera height and width
		camheight = main.orthographicSize * 2;
		camwidth = camheight * main.aspect;
		
		bg1sprite = bg1.GetComponent<SpriteRenderer> ();
		bg1spritetemp = bg1sprite.sprite;
		
		// How big bg1 is in world units
		float unitWidth = bg1spritetemp.textureRect.width / bg1spritetemp.pixelsPerUnit;
		float unitHeight = bg1spritetemp.textureRect.height / bg1spritetemp.pixelsPerUnit;
		
		// Scale bg1 according to camera size
		bg1.transform.localScale = new Vector3 (camwidth / unitWidth + 0.1f, camheight / unitHeight + 0.1f);
		
		
		camend = main.transform.position.y - main.orthographicSize;
		spriteend = bg1sprite.bounds.min.y;
		offset = camend - spriteend;
		
		// Set new position
		position.x = 0;
		position.y = offset;
		
		// Apply new position to bg1
		bg1.transform.position = position;
		bg1.transform.parent = transform;
		
		spritelen = 2 * (bg1.transform.position.y - bg1sprite.bounds.min.y);
		
		// Set bg2 position;
		bg2pos.x = bg1.transform.position.x;
		bg2pos.y = bg1.transform.position.y + spritelen;
		
		// Instantiate bg2
		bg2 = (GameObject)Instantiate (bg2, bg2pos, Quaternion.identity);
		bg2.transform.localScale = bg1.transform.localScale;
		bg2.transform.parent = transform;

		// Instantiate backup
		backup = (GameObject)Instantiate (backup, Vector2.zero, Quaternion.identity);
		backup.transform.localScale = bg1.transform.localScale;

		//Camera details
		xMax = main.orthographicSize * main.aspect - 1f;
		xMin = -xMax;
		yMaxPlanet = main.orthographicSize + 1.7f;
		yMinPlanet = -yMaxPlanet;

		// Setup planets for parallax effect
		planet1 = (GameObject)Instantiate (planet1, new Vector2 (Random.Range (xMin, xMax), yMaxPlanet), Quaternion.identity);
		bg1sprite = planet1.GetComponent<SpriteRenderer> ();
		bg1spritetemp = bg1sprite.sprite;
		planet1.transform.localScale = new Vector3 (camwidth / (unitWidth * 0.3f), camwidth / (unitWidth * 0.3f));

		planet2 = (GameObject)Instantiate (planet2, new Vector2 (Random.Range (xMin, xMax), yMaxPlanet), Quaternion.identity);
		planet2.transform.localScale = planet1.transform.localScale;



	}

	// Use this for initialization
	void Start ()
	{
		mainstate = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		// Initial scroll speed
		scrollspd = 0.3f;//0.3f;
		// Change speed as game progresses
		StartCoroutine (changespd ());
		StartCoroutine (lightspd ());
		StartCoroutine (MovePlanet ());

	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	IEnumerator changespd ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}
		while (mainstate.curState == GameController.gameState.Ongoing) {
			//Debug.Log ("scrollspd: " + scrollspd);
			yield return new WaitForSeconds (5f);
			// Check again;
			if (mainstate.curState == GameController.gameState.Ongoing) {
				scrollspd += 0.2f;
			}
		}
		yield break;
	}

	// Initialiate lightspeed anim
	IEnumerator lightspd ()
	{
		lsbg = (GameObject)Instantiate (lsbg, position, Quaternion.identity);
		lsSprite = lsbg.GetComponent<SpriteRenderer> ();
		lsOrigColor = lsSprite.color;
		lsOrigColor.a = 0;
		lsSprite.color = lsOrigColor;

		// How big bg1 is in world units
		float unitWidth = lsSprite.sprite.textureRect.width / lsSprite.sprite.pixelsPerUnit;
		float unitHeight = lsSprite.sprite.textureRect.height / lsSprite.sprite.pixelsPerUnit;
		
		// Scale bg1 according to camera size
		lsbg.transform.localScale = new Vector3 (camwidth / unitWidth + 0.1f, camheight / unitHeight);

		/*camend = main.transform.position.y - main.orthographicSize;
		spriteend = lsSprite.bounds.min.y;
		offset = camend - spriteend;*/
		
		// Set new position
		position.x = 0;
		position.y = 0;

		//Position LSBG
		lsbg.transform.position = position;
		lsbg.transform.parent = transform;

		lspart = lsbg.transform.GetChild (0).GetComponent<ParticleSystem> ();
		lspart.enableEmission = false;

		while (mainstate.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
		if (mainstate.curState == GameController.gameState.Lightspeed) {

			lspart.enableEmission = true;	//Start bg particles

			while (scrollspd < 50) {
				Color tmp = lspart.startColor;
				scrollspd += 0.2f;
				if (scrollspd > 40) {
					lsOrigColor.a += 0.007f;
					lsSprite.color = lsOrigColor;
				}
				if (lspart.startSpeed < 2) {
					lspart.startSpeed += 0.01f;
				}
				if (lspart.startColor.r < 1) {
					tmp.r += 0.006f;
					lspart.startColor = tmp;
				}
				yield return null;
			}

			while (lsSprite.color.a < 1) {
				lsOrigColor.a += 0.007f;
				lsSprite.color = lsOrigColor;
				yield return null;
			}
			scrollspd = 0;
		}
		while (mainstate.curState == GameController.gameState.Lightspeed) {
			yield return null;
		}
	}

	IEnumerator MovePlanet ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}

		Vector2 movement;
		Vector3 spin = new Vector3 (0, 0, spinspeed);

		while (planet1.transform.position.y > yMinPlanet) {
			movement = planet1.transform.position;
			movement.y -= planetspeed;
			planet1.transform.Rotate (spin);
			planet1.transform.position = movement;
			yield return null;
		}
		while (planet2.transform.position.y > yMinPlanet) {
			movement = planet2.transform.position;
			movement.y -= planetspeed;
			planet2.transform.Rotate (-spin);
			planet2.transform.position = movement;
			yield return null;
		}
		yield break;
	}
}
