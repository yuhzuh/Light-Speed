using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour
{
	GameController mainstate;

	Animator LineAnim;

	//LineRenderer cur;
	//BoxCollider2D curcol;

	//LinePoint details
	float unitWidth, unitHeight;
	SpriteRenderer tempsr;
	Sprite temps;

	// Camera height and width
	float camheight, camwidth;

	// Camera details
	Vector2 campos;
	Vector2 endpos;
	Vector2 startpos;
	Vector2 movepos;
	float /*xDist,*/ yDist, yMax, yMin;

	enum pointState
	{
		Stall,
		Go
	}
	;

	// Help move point
	pointState curState;

	// Collision detection
	public int curpoints;
	bool inside = false;


	// Use this for initialization
	void Start ()
	{
		mainstate = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		//curcol = GetComponent<BoxCollider2D> ();

		// Just in case check.
		transform.position = Vector2.zero;

		/*// Show Line Renderer in 2D by bringing it to foreground
		cur = GetComponent<LineRenderer> ();
		cur.sortingLayerName = "Obstacles";
		*/

		// Camera details
		campos = Camera.main.transform.position;
		//xDist = Camera.main.aspect * Camera.main.orthographicSize;
		yDist = Camera.main.orthographicSize;
		//xMax = campos.x + xDist;
		yMax = campos.y + yDist;
		yMin = campos.y - yDist;
		endpos = new Vector2 (campos.x, yMin - 0.25f);
		startpos = new Vector2 (campos.x, yMax + 0.25f);

		/*/ Set length of line
		cur.SetPosition (0, new Vector3 (xDist, 0, 0));
		cur.SetPosition (1, new Vector3 (-xDist, 0, 0));*/

		/*/ Set Collider size
		curcol.size = new Vector2 (xDist * 2, 0.2f);*/

		// Main camera height and width
		camheight = Camera.main.orthographicSize * 2;
		camwidth = camheight * Camera.main.aspect;
		
		tempsr = transform.GetChild (2).GetComponent<SpriteRenderer> ();
		temps = tempsr.sprite;
		
		// How big bg1 is in world units
		unitWidth = temps.textureRect.width / temps.pixelsPerUnit;
		unitHeight = temps.textureRect.height / temps.pixelsPerUnit;
		
		transform.localScale = new Vector3 (camwidth / (unitWidth * 4f), camheight / (unitHeight * 70));

		// Set position above camera
		transform.position = startpos;

		LineAnim = GetComponent<Animator> ();

		curState = pointState.Stall;
		StartCoroutine (pointGo ());

		curpoints = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Point goes down screen
		if (curState == pointState.Go) {
			movePoint ();
			if (transform.position.y <= endpos.y) {
				curState = pointState.Stall;
				LineAnim.SetBool ("Gained", false);
			}
		}
	}

	// Determines when the point goes down screen
	IEnumerator pointGo ()
	{
		while (mainstate.curState == GameController.gameState.Start) {
			yield return null;
		}
		while (mainstate.curState != GameController.gameState.End) {
			yield return new WaitForSeconds (5f);
			if (mainstate.curState != GameController.gameState.End) {
				transform.position = startpos;
				curState = pointState.Go;
				inside = false;
			} else {
				yield return null;
			}
		}
		yield break;
	}

	// Moves the point down the screen
	void movePoint ()
	{
		movepos = transform.position;
		movepos.y -= 5f * Time.deltaTime;
		transform.position = movepos;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Ship" && !inside && mainstate.curState != GameController.gameState.End) {
			curpoints++;
			//Debug.Log (curpoints);
			inside = true;
			LineAnim.SetBool ("Gained", true);
		}
	}
	
}
