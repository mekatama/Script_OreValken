using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{
	public float speed = 10.0f;
	public Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		float z =  Input.GetAxis("Horizontal") * speed;
		//実際に力を加える
		rb.AddForce(0 , 0 , z );
	}
}
