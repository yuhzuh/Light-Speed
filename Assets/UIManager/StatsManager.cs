using UnityEngine;
//using UnityEditor;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class StatsManager : MonoBehaviour
{
	public static StatsManager stats;

	public int highscore;
	public int gamesplayed;
	public int muted;

	void Awake ()
	{
		if (stats == null) {
			DontDestroyOnLoad (gameObject); //this gameobject will persist from scene to scene
			stats = this;
		} else if (stats != this) {
			Destroy (gameObject);
		}
		Load (); //Muted didn't work until i put it here
	}

	// Use this for initialization
	void Start ()
	{
		//Load ();
	}

	void Update ()
	{
	}

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