using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerKeys : MonoBehaviour {
	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;
	public KeyCode block;
	public KeyCode attack;
	public KeyCode heavyattack;
	public KeyCode projectile;
	public bool newmove;

	bool leftkeydown;
	bool rightkeydown;
	bool jumpkeydown;
	bool blockkeydown;
	bool attackkeydown;
	bool heavyattackkeydown;
	bool projectilekeydown;
	bool leftkey;
	bool rightkey;
	bool jumpkey;
	bool blockkey;
	bool leftkeyup;
	bool rightkeyup;
	bool jumpkeyup;
	bool blockkeyup;
	//bool attackkey;
	//bool projectilekey;
	//bool attackkeyup;
	//bool projectilekeyup;

	XDirection currentX = XDirection.None;
	XDirection nextX = XDirection.None;
	YDirection currentY = YDirection.None;
	YDirection nextY = YDirection.None;
	
	private Dictionary<Direction,XDirection> _dicXdirByDir = null;
	private Dictionary<Direction,YDirection> _dicYdirByDir = null;

	public Direction currentDirection;
	public Attack currentAttack;
	public Move currentMove;
	Direction nextDirection;
	Attack nextAttack;
	void Awake(){
		
		_dicXdirByDir = new Dictionary<Direction,XDirection> ();
		_dicXdirByDir.Add (Direction.None, XDirection.None);
		_dicXdirByDir.Add (Direction.Left, XDirection.Left);
		_dicXdirByDir.Add (Direction.Right, XDirection.Right);
		_dicXdirByDir.Add (Direction.Up, XDirection.None);
		_dicXdirByDir.Add (Direction.Down, XDirection.None);
		_dicXdirByDir.Add (Direction.LeftUp, XDirection.Left);
		_dicXdirByDir.Add (Direction.RightUp, XDirection.Right);
		_dicXdirByDir.Add (Direction.LeftDown, XDirection.Left);
		_dicXdirByDir.Add (Direction.RightDown, XDirection.Right);
		_dicXdirByDir.Add (Direction.Special, XDirection.Special);

		_dicYdirByDir = new Dictionary<Direction,YDirection> ();
		_dicYdirByDir.Add (Direction.None, YDirection.None);
		_dicYdirByDir.Add (Direction.Left, YDirection.None);
		_dicYdirByDir.Add (Direction.Right, YDirection.None);
		_dicYdirByDir.Add (Direction.Up, YDirection.Up);
		_dicYdirByDir.Add (Direction.Down, YDirection.Down);
		_dicYdirByDir.Add (Direction.LeftUp, YDirection.Up);
		_dicYdirByDir.Add (Direction.RightUp, YDirection.Up);
		_dicYdirByDir.Add (Direction.LeftDown, YDirection.Down);
		_dicYdirByDir.Add (Direction.RightDown, YDirection.Down);
		_dicYdirByDir.Add (Direction.Special, YDirection.None);
	}
	void Start(){
		currentDirection = Direction.None;
		currentAttack = Attack.None;
	}
	
	// Update is called once per frame
	void Update () {

		//Inputs
		
		leftkeydown 	= 	Input.GetKeyDown(left);
		rightkeydown 	= 	Input.GetKeyDown(right);
		jumpkeydown 	= 	Input.GetKeyDown(jump);
		blockkeydown 	= 	Input.GetKeyDown(block);
		projectilekeydown = Input.GetKeyDown(projectile);
		attackkeydown 	= 	Input.GetKeyDown (attack);
		heavyattackkeydown = Input.GetKeyDown (heavyattack);
		leftkey		 	= 	Input.GetKey(left);
		rightkey	 	= 	Input.GetKey(right);
		jumpkey		 	= 	Input.GetKey(jump);
		blockkey	 	= 	Input.GetKey(block);
		leftkeyup 		= 	Input.GetKeyUp(left);
		rightkeyup 		= 	Input.GetKeyUp(right);
		jumpkeyup 		= 	Input.GetKeyUp(jump);
		blockkeyup 		= 	Input.GetKeyUp(block);
		//projectilekey	= 	Input.GetKey(projectile);
		//attackkey	 	= 	Input.GetKey (attack);
		//projectilekeyup = 	Input.GetKeyUp(projectile);
		//attackkeyup 	= 	Input.GetKeyUp (attack);
		
		//if any input, Change State (my keyboard can only handle 2 arrows at once)
		//player1 directions and attacks
		nextDirection = GetDirection (leftkeydown,rightkeydown,jumpkeydown,blockkeydown,leftkey,rightkey,jumpkey,blockkey,leftkeyup,rightkeyup,jumpkeyup,blockkeyup, currentDirection);
		nextAttack = GetAttack (attackkeydown,heavyattackkeydown,projectilekeydown);


		currentDirection = nextDirection;
		currentAttack = nextAttack;
		currentMove = new Move(currentDirection,currentAttack);
	 
		
	}
	Attack GetAttack(bool attackkeydown, bool heavyattackkeydown, bool projectilekeydown){
		if ((projectilekeydown) && (attackkeydown ||heavyattackkeydown)) {
			return Attack.CombatProjectile;
		}
		if (attackkeydown && !projectilekeydown && !heavyattackkeydown) {
			return Attack.LightCombat;
		}
		if (heavyattackkeydown && !projectilekeydown) {
			return Attack.HeavyCombat;
		}
		if (!attackkeydown && projectilekeydown && !heavyattackkeydown) {
			return Attack.Projectile;
		}
		if (!attackkeydown && !projectilekeydown && !heavyattackkeydown) {
			return Attack.None;
		}
		//so it doesn't complain
		return Attack.None;

	}
	Direction GetDirection(bool ld, bool rd, bool ud, bool dd, bool l, bool r, bool u, bool d, bool lu, bool ru, bool uu, bool du, Direction current = Direction.None){
		XDirection currentX = Horizontalinput (_dicXdirByDir[current],ld,rd,l,r,lu,ru); //currentX updated
		YDirection currentY = Verticalinput (_dicYdirByDir[current],ud,dd,u,d,uu,du); //currentY updated

		if (currentX == XDirection.None && currentY == YDirection.None) {
			return Direction.None;
		}
		if (currentX == XDirection.None && currentY == YDirection.Up) {
			return Direction.Up;
		}
		if (currentX == XDirection.None && currentY == YDirection.Down) {
			return Direction.Down;
		}
		if (currentX == XDirection.Left && currentY == YDirection.None) {
			return Direction.Left;
		}
		if (currentX == XDirection.Left && currentY == YDirection.Up) {
			return Direction.LeftUp;
		}		
		if (currentX == XDirection.Left && currentY == YDirection.Down) {
			return Direction.LeftDown;
		}
		if (currentX == XDirection.Right && currentY == YDirection.None) {
			return Direction.Right;
		}
		if (currentX == XDirection.Right && currentY == YDirection.Up) {
			return Direction.RightUp;
		}		
		if (currentX == XDirection.Right && currentY == YDirection.Down) {
			return Direction.RightDown;
		}if (currentX == XDirection.Special && currentY == YDirection.None) {
			return Direction.Special;
		}
		return current;

	}
	XDirection Horizontalinput(XDirection x, bool ld, bool rd, bool l, bool r, bool lu, bool ru){
		if (x == XDirection.None) {
			if (ld) {
				return XDirection.Left;
			}
			if (rd) {
				return XDirection.Right;
			}
			if(rd&&ld){
				return XDirection.Special;
			}
		}
		if (x == XDirection.Left) {
			if(rd){
				return XDirection.Right;
			}
			if(lu && !r){
				return XDirection.None;
			}
			if(lu && r){
				return XDirection.Right;
			}
		}
		if (x == XDirection.Right) {
			if(ld){
				return XDirection.Left;
			}
			if(ru && !l){
				return XDirection.None;
			}
			if(ru && l){
				return XDirection.Left;
			}
		}
		if (x == XDirection.Special) {
			if(ru && !lu){
				return XDirection.Left;
			}
			if(lu && !ru){
				return XDirection.Right;
			}
			if(lu && ru){
				return XDirection.None;
			}
			
		}
		return x;
	}
	YDirection Verticalinput(YDirection y, bool ud, bool dd, bool u, bool d, bool uu, bool du){
		if (y == YDirection.None) {
			if (ud) {
				return YDirection.Up;
			}
			if (dd) {
				return YDirection.Down;
			}
			if(dd&&ud){
				return YDirection.Special;
			}
		}
		if (y == YDirection.Up) {
			if(dd){
				return YDirection.Down;
			}
			if(uu && !d){
				return YDirection.None;
			}
			if(uu && d){
				return YDirection.Down;
			}
		}
		if (y == YDirection.Down) {
			if(ud){
				return YDirection.Up;
			}
			if(du && !u){
				return YDirection.None;
			}
			if(du && u){
				return YDirection.Up;
			}
		}
		if (y == YDirection.Special) {
			if(uu && !du){
				return YDirection.Down;
			}
			if(!uu && du){
				return YDirection.Up;
			}
			if(uu && du){
				return YDirection.None;
			}
			
		}
		return y;
	}

}
