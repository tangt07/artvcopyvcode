﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameObjectPool {
	string poolname;
	GameObject poolobjectprefab;
	int numberofobjects;
	public GameObjectPool (string poolname, GameObject poolobjectprefab, int numberofobjects){
		this.poolname = poolname;
		this.poolobjectprefab = poolobjectprefab;
		this.numberofobjects = numberofobjects;
		for(int i = 0; i < numberofobjects; i++ ){
			GameObject obj = GameObject.Instantiate (poolobjectprefab) as GameObject;
		}
	}

}
 
