using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour{
	private Vector3 velocity;	//判定用のベクトル入れる用
	private Animator animater;	//Animater入れる用

	void Start(){
		animater = GetComponent<Animator>();	//Animater取得
	}

    void Update(){
		velocity = new Vector3(0f,0f,Input.GetAxis("Horizontal"));
		//ベクトルが0.1以上の場合判定
		if(velocity.magnitude > 0.1){
			animater.SetFloat("speed",velocity.magnitude);
		}else{
			animater.SetFloat("speed",0f);
		}
	}
}
