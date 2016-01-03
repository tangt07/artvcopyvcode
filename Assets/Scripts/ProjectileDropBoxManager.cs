using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileDropBoxManager : MonoBehaviour {
	public static ProjectileDropBoxManager current;
	public GameObject[] boxlist;
	public List<Vector3> startpositions = new List<Vector3>();

	// Use this for initialization
	void Start () {
		current = this;
		StoreInitialPositions ();
		SetGroupActive (false);
	}

	void StoreInitialPositions(){

		for (int i = 0; i < boxlist.Length; i++) {
			current.startpositions.Add ((Vector3) boxlist [i].transform.position);
		
		}
	}
	public void DropBoxes(){
		for (int i = 0; i < current.boxlist.Length; i++) {
			current.boxlist [i].transform.position = current.startpositions [i];
			SetGroupActive (true);
		}
	}
	public void SetGroupActive(bool active){
		for (int i = 0; i < current.boxlist.Length; i++) {
			current.boxlist [i].SetActive (active);
		}
	}
}
