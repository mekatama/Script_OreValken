using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour{
	private Rigidbody rb;				//Rigidbody入れる用
	private Vector3 velocity;			//判定用のベクトル入れる用
	private Vector3 input;				//移動用のベクトル入れる用
	private Animator animator;			//Animater入れる用
	public float speed = 15.0f;			//移動speed
	public float jumpPower = 5.0f;		//jump力
	public bool isGround;				//Rayでの接地flag
	public bool isGroundCollider;		//Rayとレイヤー接触flag
	public Transform charaRay;			//ray飛ばすオブジェクトの位置
	private float charaRayRange = 0.15f;//rayの長さ
	private float jumpCount = 0.0f;		//滞空判定用カウンター
	private float jumpCountEnd = 0.2f;	//滞空開始
	private float flyCount = 0.0f;		//滞空制限用カウンター
	private float flyCountEnd = 1.0f;	//滞空制限時間
	public float dashSpeed = 30.0f;				//[dash]dashSpeed
	public bool isDash = false;					//[dash]dash flag
	public bool push = false;					//[dash]最初に移動ボタンを押したかどうか
	public float nextButtonTime = 0.3f;			//[dash]次ボタン入力判定までの時間
	private float nowTimeCount = 0f;			//[dash]入力判定用カウンター
	public float limitAngle = 3f;				//[dash]入力角度許容誤差
	private Vector2 direction = Vector2.zero;	//[dash]移動キー保存用

	void Start() {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();//Animater取得
		isGround = false;					//初期化
		velocity = Vector3.zero;			//初期化
	}

	void Update(){
//		Debug.Log("isGroundCollider:" + isGroundCollider);
//		Debug.Log("isGround:" + isGround);
//		Debug.Log("jumpCount:" + jumpCount);
//		Debug.Log("push : " + push);
		Debug.Log("isDash : " + isDash);
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
			Debug.DrawLine(charaRay.position,(charaRay.position - transform.up * charaRayRange),Color.red);
		}

		//collisionが接地、またはrayが接地している場合
		if((isGroundCollider == true) || (isGround == true)){
			//接地で初期化
			if(isGroundCollider == true){
				velocity = Vector3.zero;		//判定用ベクトル初期化
				jumpCount = 0;					//滞空用カウンター初期化
				flyCount = 0;					//滞空制限用カウンター
				animator.SetBool("jump",false);	//idol motionへ
				rb.useGravity = true;			//重力on
			//rayだけの接地では重力だけ残す。多分、そのまま落下させてcollision接地に持ち込むため
			}else{
				velocity = new Vector3(0f,velocity.y,0f);
			}

			//dash入力判定
			if(isDash == false){					//dashしていない場合
				if(Input.GetButtonDown("Horizontal")){	//左右入力されたら
					//最初の入力の場合
					if(push == false){
						push = true;
						direction = new Vector2(Input.GetAxis("Horizontal"),0f);	//入力した方向を保存
						nowTimeCount = 0f;											//dash判定カウント初期化
					}
					//2回目の入力の場合
					else{
						var nowDirection = new Vector2(Input.GetAxis("Horizontal"),0f);	//2回目に入力した方向を保存
//						Debug.Log(Vector2.Angle(nowDirection,direction));
						//入力角度誤差内で制限時間内ならdash
						if(Vector2.Angle(nowDirection,direction) < limitAngle && nowTimeCount <= nextButtonTime){
							isDash = true;	//dash flag on
							animator.SetBool("dash",true); //dash motion 切り替え
						}
					}
				}
			}
			//dash時にキー入力をやめたらdash終了
			else{
				if(Input.GetButton("Horizontal") == false){	//左右入力終わったら
					isDash = false;
					push = false;
					animator.SetBool("dash",false); //dash motion 切り替え
				}
			}

			//dashの最初のキー入力でカウント開始
			if(push == true){
				nowTimeCount += Time.deltaTime;
				//制限時間をオーバーしたらキャンセル
				if(nowTimeCount > nextButtonTime){
					push = false;
				}
			}

			//左右入力
			input = new Vector3(0f,0f,Input.GetAxis("Horizontal"));

			//左右移動
			//ベクトルが0.1以上の場合判定
			if(input.magnitude > 0.1f){
				//walk motion
				animator.SetFloat("speed",input.magnitude);
				//入力方向を向く
				transform.LookAt(transform.position + input);
				//左右入力(walkとdash)
				if(isDash == true){
					//dash
					velocity += input.normalized * dashSpeed;
				}else{
					//walk
					velocity += input.normalized * speed;
				}
			}else{
				//idol motion
				animator.SetFloat("speed",0f);
			}

			//jump入力
			if(Input.GetKeyDown(KeyCode.Space)){
				animator.SetBool("jump",true);	//jump motion
				velocity.y += jumpPower;		//上昇力
				rb.useGravity = false;			//重力off
			}

		}

		//完全に空中にいるとき
		if((isGroundCollider == false) && (isGround == false)){
			//滞空判定
			jumpCount += Time.deltaTime;
			//jumpおしっぱ判定
			if(Input.GetKey(KeyCode.Space)){
				//jump開始から一定カウント後
				if(jumpCount >= jumpCountEnd){
					//滞空制限判定
					flyCount += Time.deltaTime;
					//滞空開始から一定カウント内だけ滞空する
					if(flyCount <= flyCountEnd){
						//一度、上昇力を強制的に小さく
						if(velocity.y > 0){
							velocity.y = 0.5f;
						}
						Debug.Log("taikuu");
	//					animator.SetBool("jump",true);	//jump motion
						velocity.y += (jumpPower * 0.008f);		//上昇力
						rb.useGravity = false;			//重力off
					}else{
						Debug.Log("fall");
						//Rigidbodyに設定した重力の値を使用
						velocity.y += Physics.gravity.y * Time.deltaTime;
					}
				}						
			}else{
				Debug.Log("fall");
				//Rigidbodyに設定した重力の値を使用
				velocity.y += Physics.gravity.y * Time.deltaTime;
			}
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