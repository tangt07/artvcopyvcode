using UnityEngine;
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
	public GameObject genericprojectileprefab;
	public Sprite sprite;
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
	public bool backrightjump = false;
	public bool backleftjump = false;
	public bool stopmoving;

	public float shieldhealth = 5f;
	public bool brokenshield;
	public int numprojectiles;
	public GameObject enemy;
	public bool pushback;
	bool lightpunch;
	bool heavypunch;
	bool knockdown;
	bool knockdownsecond;

	public bool redprojectilebar;


	void Awake(){
		int ForwardState = Animator.StringToHash("Base Layer.RightForward");
		int ForwardMirroredState = Animator.StringToHash ("Base Layer.LeftForward");

	}
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator>();


	}
	void Update(){
		shieldup = (grounded && 
		            (currentFacing == Facing.Right && (currentDirection == Direction.Left || currentDirection == Direction.LeftDown)) ||
		            (currentFacing == Facing.Left && (currentDirection == Direction.Right || currentDirection == Direction.RightDown)));
		if (redprojectilebar && numprojectiles >= 10) {
			redprojectilebar = false;
		}
		if (!shieldup && stopmoving) {
			stopmoving = false;
		}
		if(currentAttack == Attack.LightCombat||currentAttack==Attack.HeavyCombat){
			anim.SetTrigger("Attack");
			if (currentAttack == Attack.LightCombat) {
				lightpunch = true;
			}
			if (currentAttack == Attack.HeavyCombat) {
				heavypunch = true;
			}
		}
		
		if(currentAttack == Attack.Projectile && numprojectiles > 0 && !redprojectilebar){
			anim.SetTrigger("Projectile");
			SpawnProjectile ();
		}
		
		if(currentAttack == Attack.CombatProjectile){
			anim.SetTrigger("Projectile");
			SpawnProjectile ();
		}

		SetAnimDirectionBools (currentDirection);
		SetJumpingBools (grounded, currentDirection);
		SetFacingBools (currentFacing); 

	}
	void ThrowPunch(){
		Vector2 point = new Vector2 (transform.position.x, transform.position.y);
		float radius = 1f;
		Collider2D[] results = new Collider2D[100];
		int hits = 0;

		float punchmultiplier=1f;
		float punchdamage = 1f;

		if (lightpunch) {
			lightpunch = false;
			punchmultiplier = 1.25f;
			punchdamage = 1f;
		}

		if (heavypunch) {
			heavypunch = false;
			punchmultiplier = 1.1f;
			punchdamage = 4f;
		}
		Dictionary<int,GameObject> enemies = new Dictionary<int, GameObject>();
		int numcolliders = Physics2D.OverlapCircleNonAlloc (point,radius*punchmultiplier,results); 
		for (int i = 0; i < numcolliders; i++) {
			if(results[i].transform.root != transform.root){
				if (results[i].gameObject.tag == "Player") {
					enemies[results[i].gameObject.GetInstanceID()] = results [i].gameObject;
				}
			}
		}
		foreach (GameObject enemy in enemies.Values) {
			enemy.GetComponent<PlayerMovement> ().Take_Damage (punchdamage);
		}
	}
	void SpawnProjectile(){
		projectilecopy = GameObjectPoolManager.current.GetPooledObject ();
		if (projectilecopy) {
			Projectile behavior = projectilecopy.GetComponent<Projectile> ();
		
			if (currentFacing == Facing.Right) {
				projectilecopy.transform.position = transform.position + .5f * transform.right;
				behavior.rotation = -720f;
				behavior.velocity = 10f;
			}
			if (currentFacing == Facing.Left) {
				projectilecopy.transform.position = transform.position - .5f * transform.right;
				behavior.rotation = 720f;
				behavior.velocity = -10f;
			}
			projectilecopy.GetComponent<SpriteRenderer> ().sprite = sprite;
			projectilecopy.SetActive (true);

			numprojectiles--;
			if (numprojectiles <= 0) {
				redprojectilebar = true;
			}
		}
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
			backrightjump = true;
			tempv = new Vector2(5f,5f);
			enemy.GetComponent<PlayerMovement> ().pushback = true;
		}
		if (!grounded && tempv.y == 0 && cacheFacing == Facing.Right) {
			//if lands on something not the ground
			backleftjump = true;
			tempv = new Vector2(-5f,5f);

			enemy.GetComponent<PlayerMovement> ().pushback = true;
		} 
		if (shieldup && stopmoving && !brokenshield) {
			tempv.x = 0;
		}

		if (pushback) {
			if (currentFacing == Facing.Left) {
				tempv.x = 10f;
			}
			if (currentFacing == Facing.Right) {
				tempv.x = -10f;
			}
			StartCoroutine ("StopPushback");

		}
		rb.velocity = tempv;
	}
	IEnumerator StopPushback(){
		yield return new WaitForSeconds(.05f);
		pushback = false;
	}
	public void Take_Damage(float damage = 1f){
		stopmoving = true;

		if (shieldup && !brokenshield) {
			shieldhealth -= damage;
		
			if (shieldhealth <= 0) {
				brokenshield = true;
				stopmoving = false;
				anim.SetBool ("ShieldBroken", true);
				StopCoroutine ("RegainShield");
				StartCoroutine ("RegainShield");

			}
		} 
		else {
			PlayerMovement enemymovement = enemy.GetComponent<PlayerMovement> ();

			if (enemymovement.redprojectilebar) {
				enemymovement.numprojectiles+=2;
			}
			pushback = true;
			GetComponent<PlayerHealth> ().current_health -= damage;
			anim.SetFloat ("Health", GetComponent<PlayerHealth> ().current_health);

			if (GetComponent<PlayerHealth> ().current_health <= 0) {
				anim.SetTrigger ("Dead");
			}else{
				if (damage == 1f) {
					anim.SetTrigger ("Damage");
				}
				if (damage == 4f) {
					
					knockdownsecond = false;

					anim.SetBool ("KnockdownSecond", false);

					anim.SetTrigger ("Knockdown");
					StopCoroutine("OneSecondKnockdown");
					StartCoroutine ("OneSecondKnockdown");
				}
			}		
			if (GetComponent<PlayerHealth> ().current_health <= 0 && !GetComponent<PlayerHealth> ().dead) {
				GetComponent<PlayerHealth> ().Lose_Life ();
			}
		}

	}
	IEnumerator RegainShield(){
		yield return new WaitForSeconds(3f);
		shieldhealth = 5f;
		brokenshield = false;
		anim.SetBool ("ShieldBroken", false);
	}


	IEnumerator OneSecondKnockdown(){
		yield return new WaitForSeconds (1f);
		knockdownsecond = true;
		anim.SetBool ("KnockdownSecond", true);
	}
	void OnTriggerStay2D(Collider2D col){


		/*
		if (col.CompareTag ("Wall")) {
			if (backrightjump) {
				backrightjump = false;
				rb.velocity = new Vector2 (-10f, 5f);

			}

			if (backleftjump) {
				backleftjump = false;
				rb.velocity = new Vector2 (10f, 5f);

			}
		}
		*/
	}



	

}
