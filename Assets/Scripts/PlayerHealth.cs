using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	// Lose live when health reaches 0.
	// 100 health points.
	public bool dead = false;
	public bool setdead = false;
	public float current_health=100f;
	public bool cooldown;
	// Fight until 2 lives (rounds) are gone on either side.  Lost life (round) when health reaches 0.
	public int lives=2;
	Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
		anim.SetFloat ("Health", current_health);


	}
	void Update(){
		if ((!setdead && dead) || (setdead && !dead)) {
			dead = setdead;
			if (dead) {
				anim.SetTrigger ("Dead");
			}
		}


	}




	public void Lose_Life(){
		setdead = true;

		lives -= 1;


	}
	public void Reset_Lives(){
		
		lives = 2;
	}
	public void Reset_health(){
		setdead = false;
		current_health = 100f;
		
	}
	public void Take_Damage(float damage = 1f){

		current_health -= damage;
		if (!dead) {
			anim.SetFloat ("Health", current_health);
			anim.SetTrigger ("Damage");
		}		
		if (current_health <= 0 && !dead) {
			Lose_Life();
		}

	}
	IEnumerator Damage(){
		cooldown = false;
		while (cooldown) {
			yield return new WaitForSeconds(1);
		}


	}

}
