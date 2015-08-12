using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {
	// Lose live when health reaches 0.
	// 100 health points.
	int current_health=100;
	// Fight until 2 lives are gone on either side.  Lost live when health reaches 0.
	int lives=2;
	public GameObject prefab;
	

	void Lose_Life(){
		lives -= 1;
	}
	void Reset_Lives(){
		lives = 2;
	}
	void Reset_health(){
		current_health = 100;
		
	}
	void Take_Damage(int damage = 1){
		current_health -= damage;
		if (current_health <= 0) {
			Lose_Life();
		}
	}


}
