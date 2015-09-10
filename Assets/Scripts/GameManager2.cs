using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager2 : MonoBehaviour {
	public GameObject craigprefab;
	public GameObject amyprefab;
	public GameObject willprefab;
	public GameObject killerprefab;
	public Text timertext;
	public Text fightcountertext;
	public Text restarttext;
	public Text kotext;
	public Image player1healthbar;
	public Image player2healthbar;
	RectTransform player1healthbarrt;
	RectTransform player2healthbarrt;
	
	PlayerHealth player1health;
	PlayerHealth player2health;
	
	public PlayerKeys player1keys;
	public PlayerKeys player2keys;
	
	int fightcounter;
	
	public int current_fight;
	public int current_round;
	GameObject player1;
	GameObject player2;
	GameObject opponent;
	PlayerName[] opponentList = new PlayerName[3];
	public int player1_wins;
	public int player2_wins;
	public float timer;
	public string timerFormatted;
	
	public Image pausepanel;
	public Button resumebutton;
	float waittime=0;
	bool aiprojectilekeydown;
	bool firstframe;
	bool nextframe;
	bool roundover;
	bool pause=false;
	bool resume = false;
	bool resumeset = false;
	PlayerMovement player1movement,player2movement;
	int numfights;
	public Image player11win;
	public Image player12win;
	
	public Image player21win;	
	public Image player22win;
	
	XDirection currentX = XDirection.None;
	XDirection nextX = XDirection.None;
	YDirection currentY = YDirection.None;
	YDirection nextY = YDirection.None;
	
	float distance;
	
	Animator player1anim;
	
	Animator player2anim;
	
	//need to pass Direction Facing and Attack to PlayerMovement Scripts
	
	private Dictionary<PlayerName,GameObject> _dicPlayerPrefabByName = null;
	
	
	
	
	void Awake(){
		_dicPlayerPrefabByName = new Dictionary<PlayerName, GameObject> ();
		_dicPlayerPrefabByName.Add (PlayerName.Craig, craigprefab);
		_dicPlayerPrefabByName.Add (PlayerName.Amy, amyprefab);
		_dicPlayerPrefabByName.Add (PlayerName.Will, willprefab);
		_dicPlayerPrefabByName.Add (PlayerName.Killer, killerprefab);
		
		
	}
	
	
	
	// Use this for initialization
	void Start () {
		if (Player1Select.player1name == PlayerName.None) {
			Player1Select.player1name = PlayerName.Craig;
		}
		if (Player2Select.player2name == PlayerName.None) {
			Player2Select.player2name = PlayerName.Will;
		}
		if (SceneManager.players == 0||SceneManager.players==2) {
			numfights = 1;
			SceneManager.players = 2;
		}
		if (SceneManager.players == 1) {
			numfights = 3;
			DetermineFightOpponents ();
		}

		StartCoroutine ("Pause");
		
		StartCoroutine ("Game");
		
		
		
		
		
		
	}
	
	
	void DetermineFightOpponents(){
		int randomnumber;
			
		List<PlayerName> keyList = new List<PlayerName> (_dicPlayerPrefabByName.Keys);
		

		
		keyList.Remove (PlayerName.None);
		keyList.Remove (PlayerName.Killer);
		keyList.Remove (Player1Select.player1name);
		//assign random name from remaining names
		int i = 0;
		while (keyList.Count > 0) {
			randomnumber = Random.Range (0, keyList.Count);
			opponentList [i] = keyList [randomnumber];
			keyList.Remove (keyList [randomnumber]);
			i++;
		}
		//last opponent is killer
		opponentList [i] = PlayerName.Killer;


	}
	void GetPlayer2AIInputs(){
		//AI
		if (nextframe) {
			nextframe = false;
			player2movement.currentAttack = Attack.None;
		}
		if (!nextframe) {
			if(distance > 3.5 && waittime > 1){
				waittime=0;
				player2movement.currentAttack = Attack.Projectile;
				nextframe=true;
				
			}
			else{
				//player2 continuously blocks
				if (player2movement.currentFacing == Facing.Left) {
					player2movement.currentDirection = Direction.RightDown;
				}
				if (player2movement.currentFacing == Facing.Right) {
					player2movement.currentDirection = Direction.LeftDown;
				}
			}
		}
		waittime += Time.deltaTime;
	}
	
	
	
	
	
	void FlipSprites(){
		//Debug.Log (player1_position.x);
		
		//Debug.Log (player2_position.x);
		if (player1.transform.localPosition.x <= player2.transform.localPosition.x) {
			player1.transform.localScale = new Vector3 (1, 1, 1);
			player1movement.currentFacing = Facing.Right;
			player2.transform.localScale = new Vector3 (-1, 1, 1);
			player2movement.currentFacing = Facing.Left;
		} 
		else {
			player1.transform.localScale = new Vector3 (-1, 1, 1);				
			player1movement.currentFacing = Facing.Left;
			player2.transform.localScale = new Vector3 (1, 1, 1);
			player2movement.currentFacing = Facing.Right;
			
		}
	}
	void InstantiatePlayer1(){
		player1 = Instantiate(_dicPlayerPrefabByName [Player1Select.player1name],new Vector3(-3f,-1f,0),Quaternion.identity) as GameObject;
	}
	void InstantiateOpponent(){
		if (SceneManager.players == 1) {
			player2 = Instantiate (_dicPlayerPrefabByName [opponentList [current_fight - 1]], new Vector3 (3f, -1f, 0), Quaternion.identity) as GameObject;
		}
		if (SceneManager.players == 2) {
			player2 = Instantiate(_dicPlayerPrefabByName[Player2Select.player2name],new Vector3 (3f, -1f, 0), Quaternion.identity) as GameObject;
		}
	}
	
	
	void SetUpPlayers(){
		player1health = player1.GetComponent<PlayerHealth> ();		
		player2health = player2.GetComponent<PlayerHealth> ();
		
		player1healthbarrt = player1healthbar.rectTransform;
		player2healthbarrt = player2healthbar.rectTransform;
		
		player1movement = player1.GetComponent<PlayerMovement> ();
		player2movement = player2.GetComponent<PlayerMovement> ();
		
		player1movement.thisPlayer = PlayerNumber.Player1;
		player2movement.thisPlayer = PlayerNumber.Player2;
		
		player1movement.thisCharacter = Player1Select.player1name;
		player2movement.thisCharacter = opponentList [current_fight - 1];
		
		player1anim = player1.GetComponent<Animator> ();
		player2anim = player2.GetComponent<Animator> ();
		
		player1anim.SetTrigger ("Restart");
		player2anim.SetTrigger ("Restart");
		
		FlipSprites ();
	}
	
	void ResetTimer(){
		timer = 120f;
	}
	void DisplayTimer(){
		System.TimeSpan t = System.TimeSpan.FromSeconds (timer);
		timerFormatted = string.Format ("{0}:{1:D2}", t.Minutes, t.Seconds);
		timertext.text = timerFormatted;
	}
	IEnumerator Game(){

		current_fight = 1;
		current_round = 1;
		player1_wins = 0;
		player2_wins = 0;
		player11win.enabled =false;
		player12win.enabled =false;
		player21win.enabled =false;
		player22win.enabled =false;


		firstframe = true;
		fightcountertext.enabled = false;
		restarttext.enabled = false;
		kotext.enabled = false;
		//Create player1 and player2
		InstantiatePlayer1 ();
		InstantiateOpponent ();
		
		//hook up scripts
		SetUpPlayers ();
		


		while (!(current_fight>numfights)) {
			
			ResetTimer();
			
			DisplayTimer ();


			
			player1anim.SetTrigger("Restart");
			player2anim.SetTrigger("Restart");
			
			player1movement.enabled = false;
			player2movement.enabled = false;
			
			fightcountertext.enabled = true;
			
			fightcounter = 3;
			
			
			
			while (fightcounter > 0) {
				fightcountertext.text = fightcounter.ToString ();
				yield return new WaitForSeconds (1f);
				fightcounter--;
				
			}
			
			fightcountertext.text = "Fight!";
			
			yield return new WaitForSeconds (.5f);
			fightcountertext.enabled = false;
			
			player1movement.enabled = true;
			player2movement.enabled = true;
			
			
			
			StartCoroutine("StartTimer");
			
			while(timer>0 && player1health.current_health>0 && player2health.current_health > 0){
				
				player1anim.SetInteger("Health",player1health.current_health);				
				player2anim.SetInteger("Health",player2health.current_health);
				
				if(pause){
					player1movement.enabled=false;
					player2movement.enabled=false;
				}
				if(!pause){
					player1movement.enabled = true;
					player2movement.enabled = true;
				}
				player1movement.currentDirection = player1keys.currentDirection;
				player1movement.currentAttack = player1keys.currentAttack;
				
				player2movement.currentDirection = player2keys.currentDirection;
				player2movement.currentAttack = player2keys.currentAttack;
				
				//change facing if switch sides
				FlipSprites();
				
				
				UpdateHealthbar();
				
				
				
				
				yield return null;
				
				
			}
			
			UpdateHealthbar();
			

			
			StopCoroutine("StartTimer");
			//round is over
			
			if(player1health.current_health <= 0 || player2health.current_health <= 0){
				//K.O. text
				kotext.text = "K.O.";
				kotext.enabled = true;
				yield return new WaitForSeconds(3);
				kotext.enabled = false;
			}
			
			
			
			
			if(player1health.current_health < player2health.current_health){
				//player2 wins round
				//K.O. text
				player2_wins++;
				if(player2_wins ==1){
					player21win.enabled =true;
				}
				if(player2_wins ==2){
					player22win.enabled =true;
				}
				kotext.text = "Player 2 Wins";
				kotext.enabled = true;
				player2anim.SetTrigger("Win");
				yield return new WaitForSeconds(3);
				kotext.enabled = false;



				current_round++;
			}
			if(player1health.current_health > player2health.current_health){
				//player1 wins round
				//K.O. text
				player1_wins++;		
				if(player1_wins ==1){
					player11win.enabled =true;
				}
				if(player1_wins ==2){
					player12win.enabled =true;
				}

				kotext.text = "Player 1 Wins";
				kotext.enabled = true;
				player1anim.SetTrigger("Win");
				yield return new WaitForSeconds(3);
				kotext.enabled = false;
						

				current_round++;
			}
			if(player1health.current_health == player2health.current_health){
				//tie nothing changes
				current_round++;
			}
			if(player2_wins >=2){
				current_fight++;
				//you lose
				//continue?
				Debug.Log("You Lose!");
				
			}
			if(player1_wins >= 2){
				current_fight++;
				if(current_fight <= 3){
					Destroy(player2);
					InstantiateOpponent();
					SetUpPlayers ();
				}else{
					//you win
					Debug.Log ("You Win!");
					
				}
				
				
			}
			
			player1health.Reset_health();
			player2health.Reset_health();
			
			
			UpdateHealthbar();
			
			if(current_fight>numfights&&(player1_wins ==2 || player2_wins ==2)){

				restarttext.text = "Press Enter for Rematch\nPress Space to End";
				restarttext.enabled = true;
				
				StopCoroutine ("Pause");
				
				
				while(!(Input.GetKeyUp(KeyCode.Return)||Input.GetKeyUp(KeyCode.Space))){
					yield return null;
				}
				if(Input.GetKeyUp(KeyCode.Return)){
					//Rematch
					current_fight = 1;
					player1_wins = 0;
					player2_wins = 0;
					player11win.enabled =false;
					player12win.enabled =false;
					player21win.enabled =false;
					player22win.enabled =false;

				}
				
				if(Input.GetKeyUp(KeyCode.Space)){
					//Title
					Application.LoadLevel("StartScreen");
				}
				
				
				StartCoroutine ("Pause");
			}
			player1.transform.position = new Vector3(-3f,-1f,0);
			player2.transform.position = new Vector3(3f,-1f,0);
			restarttext.enabled = false;

		}


	}
	
	IEnumerator StartTimer(){
		while (true) {
			yield return new WaitForSeconds (1);
			timer--;
			DisplayTimer ();
		}
		
	}
	
	IEnumerator Pause(){
		pause = false;
		
		while (true) {
			
			if (Input.GetKeyDown (KeyCode.Return)) {
				pause = true;
			}
			if (pause) {
				Time.timeScale = 0;
				pausepanel.gameObject.SetActive(true);
				if(!resumeset){
					resumeset = true;
					EventSystem.current.SetSelectedGameObject (resumebutton.gameObject);
				}
				if(resume){
					pause = false;
				}
			}
			if (!pause) {
				resumeset = false;
				resume = false;
				pausepanel.gameObject.SetActive(false);
				Time.timeScale = 1;
				
			}
			yield return null;
		}
	}
	public void SelectOnHover(GameObject g){
		EventSystem.current.SetSelectedGameObject (g);
	}
	public void Resume(){

		resume = true;
	}
	public void Exit(){
		StopAllCoroutines ();
		Time.timeScale = 1;
		pausepanel.gameObject.SetActive(false);
		Application.LoadLevel ("StartScreen");
	}
	void UpdateHealthbar(){
		//Healthbar Update
		player1healthbarrt.sizeDelta = new Vector2(player1health.current_health * 2.0f,30f);	
		player2healthbarrt.sizeDelta = new Vector2(player2health.current_health * 2.0f,30f);
		
	}
	
	
}
