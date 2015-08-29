using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject craigprefab;
	public GameObject amyprefab;
	public GameObject willprefab;
	public GameObject killerprefab;
	public Text timertext;
	public Image player1healthbar;
	public Image player2healthbar;
	RectTransform player1healthbarrt;
	RectTransform player2healthbarrt;

	public PlayerHealth player1health;
	public PlayerHealth player2health;


	int current_fight = 1;
	int current_round = 1;
	GameObject player1;
	GameObject player2;
	GameObject opponent;
	GameObject[] opponentList = new GameObject[3];
	int player1_wins=0;
	int player2_wins=0;
	public float timer=120f;
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

	PlayerMovement player1movement,player2movement;

	enum XDirection{None,Left,Right,Special};
	enum YDirection{None,Up,Down,Special};

	XDirection currentX = XDirection.None;
	XDirection nextX = XDirection.None;
	YDirection currentY = YDirection.None;
	YDirection nextY = YDirection.None;

	float distance;

	//need to pass Direction Facing and Attack to PlayerMovement Scripts





	// Use this for initialization
	void Start () {

		//Select.selectedPlayer = Select.Player.Craig; // for testing
		if (Select.selectedPlayer == Select.Player.Craig) {
			player1 = Instantiate(craigprefab,new Vector3(-3f,-1f,0),Quaternion.identity) as GameObject;
			//opponent is either amy or will
			RandomOpponent(amyprefab,willprefab);
		}
		if (Select.selectedPlayer == Select.Player.Amy) {
			player1 = Instantiate(amyprefab,new Vector3(-3f,-1f,0),Quaternion.identity) as GameObject;
			//opponent is either craig or will
			RandomOpponent(craigprefab,willprefab);
		}
		if (Select.selectedPlayer == Select.Player.Will) {
			player1 = Instantiate(willprefab,new Vector3(-3f,-1f,0),Quaternion.identity) as GameObject;
			//opponent is either craig or amy
			RandomOpponent(craigprefab,amyprefab);
		}


		InstantiateOpponent ();

		player1health = player1.GetComponent<PlayerHealth> ();		
		player2health = player2.GetComponent<PlayerHealth> ();
		player1healthbarrt = player1healthbar.rectTransform;
		player2healthbarrt = player2healthbar.rectTransform;

		player1movement = player1.GetComponent<PlayerMovement> ();
		player2movement = player2.GetComponent<PlayerMovement> ();
		player1movement.currentFacing = PlayerMovement.Facing.Right;
		player2movement.currentFacing = PlayerMovement.Facing.Left;
		player1movement.thisPlayer = PlayerMovement.Player.Player1;
		player2movement.thisPlayer = PlayerMovement.Player.Player2;

		FlipSprites ();
		StartCoroutine (GameTimer ());

		firstframe = true;




	}
	void RandomOpponent(GameObject x,GameObject y){
		// Determines opponents for game
		int randomnumber = Random.Range (0,2);
		if (randomnumber == 0) {
			opponentList[0] = x;
			opponentList[1] = y;
		} else {
			opponentList[0] = y;
			opponentList[1] = x;
		}
		opponentList[2] = killerprefab;
	}
	void Restart(){
		//the fight starts over
	
	}
	void Quit(){

	}
	// Update is called once per frame
	void Update()
	{	
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
		//change facing if switch sides
		FlipSprites();


		//projectile button
		if (nextframe) {
			nextframe = false;
			player2movement.currentAttack = PlayerMovement.Attack.None;
		}
		if (!nextframe) {
			if(distance > 3.5 && waittime > 1){
				waittime=0;
				player2movement.currentAttack = PlayerMovement.Attack.Projectile;
				nextframe=true;

			}
			else{
				//player2 continuously blocks
				if (player2movement.currentFacing == PlayerMovement.Facing.Left) {
					player2movement.currentDirection = PlayerMovement.Direction.RightDown;
				}
				if (player2movement.currentFacing == PlayerMovement.Facing.Right) {
					player2movement.currentDirection = PlayerMovement.Direction.LeftDown;
				}
			}
		}

		waittime += Time.deltaTime;

		//Healthbar Update
		player1healthbarrt.sizeDelta = new Vector2(player1health.current_health * 2.0f,30f);	
		player2healthbarrt.sizeDelta = new Vector2(player2health.current_health * 2.0f,30f);	
	}
	void GetAttack(){
		if (attackkeydown && projectilekeydown) {
			player1movement.currentAttack = PlayerMovement.Attack.CombatProjectile;
		}
		if (attackkeydown && !projectilekeydown) {
			player1movement.currentAttack = PlayerMovement.Attack.Combat;
		}
		if (!attackkeydown && projectilekeydown) {
			player1movement.currentAttack = PlayerMovement.Attack.Projectile;
		}
		if (!attackkeydown && !projectilekeydown) {
			player1movement.currentAttack = PlayerMovement.Attack.None;
		}
	}
	void GetDirection(){
		Horizontalinput (); //currentX updated
		Verticalinput (); //currentY updated
		if (currentX == XDirection.None && currentY == YDirection.None) {
			player1movement.currentDirection = PlayerMovement.Direction.None;
		}
		if (currentX == XDirection.None && currentY == YDirection.Up) {
			player1movement.currentDirection = PlayerMovement.Direction.Up;
		}
		if (currentX == XDirection.None && currentY == YDirection.Down) {
			player1movement.currentDirection = PlayerMovement.Direction.Down;
		}
		if (currentX == XDirection.Left && currentY == YDirection.None) {
			player1movement.currentDirection = PlayerMovement.Direction.Left;
		}
		if (currentX == XDirection.Left && currentY == YDirection.Up) {
			player1movement.currentDirection = PlayerMovement.Direction.LeftUp;
		}		
		if (currentX == XDirection.Left && currentY == YDirection.Down) {
			player1movement.currentDirection = PlayerMovement.Direction.LeftDown;
		}
		if (currentX == XDirection.Right && currentY == YDirection.None) {
			player1movement.currentDirection = PlayerMovement.Direction.Right;
		}
		if (currentX == XDirection.Right && currentY == YDirection.Up) {
			player1movement.currentDirection = PlayerMovement.Direction.RightUp;
		}		
		if (currentX == XDirection.Right && currentY == YDirection.Down) {
			player1movement.currentDirection = PlayerMovement.Direction.RightDown;
		}if (currentX == XDirection.Special && currentY == YDirection.None) {
			player1movement.currentDirection = PlayerMovement.Direction.Special;
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
			player1movement.currentFacing = PlayerMovement.Facing.Right;
			player2.transform.localScale = new Vector3 (-1, 1, 1);
			player2movement.currentFacing = PlayerMovement.Facing.Left;
		} 
		else {
			player1.transform.localScale = new Vector3 (-1, 1, 1);				
			player1movement.currentFacing = PlayerMovement.Facing.Left;
			player2.transform.localScale = new Vector3 (1, 1, 1);
			player2movement.currentFacing = PlayerMovement.Facing.Right;

		}
	}
	void InstantiateOpponent(){

		player2 = Instantiate(opponentList[current_fight-1],new Vector3(3f,-1f,0),Quaternion.identity) as GameObject;

	}
	IEnumerator GameTimer(){
		while (timer > 0) {
			yield return new WaitForSeconds(1);
			timer--;
			System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
			timerFormatted = string.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
			timertext.text = timerFormatted;
		}
		Debug.Log ("Game Over");
	}









}
