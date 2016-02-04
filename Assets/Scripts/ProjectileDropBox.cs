using UnityEngine;
using System.Collections;

public class ProjectileDropBox : MonoBehaviour {

	void OnTriggerStay2D(Collider2D other){
		
		if (other.gameObject.tag == "Player") {

			int currentprojectiles = other.gameObject.GetComponent<PlayerMovement> ().numprojectiles;

			if (currentprojectiles < 10) {
				other.gameObject.GetComponent<PlayerMovement> ().numprojectiles++;
				gameObject.SetActive (false);
			} 
		}
	}

}
