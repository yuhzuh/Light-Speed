using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedManager : MonoBehaviour
{
	// Current active MovementController object
	MovementController curmctrl;
	// Current active LSObjManager Object
	public LSObjManager curls;
	float speed;

	public Image Bg;
	public Image Fill;

	Slider curslider;

	// Use this for initialization
	void Start ()
	{
		curmctrl = GameObject.FindGameObjectWithTag ("Player").GetComponent<MovementController> ();
		curslider = GetComponent<Slider> ();
		
		// Retrieve speed from MovementController
		speed = curmctrl.speed;

		StartCoroutine (Ongoing ());
	}

	IEnumerator Ongoing ()
	{
		while (GameController.maingame.curState != GameController.gameState.Ongoing) {
			yield return null;
		}
		// Continuously update slider value according to MovementController speed, 
		while (GameController.maingame.curState == GameController.gameState.Ongoing) {
			speed = curmctrl.speed;
			curslider.value = speed;
			yield return null;
		}
		yield return new WaitForSeconds (5f);
		// When gamestate enters Lightspeed, start ColorFadeLS
		StartCoroutine (ColorFadeLS ());
		yield break;
	}

	// Slider bg changes to alphaed yellow, while fill changes to blue
	IEnumerator ColorFadeLS ()
	{
		Color newbg = Fill.color;
		Color fillog = Fill.color;
		Color newfill = Color.blue;
		newfill.a = Fill.color.a;

		newbg.a = Bg.color.a;
		Bg.color = newbg;

		curslider.minValue = 0.1f;
		curslider.maxValue = 0.7f;

		speed = curls.speed;
		float f = 0;
		
		// Slider value decreases along with slider color change
		for (float i = 0.5f; i > 0.1f || f < 0.5f; i -= 0.005f) {
			curslider.value = i;
			Fill.color = Color.Lerp (fillog, newfill, f / 0.5f);
			f += 0.005f;
			yield return null;
		}
		
		// Set slider value to LSObjManager speed
		while (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			curslider.value = curls.speed;
			yield return null;
		}
		yield break;
	}
}
