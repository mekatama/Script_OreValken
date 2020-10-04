using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour{
	GameObject gameController;	//検索したオブジェクト入れる用

	//他のオブジェクトとの当たり判定(trigger)
	void OnTriggerEnter( Collider other) {
		if(other.tag == "Ground"){
			Destroy(gameObject);	//このGameObjectを［Hierrchy］ビューから削除する
		}
		if(other.tag == "Wall"){
			Destroy(gameObject);	//このGameObjectを［Hierrchy］ビューから削除する
		}
	}

}
