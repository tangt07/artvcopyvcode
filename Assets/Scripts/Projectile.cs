using UnityEngine;
using System.Collections;

public struct Projectile  {
	public GameObject go;
	public Facing facing;

	public Projectile(GameObject g, Facing f){
		go = g;
		facing = f;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
