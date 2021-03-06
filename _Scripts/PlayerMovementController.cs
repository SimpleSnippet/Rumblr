﻿using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

	//State control variables, and movement constants
	private float moveSpeed=1000f, rotateSpeed=5f, jumpSpeed=300f;
	private static bool jumping, attacking;

	//Variables for player animation and movement
	private static Rigidbody rb;
	private static Animator anim;

	// Use this for initialization
	private void Start () {
		jumping = false;

		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	private void Update () {
		
		//Controls movement animation
		anim.SetFloat ("speed", Input.GetAxis ("Vertical"));

		//Controls if the sword is active, and the attacking animations
		if (Input.GetMouseButton (0)) {
			anim.SetBool ("attack", true);
			StopAllCoroutines ();
			StartCoroutine (SetSwordActive ());
		} else {
			anim.SetBool ("attack", false);
		}

		//Controls player movement and rotation
		rb.AddForce (Input.GetAxisRaw("Vertical") * transform.forward * moveSpeed);
		transform.RotateAround (transform.position, Vector3.up, Input.GetAxis ("Horizontal") * rotateSpeed);

		//Controls jumping movement, if player is not already doing so
		if (Input.GetKey (KeyCode.Space) && !jumping) {
			jumping = true;
			rb.AddForce (Vector3.up * jumpSpeed, ForceMode.Impulse);
		}
	}

	//Sets the sword object active for length of current attacking animation being played
	private IEnumerator SetSwordActive(){
		GetComponentInChildren<MeshCollider> ().enabled = true;
		GetComponentInChildren<TrailRenderer> ().enabled = true;

		yield return new WaitForSeconds (1f);

		GetComponentInChildren<MeshCollider> ().enabled = false;;
		GetComponentInChildren<TrailRenderer> ().enabled = false;
	}

	private void OnCollisionEnter(Collision other){
		//Checks if I touched down, and allows me to jump again
		if (other.collider.CompareTag ("ground")) {
			jumping = false;
		}
	}
}
