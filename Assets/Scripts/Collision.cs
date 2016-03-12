﻿using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerMovement>().Take_Damage();
			Debug.Log (col);

		}
	}
}
