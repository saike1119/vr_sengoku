using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	GameObject target;
	public float speed;
	CharacterController controller;
	float dif;
	Rigidbody rigid;
	
	private int hitCount=0;
	
	GameController gameController;

	// Use this for initialization
	void Start () {
		controller=GetComponent<CharacterController>();
		target=GameObject.Find("MeMySelf");
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
	}
	
		// Update is called once per frame
	void Update () {
        //ターゲットとある程度近くなったら止まって攻撃
        dif=transform.position.z-target.transform.position.z;
		
		//ターゲットに向かっていく処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		//ターゲットとの距離が近かった時の処理
		 if(dif<7){
        //Debug.Log("近いよ君！");
        }else{//ターゲットがまだ遠かったらターゲットに近づく
        transform.position += transform.forward *Time.deltaTime* speed;//ターゲットの方へ移動させる処理
        }
	}


//主人公の剣が当たったかどうか判定とその後の処理
void OnCollisionEnter(Collision other){
	if(other.gameObject.tag=="sword"){
		hitCount++;
	Debug.Log("ヒーローの剣に当たってしまった");
		if(hitCount==4){//四回攻撃されたら死亡
			Destroy(gameObject);
			gameController.scoreCounter(100);
		}
	}
}
/*
//敵同士が近くてめり込みのを防ぐ
void OnTriggerStay(Collider other){
	rigid=other.GetComponent<Rigidbody>();
	//味方が自分の左にいるならもう少し左に行ってもらう
	if(transform.position.x>other.transform.position.x){
	rigid.AddForce(transform.right*-100);
	//味方が自分の右にいるならもう少し右に行ってもらう
	}else{
		rigid.AddForce(transform.right*100);
	}
	
}
*/
	
	
}
