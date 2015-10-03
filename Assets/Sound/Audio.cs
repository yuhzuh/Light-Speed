using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
	public static Audio mainbgm;


	void Awake ()
	{
		// If bgm object exists, don't load new one, else create new object
		if (mainbgm == null) {
			DontDestroyOnLoad (gameObject); //this gameobject will persist from scene to scene
			mainbgm = this;
		} else if (mainbgm != this) {
			Destroy (gameObject);
		}
	}
}
