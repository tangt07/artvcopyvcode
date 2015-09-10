using UnityEngine;
using System.Collections;

public class PlayerKeys : MonoBehaviour {
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


	
	XDirection currentX = XDirection.None;
	XDirection nextX = XDirection.None;
	YDirection currentY = YDirection.None;
	YDirection nextY = YDirection.None;

	public Direction currentDirection;
	public Attack currentAttack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetPlayerInputs ();
	
	}

	void GetPlayerInputs(){
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
		
		//if any input, Change State (my keyboard can only handle 2 arrows at once)
		//player1 directions and attacks
		GetDirection ();
		GetAttack ();
		
	}
	void GetAttack(){
		if (attackkeydown && projectilekeydown) {
			currentAttack = Attack.CombatProjectile;
		}
		if (attackkeydown && !projectilekeydown) {
			currentAttack = Attack.Combat;
		}
		if (!attackkeydown && projectilekeydown) {
			currentAttack = Attack.Projectile;
		}
		if (!attackkeydown && !projectilekeydown) {
			currentAttack = Attack.None;
		}
	}
	void GetDirection(){
		Horizontalinput (); //currentX updated
		Verticalinput (); //currentY updated
		if (currentX == XDirection.None && currentY == YDirection.None) {
			currentDirection = Direction.None;
		}
		if (currentX == XDirection.None && currentY == YDirection.Up) {
			currentDirection = Direction.Up;
		}
		if (currentX == XDirection.None && currentY == YDirection.Down) {
			currentDirection = Direction.Down;
		}
		if (currentX == XDirection.Left && currentY == YDirection.None) {
			currentDirection = Direction.Left;
		}
		if (currentX == XDirection.Left && currentY == YDirection.Up) {
			currentDirection = Direction.LeftUp;
		}		
		if (currentX == XDirection.Left && currentY == YDirection.Down) {
			currentDirection = Direction.LeftDown;
		}
		if (currentX == XDirection.Right && currentY == YDirection.None) {
			currentDirection = Direction.Right;
		}
		if (currentX == XDirection.Right && currentY == YDirection.Up) {
			currentDirection = Direction.RightUp;
		}		
		if (currentX == XDirection.Right && currentY == YDirection.Down) {
			currentDirection = Direction.RightDown;
		}if (currentX == XDirection.Special && currentY == YDirection.None) {
			currentDirection = Direction.Special;
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

}
