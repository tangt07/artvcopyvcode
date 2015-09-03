using UnityEngine;
using System.Collections;

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

	float maxforwardvelocity = 4f;
	float maxbackvelocity = 2f;

	public Direction currentDirection = Direction.None;
	public Attack currentAttack = Attack.None;
	public Facing currentFacing = Facing.Right;

	public bool shieldup;
	public Player thisPlayer = Player.None;
	public Player thisCharacter = Player.None;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator>();



	}
	
	// Update is called once per frame
	void Update () {
		//The following are all the inputs needed for State Machine


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
		if (currentFacing == Facing.Left) {
			anim.SetBool("FacingLeft",true);
			anim.SetBool("FacingRight",false);
		}

		if (currentFacing == Facing.Right) {
			anim.SetBool("FacingRight",true);
			anim.SetBool("FacingLeft",false);			
		}
		
		if(currentAttack == Attack.Combat){
			anim.SetTrigger("Attack");
		}
		
		if(grounded && currentAttack == Attack.Projectile){
			anim.SetTrigger("Projectile");
			SpawnProjectile();
		}
		
		if(currentAttack == Attack.CombatProjectile){
			anim.SetTrigger("Projectile");
			SpawnProjectile();
		}
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
		shieldup = (grounded && (currentFacing == Facing.Right && (currentDirection == Direction.Left || currentDirection == Direction.LeftDown)) ||
			(currentFacing == Facing.Left && (currentDirection == Direction.Right || currentDirection == Direction.RightDown)));



		if (projectilecopy) {
			if (currentFacing == Facing.Right) {
				projectilecopy.transform.Rotate(new Vector3(0,0,-720f*Time.deltaTime));
			}
			if (currentFacing == Facing.Left) {
				projectilecopy.transform.Rotate(new Vector3(0,0,720f*Time.deltaTime));
			}
		}


	}
	void SpawnProjectile(){

		if (currentFacing == Facing.Right) {
			projectilecopy = Instantiate (projectileprefab, transform.position + .5f * transform.right, Quaternion.identity) as GameObject;
			projectilecopy.GetComponent<Rigidbody2D> ().velocity = new Vector2 (10f, 0);
		}
		if (currentFacing == Facing.Left) {
			projectilecopy = Instantiate (projectileprefab, transform.position - .5f * transform.right, Quaternion.identity) as GameObject;
			projectilecopy.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-10f, 0);
		}

	}
	void FixedUpdate(){
		Direction cacheDirection;
		Facing cacheFacing;
		Attack cacheAttack;
		Vector2 tempv;

		tempv = rb.velocity;
		if (groundchange) {
			groundchange = false;
			tempv.y = 10f;
		}
		if (!groundchange) {
			grounded = Physics2D.OverlapCircle (groundCheck.position, 0.15f, whatIsGround); // checks if you are within 0.15 position in the Y of the ground
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

		rb.velocity = tempv;
	}


	

}
