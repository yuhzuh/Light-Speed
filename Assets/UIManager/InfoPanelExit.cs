using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanelExit : MonoBehaviour
{
	Button exitbutton;

	public GameObject infopanel;
	GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = (GameObject)GameObject.FindGameObjectWithTag ("Player");

		exitbutton = GetComponent<Button> ();
		exitbutton.onClick.AddListener (() => {
			ExitPanel ();});
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Exit info panel on back button press
		if (Input.GetKeyDown (KeyCode.Escape)) {
			player.SetActive (true);
			infopanel.SetActive (false);
		}
	}

	// Re enable player gameobject
	void ExitPanel ()
	{
		player.SetActive (true);
		infopanel.SetActive (false);
	}
}
