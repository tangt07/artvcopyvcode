using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {
	float _rotation;
	public float rotation {
		get { return _rotation; }
		set { _rotation = value; }
	}
	float _velocity;
	public float velocity {
		get { return _velocity; }
		set { _velocity = value; }
	}
	Rigidbody2D prb;

	void Start(){
		prb = GetComponent<Rigidbody2D> ();

	}

	void FixedUpdate(){
		prb.MoveRotation(prb.rotation+_rotation*Time.fixedDeltaTime);
		prb.velocity = new Vector2(_velocity,0);
		if (Mathf.Abs (transform.position.x) > 12) {
			gameObject.SetActive (false);
		}
	}

    
}
