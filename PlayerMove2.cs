using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour{
	private Rigidbody rb;			//Rigidbody入れる用
	private Vector3 velocity;		//判定用のベクトル入れる用
	private Vector3 moveVelocity;	//移動用のベクトル入れる用
	private Animator animater;		//Animater入れる用
	public float speed = 10.0f;		//移動speed
	public bool isGround;			//Rayの接地flag
	public bool isGroundCollider;	//Rayの接地flag
	public Transform charaRay;		//ray飛ばすオブジェクトの位置
	private float charaRayRange = 0.1f;	//rayの長さ

	void Start() {
		rb = GetComponent<Rigidbody>();
		animater = GetComponent<Animator>();//Animater取得
		isGround = false;					//初期化
		moveVelocity = Vector3.zero;		//初期化
	}

	void Update(){
Debug.Log("isGroundCollider:" + isGroundCollider);
		//空中ではRay飛ばして判定
		
		Debug.DrawLine(charaRay.position,(charaRay.position - transform.up * charaRayRange));

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

	//CapsuleColliderが他のcolligionと接触
	void OnCollisionEnter(){
		Debug.DrawLine(charaRay.position,charaRay.position + Vector3.down,Color.blue);
		//他のcollisionと接触で↓にrayを飛ばし指定レイヤーの時をtrue
		if(Physics.Linecast(charaRay.position,
							charaRay.position + Vector3.down,
							LayerMask.GetMask("Ground","Block"))){
			isGroundCollider = true;
		}else{
			isGroundCollider = false;
		}
	}

	//CapsuleColliderが空中なら
	void OnCollisionExit(){
		isGroundCollider = false;
	}
}