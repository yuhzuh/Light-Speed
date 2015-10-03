using UnityEngine;
using UnityEditor;
using System.Collections;

public class ResetPrefs : MonoBehaviour
{
	// Reset PlayerPreferences
	[MenuItem("Edit/Reset Playerprefs")]
	public static void DeletePlayerPrefs ()
	{
		PlayerPrefs.DeleteAll ();
	}
}
