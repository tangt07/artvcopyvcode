/*
 (C) 2015
 your R&D lab
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Cutscene:MonoBehaviour
{
	public static Cutscene current;
	RawImage rim;
	MovieTexture mt;
	RectTransform rt;
	Vector2 origPos;
	AudioSource audiosource;

	void Awake()
	{
		current = this;
		audiosource = GetComponent<AudioSource> ();
		rt = GetComponent<RectTransform>();
		rim = GetComponent<RawImage>();
		origPos = rt.anchoredPosition;
		rim.enabled = false;
		mt = (MovieTexture)rim.mainTexture;
	}
	public void StopCutscene(){
		rim.enabled = false;
		mt.Stop();
	}
	public void StartCutscene(){
		rim.enabled = true;
		mt.Stop();
		mt.Play();
		audiosource.Play ();
	}
	/*Testing 
	void Update()
	{

		if (Input.GetButtonDown("Jump"))
		{
			
			if (mt.isPlaying)
			{
				StopCutscene ();
			}
			else
			{
				StartCutscene ();
			}
		}

		if (Input.GetKeyDown(KeyCode.W))
			rt.Translate(0f, 1f, 0f);
		if (Input.GetKeyDown(KeyCode.X))
			rt.Translate(0f, -1f, 0f);

		if (Input.GetKeyDown(KeyCode.A))
			rt.Translate(-1f, 0f, 0f);
		if (Input.GetKeyDown(KeyCode.D))
			rt.Translate(1f, 0f, 0f);

		if (Input.GetKeyDown(KeyCode.S))
		{
			rt.anchoredPosition = origPos;
			rt.eulerAngles = Vector3.zero;
			rt.localScale = new Vector3( 1f, 1f, 1f );
		}

		if (Input.GetKeyDown(KeyCode.Q))
			rt.Rotate(0f, 0f, .25f);
		if (Input.GetKeyDown(KeyCode.E))
			rt.Rotate(0f, 0f, -.25f);

		if (Input.GetKeyDown(KeyCode.Z))
		{
			float scaleNow = rt.localScale.x;
			if (scaleNow < 0.5f ) return;
			scaleNow = scaleNow - 0.01f;
			rt.localScale = new Vector3( scaleNow, scaleNow, 1f );
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			float scaleNow = rt.localScale.x;
			if (scaleNow > 1.5f ) return;
			scaleNow = scaleNow + 0.01f;
			rt.localScale = new Vector3( scaleNow, scaleNow, 1f );
		}

	}*/
}