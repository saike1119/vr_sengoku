using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackController : MonoBehaviour {
	GameController gameController;//スコア加算のため
	EnemyGenerator enemyGenerator;//敵の数を表す値を減らすため
	EnemyBossController enemyBossController;	//ボスに必殺技を当てた時のダメージ処理など

	private float del;
	
	//必殺技関連
	public GameObject enemyPos;
	public GameObject enemyPos2;
	private bool specialAttackHit=false;
	
	// Use this for initialization
	void Start () {
	//生成されてからスクリプトファイル見つける
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
		enemyGenerator=GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();		
		
	}
	
	// Update is called once per frame
	void Update () {
		del+=Time.deltaTime;
		
		//必殺技(竜巻)を発生させてから5秒たったら消す
		if(del>8){
			Destroy(gameObject);
		}
		
		//ボスが生成されたタイミングでボスのコンポーネントを取得して処理を始めていく
		if(PlayerPrefs.GetInt("BossExist")==1){
		enemyBossController=GameObject.Find("samuzai_animation_ok(Clone)").GetComponent<EnemyBossController>();


		}
	}
	
	//必殺技(パーティクル)が敵に当たった時の処理
	void OnTriggerEnter(Collider other){
		//Debug.Log("必殺技が当たったよ");
		
		//必殺技に当たったのが雑魚キャラの時の処理
		if(other.gameObject.tag=="enemy"){
			//竜巻に当たったら敵を押すだけで破壊しない(吸い込みたいから)
			gameController.scoreCounter(100);//スコア加算
			//Destroy(other.gameObject);//衝突した敵オブジェクトを破壊
			//enemyGenerator.enemyNumber--;//敵の数の値を減らす
		}
		/*
		//必殺技に当たったのがボスの時の処理
		if(other.gameObject.tag=="enemyBoss" && specialAttackHit==false){
			specialAttackHit=true;
			enemyBossController.ToBeAttacked();//ボスをノックバックさせる関数を呼び出し
			enemyBossController.hitCount+=10;//ヒットした回数を増やす(5から二倍の10にした)
			enemyBossController.hp-=10;
			enemyBossController.specialAttackHit=true;
			gameController.scoreCounter(100);//スコア加算
			Debug.Log("ヒーローの必殺技がボスに当たったで");
			Invoke("NotAttack",1.0f);
		}
		*/
	}
	
	//必殺技が当たっている最中の処理(敵を必殺技に吸い込むブラックホール的存在作成)
    void OnTriggerStay(Collider other){
        if (other.gameObject.tag=="enemy"){
        	if(other.transform.position.x<transform.position.x){//敵が竜巻より右にいたら
        	other.gameObject.transform.position=enemyPos.transform.position;      
        	}else{
        	other.gameObject.transform.position=enemyPos2.transform.position;             		
        	}
        }
    }
    
    	//必殺技が当たったかどうかをリセットする
		void NotAttack(){
			specialAttackHit=false;
			//enemyBossController2.specialAttackHit=false;
		}
}
