using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{
	public float speed = 10.0f;		//移動speed
	public float jumpPower = 250.0f;//jump力
	public Rigidbody rb;			//Rigidbody入れる用
	public bool isGround;			//接着flag

	void Start() {
		rb = GetComponent<Rigidbody>();
		isGround = false;	//初期化
	}

	void FixedUpdate() {
		Debug.Log(isGround);
		//左右入力
		float z =  Input.GetAxis("Horizontal") * speed;
		//jump入力
		if(isGround == true){
			if(Input.GetKeyDown(KeyCode.Space)){
				isGround = false;
				//実際に力を加える
				rb.AddForce(Vector3.up * jumpPower);
			}
		}
		//実際に力を加える
		rb.AddForce(0 , 0 , z );
	}

	//接着判定
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "Ground"){
			isGround = true;
		}
	}
}
