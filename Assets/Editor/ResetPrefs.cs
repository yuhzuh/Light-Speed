using UnityEngine;
using UnityEditor;
using System.Collections;

public class ResetPrefs : MonoBehaviour
{

	[MenuItem("Edit/Reset Playerprefs")]
	public static void DeletePlayerPrefs ()
	{
		PlayerPrefs.DeleteAll ();
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
