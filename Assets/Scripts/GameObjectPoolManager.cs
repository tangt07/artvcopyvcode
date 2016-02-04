using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolManager : MonoBehaviour {

	public static GameObjectPoolManager current;

	public int numberobjects;

	public List<GameObject> genericpool = new List<GameObject> ();

	public GameObject genericprefab;



	// Use this for initialization
	void Start () {
		current = this;


		for (int i = 0; i < numberobjects; i++) {
			GameObject obj = (GameObject) Instantiate (genericprefab);
			obj.SetActive (false);
			obj.name = genericprefab.name;
			genericpool.Add (obj);
		}

	}
	
	public GameObject GetPooledObject(){
		for (int i = 0; i < genericpool.Count; i++) {
			if (!genericpool [i].activeInHierarchy) {
				return genericpool [i];
			}
		}


		return null;

		
	}
}
