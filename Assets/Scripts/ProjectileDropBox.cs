using UnityEngine;
using System.Collections;

public class ProjectileDropBox : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerMovement> ().numprojectiles++;
			gameObject.SetActive (false);

		}
	}

}
