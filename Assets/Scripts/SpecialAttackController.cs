using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackController : MonoBehaviour {
	GameController gameController;
	EnemyGenerator enemyGenerator;
	
	private float del;
	// Use this for initialization
	void Start () {
	//生成されてからスクリプトファイル見つける
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
		enemyGenerator=GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		del+=Time.deltaTime;
		if(del>15){
			Destroy(gameObject);
		}
	}
	
	//必殺技(パーティクル)が敵に当たった時の処理
	void OnTriggerEnter(Collider other){
		Debug.Log("必殺技が当たったよ");
		//必殺技に当たったのが雑魚キャラの時の処理
		if(other.gameObject.tag=="enemy" || other.gameObject.tag=="enemy2"){
		Destroy(other.gameObject);//衝突した敵オブジェクトを破壊
		gameController.scoreCounter(100);//スコア加算
		enemyGenerator.enemyNumber--;//敵の数の値を減らす
		}
		//必殺技に当たったのがボスの時の処理
		if(other.gameObject.tag=="enemyBoss"){
			//Destroy(other.gameObject);
		}
	}
	
	void OnParticleCollision(GameObject other){
		Debug.Log("必殺技に敵が衝突した");
		/*
		if(other.gameObject.tag=="enemy" || other.gameObject.tag=="enemy2"){
			Destroy(other.gameObject);//衝突した敵オブジェクトを破壊
			gameController.scoreCounter(100);//スコア加算
			enemyGenerator.enemyNumber--;//敵の数の値を減らす
		}
		*/
	}
}
