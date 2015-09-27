using UnityEngine;
using System.Collections;

public class LSObjManager : MonoBehaviour
{
	Camera main;
	public GameObject lsobj;
	public float xMax, xMin, yMax, yMin;
	public float speed;
	float newspeed;
	int curObjCount;

	// Use this for initialization
	void Start ()
	{
		main = Camera.main;
		xMax = main.aspect * main.orthographicSize + main.transform.position.x;
		xMin = -xMax;
		yMax = main.transform.position.y + main.orthographicSize;
		yMin = main.transform.position.y - main.orthographicSize;

		curObjCount = 0;
		speed = 0.1f;

		StartCoroutine (makeLSObj ());
		StartCoroutine (SpeedUp ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	IEnumerator makeLSObj ()
	{
		while (GameController.maingame.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
		if (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			yield return new WaitForSeconds (5f);
		}
		while (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			if (curObjCount < 8) {
				lsobj = (GameObject)Instantiate (lsobj, Vector2.zero, Quaternion.identity);
				lsobj.transform.SetParent (transform, true);
				curObjCount++;
				yield return new WaitForSeconds (5f);
			} else {
				yield return null;
			}
		}
		while (GameController.maingame.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
	}

	IEnumerator SpeedUp ()
	{
		while (GameController.maingame.curState != GameController.gameState.Lightspeed) {
			yield return null;
		}
		if (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			yield return new WaitForSeconds (5f);
		}
		while (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			while (speed < 0.7) {
				yield return new WaitForSeconds (10f); 
				newspeed = speed + 0.025f;
				while (speed < newspeed) {
					speed += 0.0005f;
					yield return null;
				}
			}
			yield return null;
		}
	}
}
