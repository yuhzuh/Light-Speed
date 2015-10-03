using UnityEngine;
//using UnityEditor;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class StatsManager : MonoBehaviour
{
	public static StatsManager stats;
	
	// Variables to be saved persistently
	public int highscore;
	public int gamesplayed;
	public int muted;

	void Awake ()
	{
		// Don't destroy object on load
		if (stats == null) {
			DontDestroyOnLoad (gameObject); //this gameobject will persist from scene to scene
			stats = this;
		} else if (stats != this) {
			Destroy (gameObject);
		}
		Load (); //Muted didn't work until I put it here, needs to load variables before Mute object initiates for retrieval of said variables
	}

	// Use this for initialization
	void Start ()
	{
		//Load ();
	}

	// Save data into persistentDataPath
	public void Save ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "lightSpeedStat.dat");

		StatData data = new StatData ();
		data.highscore = highscore;
		data.gamesplayed = gamesplayed;
		data.muted = muted;

		bf.Serialize (file, data);
		file.Close ();
	}

	// Load previous variables from exiting dat file, else create a new data file
	void Load ()
	{
		if (File.Exists (Application.persistentDataPath + "lightSpeedStat.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "lightSpeedStat.dat", FileMode.Open);

			StatData data = (StatData)bf.Deserialize (file);
			file.Close ();

			highscore = data.highscore;
			gamesplayed = data.gamesplayed;
			muted = data.muted;
		} else {
			highscore = 0;
			gamesplayed = 0;
			muted = 0;
		}
	}
	
	// Clear console
	public void ClearConsole ()
	{
		var logEntries = System.Type.GetType ("UnityEditorInternal.LogEntries,UnityEditor.dll");
		var clearMethod = logEntries.GetMethod ("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
		clearMethod.Invoke (null, null);
	}
}

[Serializable]
class StatData
{
	public int highscore;
	public int gamesplayed;
	public int muted;
}