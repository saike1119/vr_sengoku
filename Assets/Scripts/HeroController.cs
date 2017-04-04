using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {
	//キャラクターコントローラー関係
	//CharacterController controller;
	Vector3 moveDirection=Vector3.zero;
	public float speedZ;
	public float speedJump;
	
	//ダメージの値をGameControllerに渡すので必要
	public GameController gameController;
	
	//剣関係
	public GameObject sword;
	public GameObject sword2;
	public GameObject swordPosBlock;//剣の回転の中心点のためで、本番ではいらない。
	private float del;
	
	private bool attacked=false;//剣の振ったのか振っていないのか確かめるため
	public bool dead=false;
	
	//カメラの向きと体全体の向きを合わせるためのターゲットオブジェクト
	public GameObject target;
	
	//必殺技関連
	public GameObject specialAttackPre;//使うプレファブ
	GameObject specialAttack;//生成するオブジェクト
	Rigidbody rigid;//飛ばすので
	Vector3 specialAttackPos;//必殺技を生成する位置(少し後ろから生成したいので)
	public int specialAttackCount=0;//必殺技残り回数
	public Text specialAttackLabel;//必殺技残り回数ラベル
	
	//音声関連
	AudioSource aud;
	public AudioClip[] se;
	

	// Use this for initialization
	void Start () {
		//controller=GetComponent<CharacterController>();
		specialAttackLabel.text=""+specialAttackCount;
		
		//音声のコンポーネント取得
		aud=GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		//体全体をカメラの向きと同じ方向に向かせる(横方向のみ)
		transform.rotation= Quaternion.Euler(0,target.transform.localEulerAngles.y,0);
		
			//とりあえずクリックしたら攻撃
		if(Input.GetMouseButton(0)){
				attacked=true;//攻撃をしたことを表すフラグ
				
				//500の倍数になるごとに必殺技の回数を増やす
				if(gameController.score%500==0 && !(gameController.score==0)){
					specialAttackCount++;
				}
				
		//必殺技残り回数テキストを更新
		specialAttackLabel.text=""+specialAttackCount;
				
				//必殺技が打てる状態なら技オブジェクト生成
				if(specialAttackCount>0 && !(PlayerPrefs.GetInt("BossExist")==1)){				specialAttackCount--;
					specialAttackPos=transform.position;
					specialAttackPos.z-=2;
					specialAttack=(GameObject)Instantiate(specialAttackPre,specialAttackPos,Quaternion.identity);
            		Rigidbody attackRigidbody = specialAttack.GetComponent<Rigidbody>();//プレファブのrigidbodyコンポーネントを取得
            		attackRigidbody.AddForce(transform.forward*300);
				}//必殺技が打てる状態なら
			}//クリックしたら
			
			if(attacked==true){
				//attacked=false;
				del+=Time.deltaTime;
				if(del<0.3f && sword.transform.rotation.x<50){//約0.3秒間ゆっくり剣を振る		
				//引数は、回転の中心点、方向、角度を指定
					sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, 3);				
					sword2.transform.RotateAround(swordPosBlock.transform.position, transform.right, 3);
				}
				if(del>1 && del<1.3f){//剣を振り下ろした後に剣を戻す。
					sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, -3);
					sword2.transform.RotateAround(swordPosBlock.transform.position, transform.right, -3);
				}
				if(del>=1.3f){
				//フレーム時間を使ってるので、一回の攻撃を終えるたびに位置と角度を修正
					sword.transform.position=new Vector3(1,2.5f,1.5f);
					sword2.transform.position=new Vector3(-1,2.5f,1.5f);
					sword.transform.rotation=Quaternion.Euler(0,0,20);
					sword2.transform.rotation=Quaternion.Euler(0,0,-20);
				//剣を戻した後に剣の時間関係をリセットし、攻撃したこともリセット	
					del=0;
					attacked=false;
				}
				
		
			}
			
	}//Update終わり
	

	//敵の武器が自分に衝突したら呼ばれる
	void OnTriggerExit(Collider other){

		if(other.gameObject.tag=="enemyWeapon"){
			//Debug.Log("自分に敵の武器が当たったよ");
			aud.PlayOneShot(se[0]);
			gameController.hpController(1);
		}
	}
}
