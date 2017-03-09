using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {
	//剣と剣が衝突したよきに自分(敵)を押す処理のための
	Rigidbody rigid;
	GameObject enemy;
		
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(enemy.transform.position);
	}
	
	//剣と剣がぶつかった時の処理(エフェクトの衝突位置がコリジョンじゃないと取得できない)
	void OnCollisionEnter(Collision other){
		/*
		if(other.gameObject.tag=="enemyWeapon"){//敵側の処理
		Debug.Log("敵の武器がぶつかったね");
		enemy=other.gameObject.transform.parent.gameObject;//衝突した剣の親オブジェクト取得
		rigid=enemy.GetComponent<Rigidbody>();
		rigid.AddForce(transform.forward*-300);
		}
		*/
	}
}
