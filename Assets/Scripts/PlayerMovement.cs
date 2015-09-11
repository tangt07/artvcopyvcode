﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D rb;
	public Animator anim;
	public LayerMask whatIsGround;
	public Transform groundCheck;
	public bool grounded=false;
	public bool groundchange = false;

	public GameObject projectileprefab;
	GameObject projectilecopy;

	public float maxforwardvelocity = 3f;
	public float maxbackvelocity = 2f;

	public Direction currentDirection = Direction.None;
	public Attack currentAttack = Attack.None;
	public Facing currentFacing = Facing.Right;

	public bool shieldup;
	public bool projectilefired;
	public PlayerNumber thisPlayer = PlayerNumber.None;
	public PlayerName thisCharacter = PlayerName.None;
	public List<Projectile> projectileList = new List<Projectile> ();
	public bool damage;
	public float jumpspeed = 11f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator>();



	}
	void Update(){
		shieldup = (grounded && 
		            (currentFacing == Facing.Right && (currentDirection == Direction.Left || currentDirection == Direction.LeftDown)) ||
		            (currentFacing == Facing.Left && (currentDirection == Direction.Right || currentDirection == Direction.RightDown)));
		if(currentAttack == Attack.Combat){
			anim.SetTrigger("Attack");
		}
		
		if(grounded && currentAttack == Attack.Projectile){
			anim.SetTrigger("Projectile");
			projectileList.Add(new Projectile(SpawnProjectile(currentFacing),currentFacing)); 
		}
		
		if(currentAttack == Attack.CombatProjectile){
			anim.SetTrigger("Projectile");
			projectileList.Add(new Projectile(SpawnProjectile(currentFacing),currentFacing)); 
		}

		SetAnimDirectionBools (currentDirection);
		SetJumpingBools (grounded, currentDirection);
		SetFacingBools (currentFacing);

	}
	GameObject SpawnProjectile(Facing currentFacing){

		if (currentFacing == Facing.Right) {
			projectilecopy = Instantiate (projectileprefab, transform.position + .5f * transform.right, Quaternion.identity) as GameObject;

		}
		if (currentFacing == Facing.Left) {
			projectilecopy = Instantiate (projectileprefab, transform.position - .5f * transform.right, Quaternion.identity) as GameObject;

		}
		return projectilecopy;
	}

	void MoveProjectiles(List<Projectile> projectileList){

		foreach(Projectile projectile in projectileList){
			if(projectile.go){
				Rigidbody2D prb = projectile.go.GetComponent<Rigidbody2D>();

				
				if (projectile.facing == Facing.Right) {
					prb.MoveRotation(prb.rotation-720f*Time.fixedDeltaTime);
					prb.velocity = new Vector2(10f,0);
				}
				if (projectile.facing == Facing.Left) {
					prb.MoveRotation(prb.rotation+720f*Time.fixedDeltaTime);
					prb.velocity = new Vector2(-10f,0);
				}
			}

		}
		projectileList.RemoveAll (projectile => projectile.go == null);


	}
	public void SetJumpingBools(bool grounded, Direction currentDirection){
		if (grounded && (currentDirection == Direction.Up || currentDirection == Direction.RightUp || currentDirection == Direction.LeftUp)) {
			anim.SetBool ("Grounded", false);
			groundchange = true;
			grounded = false;
			
		} 
		if (grounded) {
			anim.SetBool ("Grounded", true);
		} else {
			anim.SetFloat("Yvelocity", rb.velocity.y);
		}
	}
	public void SetFacingBools(Facing currentFacing){
		if (currentFacing == Facing.Left) {
			anim.SetBool("FacingLeft",true);
			anim.SetBool("FacingRight",false);
		}
		
		if (currentFacing == Facing.Right) {
			anim.SetBool("FacingRight",true);
			anim.SetBool("FacingLeft",false);			
		}
	}
	public void SetAnimDirectionBools(Direction currentDirection){
		if (currentDirection == Direction.None) {
			anim.SetBool("DirectionRight",false);
			anim.SetBool("DirectionLeft",false);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",false);
		}
		if (currentDirection == Direction.Left) {
			anim.SetBool("DirectionRight",false);
			anim.SetBool("DirectionLeft",true);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",false);
		}
		if (currentDirection == Direction.Right) {
			anim.SetBool("DirectionRight",true);
			anim.SetBool("DirectionLeft",false);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",false);
		}
		if (currentDirection == Direction.Up) {
			anim.SetBool("DirectionRight",false);
			anim.SetBool("DirectionLeft",false);
			anim.SetBool("DirectionUp",true);
			anim.SetBool("DirectionDown",false);
		}
		if (currentDirection == Direction.LeftUp) {
			anim.SetBool("DirectionRight",false);
			anim.SetBool("DirectionLeft",true);
			anim.SetBool("DirectionUp",true);
			anim.SetBool("DirectionDown",false);
		}
		if (currentDirection == Direction.RightUp) {
			anim.SetBool("DirectionRight",true);
			anim.SetBool("DirectionLeft",false);
			anim.SetBool("DirectionUp",true);
			anim.SetBool("DirectionDown",false);
		}
		if (currentDirection == Direction.Down) {
			anim.SetBool("DirectionRight",false);
			anim.SetBool("DirectionLeft",false);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",true);
		}
		if (currentDirection == Direction.LeftDown) {
			anim.SetBool("DirectionRight",false);
			anim.SetBool("DirectionLeft",true);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",true);
		}
		if (currentDirection == Direction.RightDown) {
			anim.SetBool("DirectionRight",true);
			anim.SetBool("DirectionLeft",false);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",true);
		}
		if (currentDirection == Direction.Special) {
			anim.SetBool("DirectionRight",true);
			anim.SetBool("DirectionLeft",true);
			anim.SetBool("DirectionUp",false);
			anim.SetBool("DirectionDown",false);
		}
	}

	void FixedUpdate(){
		Direction cacheDirection;
		Facing cacheFacing;
		Attack cacheAttack;
		Vector2 tempv;

		tempv = rb.velocity;

		MoveProjectiles (projectileList);
		

		if (groundchange) {
			groundchange = false;
			tempv.y = jumpspeed;
		}
		if (!groundchange) {
			grounded = Physics2D.OverlapCircle (groundCheck.position, 0.15f, whatIsGround); 
			// checks if you are within 0.15 position in the Y of the ground
		}
		cacheDirection = currentDirection;
		cacheFacing = currentFacing;
		cacheAttack = currentAttack;

		if (grounded&&(cacheDirection == Direction.None || cacheDirection == Direction.Down || cacheDirection == Direction.LeftDown || cacheDirection == Direction.RightDown)) {
			tempv.x = 0;
		}
		if (grounded&&(cacheDirection == Direction.RightUp || (cacheFacing == Facing.Right && cacheDirection==Direction.Right))) {
			tempv.x = maxforwardvelocity;
		}
		if (grounded &&(cacheDirection == Direction.LeftUp|| (cacheFacing == Facing.Left && cacheDirection==Direction.Left))) {
			tempv.x = -maxforwardvelocity;
		}
		if (grounded&&(cacheFacing == Facing.Right && cacheDirection==Direction.Left)) {
			tempv.x = -maxbackvelocity;
		}		
		if (grounded&&(cacheFacing == Facing.Left && cacheDirection==Direction.Right)) {
			tempv.x = maxbackvelocity;
		}
		if (!grounded && tempv.y == 0 && cacheFacing == Facing.Left) {
			//if lands on something not the ground
			tempv = new Vector2(5f,5f);
		}
		if (!grounded && tempv.y == 0 && cacheFacing == Facing.Right) {
			//if lands on something not the ground
			tempv = new Vector2(-5f,5f);
		}
		rb.velocity = tempv;
	}


	

}
