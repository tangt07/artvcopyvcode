using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour {
	bool damage;



	void OnTriggerEnter2D(Collider2D col){
		PlayerMovement other = col.gameObject.GetComponent<PlayerMovement>();
		if (col.transform.root != transform.root) {

			if (col.gameObject.tag == "Player") {

				if (!other.shieldup) {
					if (gameObject.tag == "Projectile") {
						col.gameObject.GetComponent<PlayerMovement> ().Take_Damage (2.5f);
					} else {
						col.gameObject.GetComponent<PlayerMovement> ().Take_Damage ();

					}
					gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
					gameObject.SetActive (false);
				} else {
					if (gameObject.tag == "Projectile") {
						col.gameObject.GetComponent<PlayerMovement> ().Take_Damage (1.25f);
					} else {
						col.gameObject.GetComponent<PlayerMovement> ().Take_Damage (0);

					}
					gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
					gameObject.SetActive (false);
				}


			}

		}
		if (col.gameObject.tag == "Projectile") {
			if (gameObject.tag == "Projectile") {
				gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				gameObject.SetActive (false);
			}
		}
	}
	void OnTriggerExit2D(Collider2D col){

	}
}
