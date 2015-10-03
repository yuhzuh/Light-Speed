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
		// Audio component
		bgm = Audio.mainbgm.GetComponent<AudioSource> ();
		
		// Check if a mute gameobject exists, if so delete this gameobject
		if (mutecur == null) {
			DontDestroyOnLoad (gameObject); //this gameobject will persist from scene to scene
			mutecur = this;
		} else if (mutecur != this) {
			Destroy (gameObject);
		}
		
		// UI button components
		mute = GetComponent<Button> ();
		// Add listener to button
		mute.onClick.AddListener (() => {
			VolControl ();});
		// Retrieve past mute info from persistent data
		muted = StatsManager.stats.muted;
		
		// Initial button text according to previous game data
		text = mute.GetComponentInChildren<Text> ();
		if (muted == 1) {
			text.text = "UNMUTE";
			bgm.volume = 0;
		} else {
			text.text = "MUTE";
			bgm.volume = 0.7f;
		}
		// Disallow multiple clicks until sound fully fades in or out
		clicked = false;
	}
	
	public void VolControl ()
	{
		// If button is unmuted, fade vol out to mute, vice versa
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

	// Volume fade out
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
	
	// Volume fade in
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
