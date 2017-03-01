using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackController : MonoBehaviour {
	GameController gameController;//スコア加算のため
	EnemyGenerator enemyGenerator;//敵の数を表す値を減らすため
	EnemyBossController enemyBossController;	//ボスに必殺技を当てた時のダメージ処理など
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
		if(del>2.5){
			Destroy(gameObject);
		}
		
		//ボスが生成されたタイミングでコンポーネントを取得して処理を始めていく
		if(PlayerPrefs.GetInt("BossExist")==1){
		enemyBossController=GameObject.Find("enemyBoss(Clone)").GetComponent<EnemyBossController>();

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
			enemyBossController.ToBeAttacked();
			enemyBossController.hitCount+=5;
			gameController.scoreCounter(20);//スコア加算
			Debug.Log("ヒーローの必殺技が当たったで");
		}
	}
	/*
	void OnParticleCollision(GameObject other){
		Debug.Log("必殺技に敵が衝突した");
		
		if(other.gameObject.tag=="enemy" || other.gameObject.tag=="enemy2"){
			Destroy(other.gameObject);//衝突した敵オブジェクトを破壊
			gameController.scoreCounter(100);//スコア加算
			enemyGenerator.enemyNumber--;//敵の数の値を減らす
		}
		
	}
	*/
}
