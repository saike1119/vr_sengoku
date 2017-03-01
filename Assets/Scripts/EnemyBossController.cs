using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour {
	//敵キャラ移動関連
	GameObject target;
	public float speed;
	float dif;
	//CharacterController controller;
	//必殺技関連
	Rigidbody rigid;
	private bool specialAttackHit=false;
	
	public int hitCount=0;//倒れるまでの受ける攻撃回数
	
	GameController gameController;//自分がやられたらスコアを他スクリプトに渡す
	EnemyGenerator enemyGenerator;//自分がやられたら敵の残り数の表示を更新するため

	// Use this for initialization
	void Start () {
		//controller=GetComponent<CharacterController>();
		target=GameObject.Find("Target of the BossEnemy");
		//生成されてからスクリプトファイルを見つける
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
		enemyGenerator=GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
		
		//必殺技を受けたときに少し飛ばす処理のため
		rigid=GetComponent<Rigidbody>();
		
		//ボスが生成されたことを登録
		PlayerPrefs.SetInt("BossExist", 1);//キーに対する値を設定する
		PlayerPrefs.Save();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(hitCount);
        //ターゲットとある程度近くなったら止まって攻撃
        dif=transform.position.z-target.transform.position.z;
		
		//ターゲットに向かっていく処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
				
		//ターゲットとの距離が近かった時の処理
		 if(dif<4.5){//4だった
        //Debug.Log("近いよ君！");
        }else{//ターゲットがまだ遠かったらターゲットに近づく
        transform.position += transform.forward *Time.deltaTime* speed;//ターゲットの方へ移動させる処理
        }
        //50回分の攻撃を受けたら死亡し、シーン遷移する
			if(hitCount>=300){//300回攻撃されたら死亡
				Destroy(gameObject);
				gameController.scoreCounter(5000);
				gameController.gameClear=true;
			}
			

	}//update
	
		//必殺技に当たった時の処理
		public void ToBeAttacked(){
			if(specialAttackHit==false){
				specialAttackHit=true;
			Debug.Log("少し飛ばすで");
			//transform.Translate(0,0,-30);
			rigid.AddForce(transform.forward*-600);
			Invoke("NotAttack",5.0f);
			}
		}
		
		void NotAttack(){
			specialAttackHit=false;
		}


	void OnTriggerEnter(Collider other) {
			if(other.gameObject.tag=="sword"){
			//Debug.Log("ボスが攻撃されてるで");
			hitCount++;
		gameController.scoreCounter(10);//ボスが剣で切られるたびに少しスコア加算
		}	
	}	
}
