﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
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

	//Player1 Input
	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;
	public KeyCode block;
	public KeyCode attack;
	public KeyCode projectile;

	bool leftkeydown;
	bool rightkeydown;
	bool jumpkeydown;
	bool blockkeydown;
	bool attackkeydown;
	bool projectilekeydown;
	bool leftkey;
	bool rightkey;
	bool jumpkey;
	bool blockkey;
	bool attackkey;
	bool projectilekey;
	bool leftkeyup;
	bool rightkeyup;
	bool jumpkeyup;
	bool blockkeyup;
	bool attackkeyup;
	bool projectilekeyup;
	float waittime=0;
	bool aiprojectilekeydown;
	bool firstframe;
	bool nextframe;
	bool roundover;
	bool pause=false;

	PlayerMovement player1movement,player2movement;

	enum XDirection{None,Left,Right,Special};
	enum YDirection{None,Up,Down,Special};

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
		DetermineFightOpponents ();

		StartCoroutine ("Pause");

		StartCoroutine ("Game");






	}


	void DetermineFightOpponents(){
		int randomnumber;

		List<PlayerName> keyList = new List<PlayerName> (_dicPlayerPrefabByName.Keys);

		if (Select.selectedPlayer == PlayerName.None) {
			Select.selectedPlayer = PlayerName.Craig;
		}

		keyList.Remove (PlayerName.None);
		keyList.Remove (PlayerName.Killer);
		keyList.Remove (Select.selectedPlayer);
		//assign random name from remaining names
		int i = 0;
		while (keyList.Count > 0) {
			randomnumber = Random.Range (0, keyList.Count);
			opponentList[i] = keyList[randomnumber];
			keyList.Remove(keyList[randomnumber]);
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

	void GetPlayer1Inputs(){
		//Inputs
		
		leftkeydown 	= 	Input.GetKeyDown(left);
		rightkeydown 	= 	Input.GetKeyDown(right);
		jumpkeydown 	= 	Input.GetKeyDown(jump);
		blockkeydown 	= 	Input.GetKeyDown(block);
		projectilekeydown = Input.GetKeyDown(projectile);
		attackkeydown 	= 	Input.GetKeyDown (attack);
		leftkey		 	= 	Input.GetKey(left);
		rightkey	 	= 	Input.GetKey(right);
		jumpkey		 	= 	Input.GetKey(jump);
		blockkey	 	= 	Input.GetKey(block);
		projectilekey	= 	Input.GetKey(projectile);
		attackkey	 	= 	Input.GetKey (attack);
		leftkeyup 		= 	Input.GetKeyUp(left);
		rightkeyup 		= 	Input.GetKeyUp(right);
		jumpkeyup 		= 	Input.GetKeyUp(jump);
		blockkeyup 		= 	Input.GetKeyUp(block);
		projectilekeyup = 	Input.GetKeyUp(projectile);
		attackkeyup 	= 	Input.GetKeyUp (attack);
		
		distance = Mathf.Abs (player1.transform.localPosition.x - player2.transform.localPosition.x);
		//if any input, Change State (my keyboard can only handle 2 arrows at once)
		//player1 directions and attacks
		GetDirection ();
		GetAttack ();

	}
	void GetAttack(){
		if (attackkeydown && projectilekeydown) {
			player1movement.currentAttack = Attack.CombatProjectile;
		}
		if (attackkeydown && !projectilekeydown) {
			player1movement.currentAttack = Attack.Combat;
		}
		if (!attackkeydown && projectilekeydown) {
			player1movement.currentAttack = Attack.Projectile;
		}
		if (!attackkeydown && !projectilekeydown) {
			player1movement.currentAttack = Attack.None;
		}
	}
	void GetDirection(){
		Horizontalinput (); //currentX updated
		Verticalinput (); //currentY updated
		if (currentX == XDirection.None && currentY == YDirection.None) {
			player1movement.currentDirection = Direction.None;
		}
		if (currentX == XDirection.None && currentY == YDirection.Up) {
			player1movement.currentDirection = Direction.Up;
		}
		if (currentX == XDirection.None && currentY == YDirection.Down) {
			player1movement.currentDirection = Direction.Down;
		}
		if (currentX == XDirection.Left && currentY == YDirection.None) {
			player1movement.currentDirection = Direction.Left;
		}
		if (currentX == XDirection.Left && currentY == YDirection.Up) {
			player1movement.currentDirection = Direction.LeftUp;
		}		
		if (currentX == XDirection.Left && currentY == YDirection.Down) {
			player1movement.currentDirection = Direction.LeftDown;
		}
		if (currentX == XDirection.Right && currentY == YDirection.None) {
			player1movement.currentDirection = Direction.Right;
		}
		if (currentX == XDirection.Right && currentY == YDirection.Up) {
			player1movement.currentDirection = Direction.RightUp;
		}		
		if (currentX == XDirection.Right && currentY == YDirection.Down) {
			player1movement.currentDirection = Direction.RightDown;
		}if (currentX == XDirection.Special && currentY == YDirection.None) {
			player1movement.currentDirection = Direction.Special;
		}
		//else... something's wrong... don't mess with current Direction
		
		
		
		
	}
	void Horizontalinput(){
		if (currentX == XDirection.None) {
			if (leftkeydown) {
				nextX = XDirection.Left;
			}
			if (rightkeydown) {
				nextX = XDirection.Right;
			}
			if(rightkeydown&&leftkeydown){
				nextX = XDirection.Special;
			}
		}
		if (currentX == XDirection.Left) {
			if(rightkeydown){
				nextX = XDirection.Right;
			}
			if(leftkeyup && !rightkey){
				nextX = XDirection.None;
			}
			if(leftkeyup && rightkey){
				nextX = XDirection.Right;
			}
		}
		if (currentX == XDirection.Right) {
			if(leftkeydown){
				nextX = XDirection.Left;
			}
			if(rightkeyup && !leftkey){
				nextX = XDirection.None;
			}
			if(rightkeyup && leftkey){
				nextX = XDirection.Left;
			}
		}
		if (currentX == XDirection.Special) {
			if(rightkeyup && !leftkeyup){
				nextX = XDirection.Left;
			}
			if(leftkeyup && !rightkeyup){
				nextX = XDirection.Right;
			}
			if(leftkeyup && rightkeyup){
				nextX = XDirection.None;
			}
			
		}
		//Debug.Log ("X:"+nextX);
		currentX = nextX;
	}
	
	void Verticalinput(){
		if (currentY == YDirection.None) {
			if (jumpkeydown) {
				nextY = YDirection.Up;
			}
			if (blockkeydown) {
				nextY = YDirection.Down;
			}
			if(blockkeydown&&jumpkeydown){
				nextY = YDirection.Special;
			}
		}
		if (currentY == YDirection.Up) {
			if(blockkeydown){
				nextY = YDirection.Down;
			}
			if(jumpkeyup && !blockkey){
				nextY = YDirection.None;
			}
			if(jumpkeyup && blockkey){
				nextY = YDirection.Down;
			}
		}
		if (currentY == YDirection.Down) {
			if(jumpkeydown){
				nextY = YDirection.Up;
			}
			if(blockkeyup && !jumpkey){
				nextY = YDirection.None;
			}
			if(blockkeyup && jumpkey){
				nextY = YDirection.Up;
			}
		}
		if (currentY == YDirection.Special) {
			if(jumpkeyup && !blockkeyup){
				nextY = YDirection.Down;
			}
			if(jumpkeyup && !blockkeyup){
				nextY = YDirection.Up;
			}
			if(jumpkeyup && blockkeyup){
				nextY = YDirection.None;
			}
			
		}
		//Debug.Log ("Y:"+nextY);
		currentY = nextY;
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
		player1 = Instantiate(_dicPlayerPrefabByName [Select.selectedPlayer],new Vector3(-3f,-1f,0),Quaternion.identity) as GameObject;
	}
	void InstantiateOpponent(){
		player2 = Instantiate(_dicPlayerPrefabByName[ opponentList[current_fight-1]],new Vector3(3f,-1f,0),Quaternion.identity) as GameObject;
	}


	void SetUpPlayers(){
		player1health = player1.GetComponent<PlayerHealth> ();		
		player2health = player2.GetComponent<PlayerHealth> ();
		
		player1healthbarrt = player1healthbar.rectTransform;
		player2healthbarrt = player2healthbar.rectTransform;
		
		player1movement = player1.GetComponent<PlayerMovement> ();
		player2movement = player2.GetComponent<PlayerMovement> ();
		
		player1movement.thisPlayer = Player.Player1;
		player2movement.thisPlayer = Player.Player2;
		
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
		firstframe = true;
		fightcountertext.enabled = false;
		restarttext.enabled = false;
		kotext.enabled = false;
		//Create player1 and player2
		InstantiatePlayer1 ();
		InstantiateOpponent ();

		//hook up scripts
		SetUpPlayers ();



		while (!(current_fight > 3)) {

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
				GetPlayer1Inputs ();
				GetPlayer2AIInputs ();
				//change facing if switch sides
				FlipSprites();
				
					
				UpdateHealthbar();

				


				yield return null;


			}

			StopCoroutine("StartTimer");
			//round is over
			
			if(player1health.current_health <= 0 || player2health.current_health <= 0){
				//K.O. text
				kotext.enabled = true;
				kotext.text = "K.O.";
				yield return new WaitForSeconds(2);
				kotext.enabled = false;
			}

			


			if(player1health.current_health < player2health.current_health){
				//player2 wins round
				player2_wins++;
				player2anim.SetTrigger("Win");
				current_round++;
			}
			if(player1health.current_health > player2health.current_health){
				//player1 wins round
				player1_wins++;				
				player1anim.SetTrigger("Win");
				current_round++;
			}
			if(player1health.current_health == player2health.current_health){
				//tie nothing changes
				current_round++;
			}
			if(player2_wins >=2){
				//you lose
				//continue?
				Debug.Log("You Lose!");
				
			}
			if(player1_wins >= 2){
				player1_wins=0;
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

			restarttext.text = "Press Start to Continue";
			restarttext.enabled = true;

			StopCoroutine ("Pause");


			while(!Input.GetKeyUp(KeyCode.Return)){
				yield return null;
			}
			StartCoroutine ("Pause");
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
				pause = !pause;
			}
			if (pause) {
				Time.timeScale = 0;

			}
			if (!pause) {
				Time.timeScale = 1;

			}
			yield return null;
		}
	}
	void UpdateHealthbar(){
		//Healthbar Update
		player1healthbarrt.sizeDelta = new Vector2(player1health.current_health * 2.0f,30f);	
		player2healthbarrt.sizeDelta = new Vector2(player2health.current_health * 2.0f,30f);

	}


}
