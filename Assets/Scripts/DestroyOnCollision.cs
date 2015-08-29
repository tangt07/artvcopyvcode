using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){


		if (col.gameObject.tag == "Player") {
			if(!col.gameObject.GetComponent<PlayerMovement>().shieldup){

				col.gameObject.GetComponent<PlayerHealth>().Take_Damage();
			}

			Destroy (gameObject);
		}
		if (col.gameObject.tag == "Projectile") {
			Destroy (gameObject);
		}
	}
}
