using UnityEngine;
using System.Collections;

public class ScrollBG : MonoBehaviour
{
	//Store main camera transform
	Transform cameraTransform;

	// Store spritelen from ScaleBG
	float spriteLength;

	// The position after shift
	Vector2 shiftPos;
	// The new position each frame
	Vector2 newPos;
	// Attached background transformation
	Transform me;
	// Min y coordinate point of camera
	float end;
	// Store sprite renderer component
	SpriteRenderer bgsr;

	ScaleBG par;

	void Awake ()
	{
	}

	// Use this for initialization
	void Start ()
	{
		par = GetComponentInParent<ScaleBG> ();
		spriteLength = par.spritelen;
		bgsr = GetComponent<SpriteRenderer> ();

		me = GetComponent<Transform> ();
		cameraTransform = Camera.main.transform;
		newPos = me.position;
		end = cameraTransform.position.y - Camera.main.orthographicSize;

	}
	
	// Update is called once per frame
	void Update ()
	{
		// New y position for newPos
		newPos.y -= par.scrollspd * Time.deltaTime;
		// Apply position
		me.position = newPos;
		// keep track of shiftPos
		shiftPos = me.position;

		// If max coordinate of sprite matches min coordinate of camera, shift the bg up 2x spritelen
		if (bgsr.bounds.max.y < end) {
			shiftPos.y += 2 * spriteLength;
			me.position = shiftPos;
			newPos = me.position;
		}
	}
}
