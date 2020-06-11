using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour{
	private Rigidbody rb;			//Rigidbody入れる用
	private Vector3 velocity;		//判定用のベクトル入れる用
	private Vector3 moveVelocity;	//移動用のベクトル入れる用
	private Animator animater;		//Animater入れる用
	public float speed = 10.0f;		//移動speed
	public bool isGround;			//接着flag

	void Start() {
		rb = GetComponent<Rigidbody>();
		animater = GetComponent<Animator>();//Animater取得
		isGround = false;					//初期化
		moveVelocity = Vector3.zero;		//初期化
	}

	void Update(){
		velocity = new Vector3(0f,0f,Input.GetAxis("Horizontal"));
		//ベクトルが0.1以上の場合判定
		if(velocity.magnitude > 0.1){
			//walk motion
			animater.SetFloat("speed",velocity.magnitude);
			//入力方向を向く
			transform.LookAt(transform.position + velocity);
			//左右入力
			moveVelocity += velocity.normalized * speed;
		}else{
			//idol motion
			animater.SetFloat("speed",0f);
		}
	}

	void FixedUpdate() {
		//実際に力を加える
		rb.MovePosition(transform.position + moveVelocity * Time.deltaTime);
	}
}
