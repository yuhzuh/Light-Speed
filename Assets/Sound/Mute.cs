using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mute : MonoBehaviour
{
	AudioSource bgm;
	public static Mute mutecur;
	Button mute;
	Text text;
	float newvol;

	int muted;
	bool clicked;

	// Use this for initialization
	void Start ()
	{
		bgm = Audio.mainbgm.GetComponent<AudioSource> ();
		if (mutecur == null) {
			DontDestroyOnLoad (gameObject); //this gameobject will persist from scene to scene
			mutecur = this;
		} else if (mutecur != this) {
			Destroy (gameObject);
		}

		mute = GetComponent<Button> ();
		mute.onClick.AddListener (() => {
			VolControl ();});

		muted = StatsManager.stats.muted;

		text = mute.GetComponentInChildren<Text> ();
		if (muted == 1) {
			text.text = "UNMUTE";
			bgm.volume = 0;
		} else {
			text.text = "MUTE";
			bgm.volume = 0.7f;
		}

		clicked = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void VolControl ()
	{
		if (muted == 0 && clicked == false) {
			StartCoroutine (fadeOut ());
			text.text = "UNMUTE";
			muted = 1;
			clicked = true;
		} else if (muted == 1 && clicked == false) {
			StartCoroutine (fadeIn ());
			text.text = "MUTE";
			muted = 0;
			clicked = true;
		}
		StatsManager.stats.muted = muted;
		StatsManager.stats.Save ();
	}

	IEnumerator fadeOut ()
	{
		newvol = bgm.volume;
		while (bgm.volume > 0) {
			newvol -= 0.5f * Time.deltaTime;
			bgm.volume = newvol;
			yield return null;
		}
		clicked = false;
		yield break;
	}

	IEnumerator fadeIn ()
	{
		newvol = bgm.volume;
		while (bgm.volume < 0.7f) {
			newvol += 0.5f * Time.deltaTime;
			bgm.volume = newvol;
			yield return null;
		}
		clicked = false;
		yield break;
	}
}
