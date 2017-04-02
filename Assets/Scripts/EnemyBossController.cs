using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	
	public int hitCount=0;//のけぞるまでの攻撃を受けた回数格納
	
	GameController gameController;//自分がやられたらスコアを他スクリプトに渡す
	
	//Hp関連
	public Slider hpSlider;
	public float hp=100;//下のほうで、二回攻撃を受けたら消えるという処理をしてるので2と設定
	
	//アニメーション関連
	Animator animator;
	Animation anim;
	private bool walked=false;
	private bool attacked=false;
	public bool dead=false;
	public bool swordCollided=false;
	private bool swordCollidedOk=true;//ボスが他のアニメーションを切り替える時に剣がプレイヤーの剣に当たってしまって永遠にダメージアニメーションが再生されてしまうので、その対策。剣を弾く場面はボスが攻撃する時しかないはずだから。
	private bool interval=false;//ボスが攻撃をする時だけ剣と剣を弾くアニメーションを再生するかを許可したが、その間に連続で剣と剣が衝突するとダメージアニメーションが連続で再生されてしまうのでインターバルを設ける
	
	//音声関連
	AudioSource aud;
	public AudioClip[] se;

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
		anim=GetComponent<Animation>();//使わなくなった
		
		//音声のコンポーネント取得
		aud=GetComponent<AudioSource>();
		aud.PlayOneShot(se[3]);
	}
	
	// Update is called once per frame
	void Update () {
		//ターゲットに向く処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		
		//ターゲットから遠かったら移動する
		if(transform.position.z>2.0){
			transform.position += transform.forward *Time.deltaTime* speed;//前へ移動
		}else{//ターゲットに近くなったら待機アニメーションスタート
				del+=Time.deltaTime;//このタイミングで、下の攻撃のための時間計算
				aud.Stop();//足音の秒数が長いので、目的地に着いたら音声終了
				if(walked==false && dead==false){
					walked=true;
					animator.SetTrigger("idle");
				}
		}
		//Debug.Log(del);
		//剣で攻撃処理
		if(del>1.5){//3秒になったら攻撃
			if(attacked==false && dead==false){//死亡しても時間になったら攻撃するのを防ぐ
				swordCollidedOk=false;//ボスの攻撃時のみ剣と剣の衝突時アニメーション許可
				animator.SetTrigger("Attack");
				if(swordCollided==true) animator.SetTrigger("idle");//ボスが攻撃した時にプレイヤーが剣で防いだ場合、強制的にidleアニメーション再生して攻撃中止
				attacked=true;
			}
		}
		//3秒で攻撃開始して、それにアニメーションの秒数を考慮した時間になったらリセット
			if(del>3){
				swordCollidedOk=true;
				del=0;
			}
		//3秒以内なら待機アニメーションに戻る
		if(del<1.5){
			if(attacked==true && dead==false){
				attacked=false;
			animator.SetTrigger("idle");		
			}	
		}
						
			//hp表示処理
			hpSlider.value=hp/100;
	}//update

/*
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
*/
		//通常ダメージ処理。剣で攻撃された時の処理
		void OnCollisionEnter(Collision other) {
			if(other.gameObject.tag=="sword" && dead==false){
				if (swordCollided == false) {//死んでもなくて剣と剣がぶつかってなかったら
				if(hp>1){//ボスが死亡する前までは普通のダメージアニメーション再生など
				aud.PlayOneShot(se[0]);
				if(hitCount==5){//5回攻撃を受けるごとに仰け反るアニメーションを再生
				animator.SetTrigger("Back");
				}
				Invoke("DelayIdle",0.5f);
				Invoke("SetInterval",3.0f);
				}else{//ボスが死亡する最後の一撃は、死亡アニメーション再生など
					dead=true;
					animator.SetTrigger("Dead");
					Invoke("DelayDeadAudio",1.8f);
					Invoke("DelayAudio",4.0f);

				}
					//剣が当たった位置にエフェクトを発生させる
					foreach (ContactPoint point in other.contacts) {
					effectPos=(Vector3)point.point;
					effect=(GameObject)Instantiate(effectPre,effectPos,Quaternion.identity);
					}
				hitCount++;
				hp--;
				if(hitCount>=5) hitCount=0;
				//hp-=100;//死亡アニメーション再生実験
				gameController.scoreCounter(10);//ボスが剣で切られるたびに少しスコア加算
			}
		}
	}
	
	//仰け反る(ダメージ時)アニメーションを再生した後すぐidleアニメーションを再生したい
	void DelayIdle(){
		animator.SetTrigger("idle");
	}
	
	void DelayDeadAudio(){
		aud.PlayOneShot(se[2]);//地面に倒れた時の音
}
	
	//勝利決定音声再生
	void DelayAudio(){
		//勝利決定時の音声を再生してからオブジェクト削除とシーン遷移
		aud.PlayOneShot(se[1]);//勝利決定の音
		Invoke("DelayDestroyer",6);
	}
	
	void DelayDestroyer(){
				//Destroy(gameObject);
				gameController.scoreCounter(5000);
				gameController.gameClear=true;//GameControllerの方でシーン遷移するため
	}
	
	//swordControllerから呼ばれる、剣と剣がぶつかったときに、ダメージと同じアニメーションを再生する
	public void SwordCollided(){
		if(swordCollidedOk==false && interval==false){//ボスが攻撃した時だけ弾くアニメーションが再生
			interval=true;
			animator.SetTrigger("Back");
			Invoke("DelayIdle",0.5f);//弾くアニメーションを再生したらすぐidleに戻す
			Invoke("SetInterval",3.0f);
		}
	}	
	
	void SetInterval(){
		interval=false;
	}
}
