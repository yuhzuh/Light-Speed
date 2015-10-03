using UnityEngine;
using System.Collections;

public class LSObjMovement : MonoBehaviour
{
	// bullet object components
	Transform bullet;
	SpriteRenderer bulletsr;
	// bullet movement
	int bpos;
	Vector2 bcur;

	LineRenderer lr;
	
	// Active LSObjManager object
	LSObjManager par;
	float xMax, xMin, yMax, yMin;

	// Line renderer animation
	Vector2 start, end;
	Vector2 cur;
	Vector2 direction;

	// Line and bullet start/end positions
	int startpos;
	int endpos;

	// Non-alpha color and alpha ver of that color
	Color curcolor;
	Color clearcolor;
	//************************

	// Bullet move speed
	float speed;
	// LineRenderer fade out speed
	float linespeed;

	// Use this for initialization
	void Start ()
	{
		par = GetComponentInParent<LSObjManager> ();
		bullet = GetComponentInChildren<Transform> ();
		bulletsr = GetComponentInChildren<SpriteRenderer> ();
		
		// Scale Bullet based on screen size
		float unitWidth = bulletsr.sprite.textureRect.width / bulletsr.sprite.pixelsPerUnit;
		float camwidth = Camera.main.aspect * 2 * Camera.main.orthographicSize;
		bullet.transform.localScale = new Vector3 (camwidth / (unitWidth * 2f), camwidth / (unitWidth * 2f));

		lr = GetComponent<LineRenderer> ();
		// Make objects visible by setting it to upper layer
		lr.sortingLayerName = "Obstacles";
		// Line renderer details
		lr.SetWidth (0.05f, 0.05f);

		clearcolor = Color.white;
		clearcolor.a = 0;
		
		// Retrieve camera dimensions from parent
		xMax = par.xMax;
		xMin = par.xMin;
		yMax = par.yMax;
		yMin = par.yMin;

		// Initial line fade speed
		linespeed = 0.15f;

		StartCoroutine (newPos ());
		StartCoroutine (linespeedUp ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		speed = par.speed;
	}
	
	// New position for LSObj after bullet leaves screen
	IEnumerator newPos ()
	{
		while (GameController.maingame.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
		while (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			// 3 different position states for lsobj to appear, chosen randomly
			startpos = Random.Range (0, 2);

			// specific target for 3 left, middle, right
			if (startpos == 0) {
				start.x = xMin;
				start.y = Random.Range (yMin + 1, yMax);
				endpos = Random.Range (0, 1);
				if (endpos == 1) {
					end.x = Random.Range (0, xMax);
					end.y = yMin;
				} else {
					end.x = xMax;
					end.y = Random.Range (yMin, yMax);
				}
			} else if (startpos == 1) {
				start.x = Random.Range (xMin + 1, xMax - 1);
				start.y = yMax;
				endpos = Random.Range (0, 2);
				if (endpos == 0) {
					end.x = xMin;
					end.y = Random.Range (yMax - 2, yMin);
				} else if (endpos == 1) {
					end.x = Random.Range (xMin, xMax);
					end.y = yMin;
				} else {
					end.x = xMax;
					end.y = Random.Range (yMax - 2, yMin);
				}
			} else {
				start.x = xMax;
				start.y = Random.Range (yMin + 1, yMax);
				endpos = Random.Range (0, 1);
				if (endpos == 1) {
					end.x = Random.Range (yMin, 0);
					end.y = yMin;
				} else {
					end.x = xMin;
					end.y = Random.Range (yMin, yMax);
				}
			}
	
			// 
			lr.SetColors (Color.white, Color.white);

			direction = (end - start).normalized;
			end += direction.normalized;
			start -= direction.normalized;
			cur = start;

			// bullet info
			bullet.position = start;
			bcur = start;
			
			// Set start position for lsobj
			lr.SetPosition (0, start);
			
			// Movement from start to end for linerenderer
			if (start.y > end.y) {

				//bullet pos that was chosen earlier
				bpos = 0;

				while (cur.y > end.y) {
					lr.SetPosition (1, cur);
					cur += direction * linespeed;
					yield return null;
				}
			} else if (start.y < end.y) {

				bpos = 1;

				while (cur.y < end.y) {
					lr.SetPosition (1, cur);
					cur += direction * linespeed;
					yield return null;
				}
			} else {
				if (start.x > end.x) {

					bpos = 2;

					while (cur.x > end.x) {
						lr.SetPosition (1, cur);
						cur += direction * linespeed;
						yield return null;
					}
				} else {

					bpos = 3;

					while (cur.x < end.x) {
						lr.SetPosition (1, cur);
						cur += direction * linespeed;
						yield return null;
					}
				}
			}
			
			curcolor = Color.white;
			curcolor.a = 1;
			// Color fade in/out
			while (curcolor.a > 0) {
				lr.SetColors (curcolor, Color.white);
				curcolor.a -= 0.05f;
				yield return null;
			}
			curcolor = Color.white;
			while (curcolor.a > 0) {
				lr.SetColors (clearcolor, curcolor);
				curcolor.a -= 0.05f;
				yield return null;
			}

			//bullet movement
			if (bpos == 0) {
				while (bcur.y > end.y) {
					bullet.position = bcur;
					bcur += direction.normalized * speed;
					yield return null;
				}
			} else if (bpos == 1) {
				while (bcur.y < end.y) {
					bullet.position = bcur;
					bcur += direction.normalized * speed;
					yield return null;
				}
			} else if (bpos == 2) {
				while (bcur.x > end.x) {
					bullet.position = bcur;
					bcur += direction.normalized * speed;
					yield return null;
				}
			} else {
				while (bcur.x < end.x) {
					bullet.position = bcur;
					bcur += direction.normalized * speed;
					yield return null;
				}
			}
		}

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// Gamestate changes to end when bullet touches ship object
		if (other.gameObject.tag == "Ship" && GameController.maingame.curState != GameController.gameState.End) {
			GameController.maingame.curState = GameController.gameState.End;
		}
	}
	
	// Speed up line fade out speed
	IEnumerator linespeedUp ()
	{
		while (GameController.maingame.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
		// Speed only increases during Lightspeed gamestate
		if (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			yield return new WaitForSeconds (10f);
			while (linespeed < 0.7f) {
				yield return new WaitForSeconds (10f);
				linespeed += 0.025f;
			}
		}
		yield break;
	}
}
