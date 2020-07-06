using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour{
	public GameObject bulletObject = null;		//弾のプレハブ
	public Transform bulletStartPosition = null;//弾の発射位置
	private float timeElapsed = 0.0f;			//弾の連射間隔カウント用
	public float timeOut;						//連射間隔の時間

	void Start(){
	}

	void Update(){
		Shot();
	}

	void Shot(){
		timeOut = 0.4f;					//連射間隔設定
		timeElapsed += Time.deltaTime;	//カウント
		if(timeElapsed >= timeOut){
			//弾の生成位置を指定
			Vector3 vecBulletPos = bulletStartPosition.position;
			//弾を生成
			Instantiate(
						bulletObject,
						vecBulletPos,
						transform.rotation
						);
			timeElapsed = 0.0f;			//初期化
		}
	}
}