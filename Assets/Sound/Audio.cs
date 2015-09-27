using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
	public static Audio mainbgm;


	void Awake ()
	{
		if (mainbgm == null) {
			DontDestroyOnLoad (gameObject); //this gameobject will persist from scene to scene
			mainbgm = this;
		} else if (mainbgm != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
