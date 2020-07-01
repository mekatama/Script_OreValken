using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPod : MonoBehaviour{
	private float speed = 1.8f;	//角度変化速度調整用
	void Start(){
	}

	void Update(){
		gameObject.transform.Rotate(Input.GetAxis("Vertical") * speed, 0, 0);
	}
}
