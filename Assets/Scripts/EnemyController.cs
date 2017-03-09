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
	private float hp=2;//下のほうで、二回攻撃を受けたら消えるという処理をしてるので2と設定
	
	//プレイヤーの剣に攻撃された際のエフェクト処理
	public GameObject effectPre;
	GameObject effect;
	Vector3 effectPos;
	
	GameController gameController;//自分がやられたらスコアを他スクリプトに渡す
	EnemyGenerator enemyGenerator;//自分がやられたら敵の残り数の表示を更新するため

	// Use this for initialization
	void Start () {
		//controller=GetCompofnent<CharacterController>();
		target=GameObject.Find("Target of the enemy");//FPSControllerだった
	//生成されてからスクリプトファイル見つける
		gameController=GameObject.Find("GameController").GetComponent<GameController>();
		enemyGenerator=GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
		
		//Hpを最初はマックスにする
		hpSlider.value=1;
	}
	
		// Update is called once per frame
	void Update () {
        //ターゲットとある程度近くなったら止まって攻撃
        dif=transform.position.z-target.transform.position.z;
		
		//ターゲットの方に向く処理
		transform.rotation=Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
		//ターゲットとの距離が近かった時の処理
		 if(dif<1.5){
        //Debug.Log("近いよ君！");
        //定期的に剣で攻撃する処理
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

        }else{//ターゲットがまだ遠かったらターゲットに近づく
        transform.position += transform.forward *Time.deltaTime* speed;//ターゲットの方へ移動させる処理
        }                
        
        //hp表示処理
        hpSlider.value=hp/2;
       
	}//update

	//主人公の剣が当たったかどうか判定とその後の処理
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag=="sword"){
			hitCount++;
			hp--;
			if(hitCount==2){//2回攻撃されたら死亡
				Destroy(gameObject);
				gameController.scoreCounter(100);
				enemyGenerator.enemyNumber--;//敵の数の値を減らす
			}
			
			//剣が当たった位置にエフェクトを発生させる
			foreach (ContactPoint point in other.contacts) {
				effectPos=(Vector3)point.point;
				effect=(GameObject)Instantiate(effectPre,effectPos,Quaternion.identity);
			}
		}//swordが当たった時の処理
}

void OnTriggerEnter(Collider other){
	if(other.gameObject.tag=="cyclone"){
		//Invoke("DelayDestroyer",6.0f);
	}
}

void DelayDestroyer(){
		Destroy(gameObject);//衝突した敵オブジェクトを破壊
		enemyGenerator.enemyNumber--;//敵の数の値を減らす
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
