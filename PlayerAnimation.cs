using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour{
	private Vector3 velocity;	//判定用のベクトル入れる用
	private Animator animater;	//Animater入れる用
	private PlayerMove moveScript;	//PlayerMoveを参照

	void Start(){
		animater = GetComponent<Animator>();	//Animater取得
		moveScript = GetComponentInParent<PlayerMove>();//上の階層のオブジェクトのPlayerMove.cs参照
	}

    void Update(){
		velocity = new Vector3(0f,0f,Input.GetAxis("Horizontal"));
		//ベクトルが0.1以上の場合判定
		if(velocity.magnitude > 0.1){
			//walk motion
			animater.SetFloat("speed",velocity.magnitude);
			//入力方向を向く
			transform.LookAt(transform.position + velocity);
		}else{
			//idol motion
			animater.SetFloat("speed",0f);
		}

		//jump motion制御
		if(moveScript.isGround == false){
			//jump motion
			animater.SetBool("jump",true);
		}else{
			//idol motion
			animater.SetBool("jump",false);
		}

	}
}
