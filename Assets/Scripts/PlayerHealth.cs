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
		

	}




	public void Lose_Life(){
		setdead = true;

		lives -= 1;


	}
	public void Reset_Lives(){
		
		lives = 2;
	}
	public void Reset_health(){
		
		current_health = 100f;
		
	}

	IEnumerator Damage(){
		cooldown = false;
		while (cooldown) {
			yield return new WaitForSeconds(1);
		}


	}

}
