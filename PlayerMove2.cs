using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour{
	private Rigidbody rb;				//Rigidbody入れる用
	private Vector3 velocity;			//判定用のベクトル入れる用
	private Vector3 input;				//移動用のベクトル入れる用
	private Animator animater;			//Animater入れる用
	public float speed = 15.0f;			//移動speed
	public bool isGround;				//Rayでの接地flag
	public bool isGroundCollider;		//Rayとレイヤー接触flag
	public Transform charaRay;			//ray飛ばすオブジェクトの位置
	private float charaRayRange = 0.1f;	//rayの長さ

	void Start() {
		rb = GetComponent<Rigidbody>();
		animater = GetComponent<Animator>();//Animater取得
		isGround = false;					//初期化
		velocity = Vector3.zero;			//初期化
	}

	void Update(){
//		Debug.Log("isGroundCollider:" + isGroundCollider);
//		Debug.Log("isGround:" + isGround);
		//空中ではRay飛ばして判定
		if(isGroundCollider == false){
			//真下にcharaRayRange分ray飛ばして当たったらtrue
			if(Physics.Linecast(charaRay.position,(charaRay.position - transform.up * charaRayRange))){
				isGround = true;		//接地flag
				rb.useGravity = true;	//重力on
			}else{
				isGround = false;		//接地flag
				rb.useGravity = false;	//重力off jump中は独自に重力的なものを発生させるため
			}
		}
		Debug.DrawLine(charaRay.position,(charaRay.position - transform.up * charaRayRange),Color.red);

		//collisionが接地、またはrayが接地している場合
		if(isGroundCollider == true || isGround == true){
			//接地で初期化
			if(isGroundCollider == true){
				velocity = Vector3.zero;		//判定用ベクトル初期化
//				animater.SetBool("jump",false);	//idol motionへ
				rb.useGravity = true;			//重力on
			//rayだけの接地では重力だけ残す。多分、そのまま落下させてcollision接地に持ち込むため
			}else{
				velocity = new Vector3(0f,velocity.y,0f);
			}
			input = new Vector3(0f,0f,Input.GetAxis("Horizontal"));

			//ベクトルが0.1以上の場合判定
			if(input.magnitude > 0.1){
				//walk motion
				animater.SetFloat("speed",input.magnitude);
				//入力方向を向く
				transform.LookAt(transform.position + input);
				//左右入力
				velocity += input.normalized * speed;
			}else{
				//idol motion
				animater.SetFloat("speed",0f);
			}
		}

		//完全に空中にいるとき
		if(isGroundCollider == false && isGround == false){
			//Rigidbodyに設定した重力の値を使用
			velocity.y = Physics.gravity.y * Time.deltaTime;
		}
	}

	void FixedUpdate() {
		//実際に力を加える
		rb.MovePosition(transform.position + velocity * Time.deltaTime);
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