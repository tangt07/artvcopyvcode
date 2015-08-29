using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	// Lose live when health reaches 0.
	// 100 health points.
	public int current_health=100;
	// Fight until 2 lives (rounds) are gone on either side.  Lost life (round) when health reaches 0.
	public int lives=2;
	Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
		anim.SetInteger ("Health", current_health);
	}




	public void Lose_Life(){
		lives -= 1;
	}
	public void Reset_Lives(){
		lives = 2;
	}
	public void Reset_health(){
		current_health = 100;
		
	}
	public void Take_Damage(int damage = 1){
		current_health -= damage;

		anim.SetInteger ("Health", current_health);
		anim.SetTrigger ("Damage");

		if (current_health <= 0) {
			Lose_Life();
		}
	}


}
