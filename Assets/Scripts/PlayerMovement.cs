using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
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

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown(left)) {
//			leftkeydown = true;
//		}	
//		if (Input.GetKeyDown(right)) {
//			rightkeydown = true;
//		}
//		if (Input.GetKeyDown(jump)) {
//			jumpkeydown = true;			
//		}
//		if (Input.GetKeyDown(block)) {
//			blockkeydown = true;
//		}
//		if (Input.GetKeyDown(projectile)) {
//			projectilekeydown = true;
//		}
//		if (Input.GetKeyDown (attack)) {
//			attackkeydown = true;
//		}
//		if (Input.GetKeyUp(left)) {
//			leftkeydown = false;
//		}	
//		if (Input.GetKeyUp(right)) {
//			rightkeydown = false;
//		}
//		if (Input.GetKeyUp(jump)) {
//			jumpkeydown = false;			
//		}
//		if (Input.GetKeyUp(block)) {
//			blockkeydown = false;
//		}
//		if (Input.GetKeyUp(projectile)) {
//			projectilekeydown = false;
//		}
//		if (Input.GetKeyUp (attack)) {
//			attackkeydown = false;
//		}
		leftkeydown = Input.GetKey(left);
		rightkeydown = Input.GetKey(right);
		jumpkeydown = Input.GetKeyDown(jump);
		blockkeydown = Input.GetKey(block);
		projectilekeydown = Input.GetKey(projectile);
		attackkeydown = Input.GetKey (attack);

		XMove (leftkeydown, rightkeydown);
		YMove (jumpkeydown, blockkeydown);
		Attack(projectilekeydown, attackkeydown);

	}

	enum State {Left, Right, Idle, Block, Jump, JumpRight, JumpLeft};
	
	State CurrentState = State.Idle;

	void XMove (bool leftdown, bool rightdown){

		if (leftdown && rightdown) {
			Debug.Log ("Invalid Key Combination");
		} else if (leftdown) {
			Left ();
		} else if (rightdown) {
			Right ();
		} else if (!leftdown && !rightdown) {
			Debug.Log ("No X Movement");
		}
	}
	void YMove(bool jumpdown, bool blockdown){
		if (jumpdown && blockdown) {
			Debug.Log ("Invalid Key Combination");
		} else if (jumpdown) {
			Jump ();
		} else if (blockdown) {
			Block ();
		} else if (!jumpdown && !blockdown) {
			Debug.Log ("No Y Movement");
		}
	}
	void Attack( bool projectiledown, bool attackdown){
		if (projectiledown && attackdown) {
			Debug.Log ("Invalid Key Combination");
		} else if (attackdown) {
			Contact_Atk ();
		} else if (projectiledown) {
			Projectile_Atk ();
		} if (!projectiledown && !attackdown) {
			Debug.Log ("No Attack");
		}

	}

	void Jump(){
		Debug.Log ("Jumping!");
	}
	void Left(){		
		Debug.Log ("Moving Left!");
	}
	void Right(){		
		Debug.Log ("Moving Right!");
	}
	void Contact_Atk(){		
		Debug.Log ("Contact Attack!");
	}
	void Projectile_Atk(){
		Debug.Log ("Projectile Attack!");
	}
	void Block(){
		Debug.Log ("Blocking!");
	}

}
