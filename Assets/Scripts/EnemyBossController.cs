using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossController : MonoBehaviour {
	//CharacterController controller;

	//敵キャラ移動関連
	GameObject target;
	//public float speed;
	//float dif;
	
	//剣で攻撃関連
	public GameObject sword;
	public GameObject swordPosBlock;
	private float del;
	private float del2;
	public bool swordCollides;//剣と剣がぶつかったかどうか判定するため
	
	//必殺技関連
	Rigidbody rigid;
	public bool specialAttackHit=false;
	
	//プレイヤーの剣に攻撃された際のエフェクト処理
	public GameObject effectPre;
	GameObject effect;
	Vector3 effectPos;
	
	public int hitCount=0;//倒れるまでの受ける攻撃回数
	
	GameController gameController;//自分がやられたらスコアを他スクリプトに渡す
	
	//Hp関連
	public Slider hpSlider;
	public float hp=300;//下のほうで、二回攻撃を受けたら消えるという処理をしてるので2と設定

	// Use this for initialization
	void Start () {
		//controller=GetComponent<CharacterController>();
		target=GameObject.Find("Target of the BossEnemy");
		//生成されてからスクリプトファイルを見つける
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
		
		//必殺技を受けたときに少し飛ばす処理のため
		rigid=GetComponent<Rigidbody>();
		
		//ボスが生成されたことを登録
		PlayerPrefs.SetInt("BossExist", 1);//キーに対する値を設定する
		PlayerPrefs.Save();
		
		//必殺技実験のためで、終わったらコメントアウトする
		//gameController.scoreCounter(500);
		
		//Hpを最初はマックスにする
		hpSlider.value=1;

	}
	
	// Update is called once per frame
	void Update () {
		//ターゲットに向く処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		
		//剣で攻撃処理
		del2+=Time.deltaTime;//〜秒ごとに攻撃するかを決めるため
				if(del2>3){
				del+=Time.deltaTime;//剣を振り始める時間や元に戻す時間の計算のため
					if(del<0.3f && sword.transform.rotation.x<80){//約0.3秒間ゆっくり剣を振る		
					//引数は、回転の中心点、方向、角度を指定
						sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, 5);				
					}
					if(del>1 && del<1.3f){//剣を振り下ろした後に剣を戻す。
						sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, -5);
					}
					if(del>=1.3f){
					//フレーム時間を使ってるので、一回の攻撃を終えるたびに位置と角度を修正
					//sword.transform.position=new Vector3(1.2f,0.3f,-1.0f);
					//sword.transform.rotation=Quaternion.Euler(0,0,-20);
					//剣を戻した後に剣の時間関係をリセットし、攻撃したこともリセット	
						del=0;
						del2=0;
					}//剣を振り始める時間や元に戻す時間の計算のため
				}//〜秒ごとに攻撃するかを決めるため
		
				
        //50回分の攻撃を受けたら死亡し、シーン遷移する
			if(hitCount>=300){//300回攻撃されたら死亡
				Destroy(gameObject);
				gameController.scoreCounter(5000);
				gameController.gameClear=true;
			}
			
			//hp表示処理
			hpSlider.value=hp/300;
	}//update
	
		//必殺技に当たった時の処理
		public void ToBeAttacked(){//specialAttackControllerから呼ばれる
			//Debug.Log("少し飛ばすで");
			rigid.AddForce(transform.forward*-60);
			Invoke("StopMyself",3.0f);
		}
		
		//自分(敵)がノックバックした時に数秒後に停止する処理
		void StopMyself(){
			gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}

		//剣で攻撃された時の処理
		void OnTriggerEnter(Collider other) {
			if(other.gameObject.tag=="sword"){
			//Debug.Log("ボスが攻撃されてるで");
			hitCount++;
			hp--;
			gameController.scoreCounter(10);//ボスが剣で切られるたびに少しスコア加算			
		}	
	}	
	
	//剣が敵に当たった際にはエフェクトを表示する
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag=="sword"){
			//剣が当たった位置にエフェクトを発生させる
			foreach (ContactPoint point in other.contacts) {
				effectPos=(Vector3)point.point;
				effect=(GameObject)Instantiate(effectPre,effectPos,Quaternion.identity);
			}
		}
	}
}
