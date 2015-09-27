using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedManager : MonoBehaviour
{
	MovementController curmctrl;
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
		speed = curmctrl.speed;

		StartCoroutine (Ongoing ());
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	IEnumerator Ongoing ()
	{
		while (GameController.maingame.curState != GameController.gameState.Ongoing) {
			yield return null;
		}
		while (GameController.maingame.curState == GameController.gameState.Ongoing) {
			speed = curmctrl.speed;
			curslider.value = speed;
			yield return null;
		}
		yield return new WaitForSeconds (5f);
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

		for (float i = 0.5f; i > 0.1f || f < 0.5f; i -= 0.005f) {
			curslider.value = i;
			Fill.color = Color.Lerp (fillog, newfill, f / 0.5f);
			f += 0.005f;
			yield return null;
		}

		while (GameController.maingame.curState == GameController.gameState.Lightspeed) {
			curslider.value = curls.speed;
			yield return null;
		}
		yield break;
	}
}
