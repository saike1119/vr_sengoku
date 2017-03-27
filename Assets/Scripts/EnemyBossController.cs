using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossController : MonoBehaviour {
	//CharacterController controller;

	//敵キャラ移動関連
	GameObject target;
	public float speed;
	
	//剣で攻撃関連
	private float del;
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
	public float hp=50;//下のほうで、二回攻撃を受けたら消えるという処理をしてるので2と設定
	
	//アニメーション関連
	Animator animator;
	Animation anim;
	private bool walked=false;
	private bool attacked=false;
	public bool dead=false;
	public bool swordCollided=false;

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
		
		//アニメーション関係
		animator=GetComponent<Animator>();
		anim=GetComponent<Animation>();
		//anim.Play("Walk");//最初は歩くアニメーション再生

	}
	
	// Update is called once per frame
	void Update () {
		//ターゲットに向く処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		
		//ターゲットから遠かったら移動する
		if(transform.position.z>4){
			transform.position += transform.forward *Time.deltaTime* speed;//前へ移動
		}else{//ターゲットに近くなったら待機アニメーションスタート
				del+=Time.deltaTime;//このタイミングで、下の攻撃のための時間計算
				if(walked==false && dead==false){
					walked=true;
					//anim.Play("idle");
				}
		}
		//Debug.Log(del);
		//剣で攻撃処理
		if(del>3){//3秒になったら攻撃
			if(attacked==false && dead==false){//死亡しても時間になったら攻撃するのを防ぐ
				//anim.Play("Attack");//3秒ごとに攻撃
				//anim.Play("samurai_Dying");
				attacked=true;
			}
		}
		//3秒で攻撃開始して、それにアニメーションの秒数を考慮した時間になったらリセット
			if(del>4.5){
			del=0;
			}
		//3秒以内なら待機アニメーションに戻る
		if(del<3){
			if(attacked==true && dead==false){
				attacked=false;
			//anim.Play("idle");		
			}	
		}
			
				
        //50回分の攻撃を受けたら死亡し、シーン遷移する
			if(hitCount>=50){//50回攻撃されたら死亡
				Destroy(gameObject);
				gameController.scoreCounter(5000);
				gameController.gameClear=true;
			}
			
			//hp表示処理
			hpSlider.value=hp/50;
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

		//通常ダメージ処理。剣で攻撃された時の処理
		void OnCollisionEnter(Collision other) {
			if(other.gameObject.tag=="sword"){
				if (swordCollided == false) {//死んでもなくて剣と剣がぶつかってなかったら
					//剣が当たった位置にエフェクトを発生させる
					foreach (ContactPoint point in other.contacts) {
					effectPos=(Vector3)point.point;
					effect=(GameObject)Instantiate(effectPre,effectPos,Quaternion.identity);
					}
				hitCount++;
				hp--;
				gameController.scoreCounter(10);//ボスが剣で切られるたびに少しスコア加算
			}
		}
	}

	//swordControllerから呼ばれる、剣と剣がぶつかったときに、ダメージと同じアニメーションを再生する
	public void SwordCollided(){
		//anim.Play("samurai_backwards");//ダメージを受けたみたいなアニメーション再生
	}
	
}
