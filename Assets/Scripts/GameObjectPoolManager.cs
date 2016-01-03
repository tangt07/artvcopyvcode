using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolManager : MonoBehaviour {

	public static GameObjectPoolManager current;

	public int numberprojectiles;

	public List<GameObject> genericprojectilepool = new List<GameObject> ();

	public GameObject genericprojectileprefab;



	// Use this for initialization
	void Start () {
		current = this;


		for (int i = 0; i < numberprojectiles; i++) {
			GameObject obj = (GameObject) Instantiate (genericprojectileprefab);
			obj.SetActive (false);
			obj.name = genericprojectileprefab.name;
			genericprojectilepool.Add (obj);
		}

	}
	
	public GameObject GetPooledObject(){
		for (int i = 0; i < genericprojectilepool.Count; i++) {
			if (!genericprojectilepool [i].activeInHierarchy) {
				return genericprojectilepool [i];
			}
		}


		return null;

		
	}
}
