using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	//敵キャラ移動関連
	GameObject target;
	public float speed;
	float dif;
	//CharacterController controller;
	//Rigidbody rigid;
	

	private float del;
	private float del2;
		
	private int hitCount=0;//倒れるまでの受ける攻撃回数
	
	//Hp関連
	public Slider hpSlider;
//下のほうで、二回攻撃を受けたら消えるという処理をしてるので2と設定	
	private float hp=6;//115行目も値を変更する
	
	//プレイヤーの剣に攻撃された際のエフェクト処理
	public GameObject effectPre;
	GameObject effect;
	Vector3 effectPos;
	
	GameController gameController;//自分がやられたらスコアを他スクリプトに渡す
	EnemyGenerator enemyGenerator;//自分がやられたら敵の残り数の表示を更新するため
	
	//アニメーション関連
	Animator animator;
	Animation anim;
	private bool walked=false;
	private bool attacked=false;
	public bool dead=false;
	public bool swordCollided=false;
	private bool swordCollidedOk=true;
	private bool interval=false;
	private int ran;
	
	//音声関連
	AudioSource aud;
	public AudioClip[] se;

	// Use this for initialization
	void Start () {
		//controller=GetCompofnent<CharacterController>();
		target=GameObject.Find("Target of the enemy");//FPSControllerだった
	//生成されてからスクリプトファイル見つける
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
		enemyGenerator=GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
		
		//rigid=GetComponent<Rigidbody>();
		
		//Hpを最初はマックスにする
		hpSlider.value=1;
		
		//アニメーション関連のコンポーネント取得
		animator=GetComponent<Animator>();
		anim = gameObject.GetComponent<Animation> ();
		
		//音声のコンポーネント取得
		aud=GetComponent<AudioSource>();
		aud.PlayOneShot(se[0]);//走る足音の音声再生
	}
	
		// Update is called once per frame
	void Update () {
		Debug.Log (swordCollided);
        //ターゲットとある程度近くなったら止まって攻撃
        dif=transform.position.z-target.transform.position.z;
		
		//ターゲットの方に向く処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		//ターゲットから遠かったら移動する
		if(transform.position.z>1.5){
			transform.position += transform.forward *Time.deltaTime* speed;//前へ移動
		}else{//ターゲットに近くなったら待機アニメーションスタート
				aud.Stop();//足音の秒数が長いので、目的地に着いたら音声終了
				del+=Time.deltaTime;//このタイミングで、下の攻撃のための時間計算
				if(walked==false && dead==false){
					walked=true;
					//anim.Play("samurai_bow_combat_mode");
					animator.SetTrigger("idle");
				}
		}
		//Debug.Log(del);
		//剣で攻撃処理
		if(del>5){//5秒になったら攻撃
		if(attacked==false && dead==false){
			swordCollidedOk=false;
				//anim.Play("samurai_specal_attack_A");
				animator.SetTrigger("Attack");
			attacked=true;
		}
		}
		//5秒で攻撃開始して、それにアニメーションの秒数を考慮した時間になったらリセット
			if(del>6.5){
				swordCollidedOk=true;
				del=0;
			}
		//5秒以内なら待機アニメーションに戻る
		if(del<5){
			if(attacked==true && dead==false){
				attacked=false;
				//anim.Play("samurai_bow_combat_mode");
				animator.SetTrigger("idle");
			}	
		}
		        
        //hp表示処理
        hpSlider.value=hp/6;
	}//update
	
	//通常ダメージ処理。主人公の剣が当たったかどうか判定とその後の処理
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag=="sword" && dead==false){//相手の(主人公)の剣が当たった場合&死んでいない場足&剣と剣がぶつかった場合それは体ではないので処理しない
			if (swordCollided == false) {//死んでもなくて剣と剣がぶつかってなかったら
				//anim.Play ("samurai_backwards");//ダメージを受けたみたいなアニメーション再生
				if(hp>1){
					aud.PlayOneShot(se[3]);//剣で切られた時の音声再生
					animator.SetTrigger("Back");
					Invoke("DelayIdle",0.5f);
					Invoke("SetInterval",3.0f);
				}else{
					dead = true;
					//anim.Play ("samurai_Dying");//死亡時のアニメーション再生
					animator.SetTrigger("Dead");
					Invoke("DelayDeadAudio",1.8f);
					Invoke ("DelayDestroyer", 3.0f);					
				}
				hitCount++;
				hp--;
			
				//剣が当たった位置にエフェクトを発生させる
				foreach (ContactPoint point in other.contacts) {
					effectPos = (Vector3)point.point;
					effect = (GameObject)Instantiate (effectPre, effectPos, Quaternion.identity);
				}
			}

		}//swordが当たった時の処理
}

//サイクロンに当たっているときは走るアニメーションはやめる
void OnTriggerEnter(Collider other){
	if(other.gameObject.tag=="cyclone"){
		Debug.Log("竜巻に当たっちまった");
		dead=true;
		//anim.Play("samurai_Dying");//死亡時のアニメーション再生
		animator.SetTrigger("Dead");
		Invoke("DelayDestroyer",5.0f);
	}
}

void DelayDestroyer(){
		Destroy(gameObject);//衝突した敵オブジェクトを破壊
		gameController.scoreCounter(100);
		enemyGenerator.enemyNumber--;//敵の数の値を減らす
}

void DelayDeadAudio(){
		aud.PlayOneShot(se[4]);//倒れた時の音声再生
}

	//swordControllerから呼ばれる、剣と剣がぶつかったときに、ダメージと同じアニメーションを再生する
	public void SwordCollided(){
		if(swordCollidedOk==false && interval==false){
			interval=true;
		//anim.Play("samurai_backwards");//ダメージを受けたみたいなアニメーション再生
		animator.SetTrigger("Back");
		Invoke("DelayIdle",0.5f);//弾くアニメーションを再生したらすぐidleに戻す
		Invoke("SetInterval",3.0f);
		}
	}
	
	//仰け反る(ダメージ時)アニメーションを再生した後すぐidleアニメーションを再生したい
	void DelayIdle(){
		animator.SetTrigger("idle");
	}

	void SetInterval(){
		interval=false;
	}	
}
