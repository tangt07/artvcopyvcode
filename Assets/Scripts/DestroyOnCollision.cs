using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour {
	bool damage;



	void OnTriggerEnter2D(Collider2D col){
		PlayerMovement other = col.gameObject.GetComponent<PlayerMovement>();
		if (col.transform.root != transform.root) {

			if (col.gameObject.tag == "Player") {

				if (!other.shieldup) {

					col.gameObject.GetComponent<PlayerHealth>().Take_Damage ();
				}


			}

		}
		if (gameObject.tag == "Projectile") {
			Destroy (gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D col){

	}
}
