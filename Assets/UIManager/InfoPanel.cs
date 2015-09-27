using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanel : MonoBehaviour
{
	Button infobutton;

	public GameObject infopanel;
	GameObject player;
	
	// Use this for initialization
	void Start ()
	{
		player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
		infobutton = GetComponent<Button> ();
		infobutton.onClick.AddListener (() => {
			StartPanel ();});

	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void StartPanel ()
	{
		infopanel.SetActive (true);
		player.SetActive (false);
	}
}
