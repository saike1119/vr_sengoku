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
	
	//剣で攻撃関連
	public GameObject sword;
	public GameObject swordPosBlock;
	private float del;
	private float del2;
	public bool swordCollides;//剣と剣がぶつかったかどうか判定するため
		
	private int hitCount=0;//倒れるまでの受ける攻撃回数
	
	//Hp関連
	public Slider hpSlider;
//下のほうで、二回攻撃を受けたら消えるという処理をしてるので2と設定	
	private float hp=3;//100と109行目も値を変更する
	
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
	private bool dead=false;
	private int ran;

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
		anim = gameObject.GetComponent<Animation> ();
		animator = GetComponent<Animator>();
		anim.Play("samurai_Run");//最初は走るアニメーション再生
	}
	
		// Update is called once per frame
	void Update () {
        //ターゲットとある程度近くなったら止まって攻撃
        dif=transform.position.z-target.transform.position.z;
		
		//ターゲットの方に向く処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		//ターゲットから遠かったら移動する
		if(transform.position.z>4){
			transform.position += transform.forward *Time.deltaTime* speed;//前へ移動
		}else{//ターゲットに近くなったら待機アニメーションスタート
				del+=Time.deltaTime;//このタイミングで、下の攻撃のための時間計算
				if(walked==false && dead==false){
					walked=true;
					anim.Play("samurai_bow_combat_mode");
				}
		}
		//Debug.Log(del);
		//剣で攻撃処理
		if(del>5){//5秒になったら攻撃
		if(attacked==false && dead==false){
			ran=Random.Range(1,3);
			if(ran==1){
				anim.Play("samurai_specal_attack_A");
			}else{
				anim.Play("samurai_specal_attack_B");				
			}
			attacked=true;
		}
		}
		//5秒で攻撃開始して、それにアニメーションの秒数を考慮した時間になったらリセット
			if(del>6.5){
			del=0;
			}
		//5秒以内なら待機アニメーションに戻る
		if(del<5){
			if(attacked==true && dead==false){
				attacked=false;
				anim.Play("samurai_bow_combat_mode");
			}	
		}
		        
        //hp表示処理
        hpSlider.value=hp/3;
	}//update
	
	//主人公の剣が当たったかどうか判定とその後の処理
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag=="sword" && dead==false){
		anim.Play("samurai_backwards");//ダメージを受けたみたいなアニメーション再生
			hitCount++;
			hp--;
			if(hitCount==3){//3回攻撃されたら死亡
				dead=true;
				anim.Play("samurai_Dying");//死亡時のアニメーション再生
				Invoke("DelayDestroyer",3.0f);
			}
			
			//剣が当たった位置にエフェクトを発生させる
			foreach (ContactPoint point in other.contacts) {
				effectPos=(Vector3)point.point;
				effect=(GameObject)Instantiate(effectPre,effectPos,Quaternion.identity);
			}
		}//swordが当たった時の処理
}

//サイクロンに当たっているときは走るアニメーションはやめる
void OnTriggerEnter(Collider other){
	if(other.gameObject.tag=="cyclone"){
		Debug.Log("竜巻に当たっちまった");
		dead=true;
		anim.Play("samurai_Dying");//死亡時のアニメーション再生
		Invoke("DelayDestroyer",5.0f);
	}
}

void DelayDestroyer(){
		Destroy(gameObject);//衝突した敵オブジェクトを破壊
		gameController.scoreCounter(100);
		enemyGenerator.enemyNumber--;//敵の数の値を減らす
}

//敵の数を減らさないバージョンの関数(剣で倒した時と必殺技で倒した時が重なることが多く、敵の数がマイナスになってしまうことが多いので、必殺技で倒した時は敵の数を減らさない)
void DelayDestroyer2(){
		Destroy(gameObject);//衝突した敵オブジェクトを破壊
		gameController.scoreCounter(100);
		//enemyGenerator.enemyNumber--;//敵の数の値を減らす
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
