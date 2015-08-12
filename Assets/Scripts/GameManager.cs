using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject craigprefab;
	public GameObject amyprefab;
	public GameObject willprefab;
	public GameObject killerprefab;
	GameObject opponent1;
	GameObject opponent2;
	GameObject opponent3;
	int current_fight = 1;
	int current_round = 1;
	GameObject player1_prefab;
	GameObject player2_prefab;
	int opponent;
	int player1_wins=0;
	int player2_wins=0;
	public float timer=120f;
	public string timerFormatted;



	// Use this for initialization
	void Start () {
		if (Select.selectedPlayer == Select.Player.Craig) {
			player1_prefab = craigprefab;
			//opponent is either amy or will
			RandomOpponent(amyprefab,willprefab);


		}
		if (Select.selectedPlayer == Select.Player.Amy) {
			player1_prefab = amyprefab;
			//opponent is either craig or will
			RandomOpponent(craigprefab,willprefab);

		}
		if (Select.selectedPlayer == Select.Player.Will) {
			player1_prefab = willprefab;
			//opponent is either craig or amy
			RandomOpponent(craigprefab,amyprefab);
		}
		
		if (current_fight == 1) {
			player2_prefab = opponent1;
		}
		
		if (current_fight == 2) {
			player2_prefab = opponent2;
		}
		
		if (current_fight == 3) {
			player2_prefab = opponent3;
		}

		player1_prefab.transform.localPosition = new Vector3 (-3.25f, 0, 0);
		player1_prefab.transform.localScale = new Vector3 (10, 10, 1);
		player2_prefab.transform.localPosition = new Vector3 (3.25f, 0, 0);
		player2_prefab.transform.localScale = new Vector3 (-10, 10, 1);
		Instantiate(player1_prefab);
		Instantiate(player2_prefab);



	}
	void RandomOpponent(GameObject x,GameObject y){
		// Determines opponents for game
		int randomnumber = Random.Range (0,2);
		if (randomnumber == 0) {
			opponent1 = x;
			opponent2 = y;
		} else {
			opponent1 = y;
			opponent2 = x;
		}
		opponent3 = killerprefab;
	}
	void Restart(){
		//the fight starts over
	
	}
	void Quit(){

	}
	// Update is called once per frame
	void Update()
	{	timer -= Time.deltaTime;
		System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
		
		timerFormatted = string.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
	}
}
