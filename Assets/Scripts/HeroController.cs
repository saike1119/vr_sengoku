using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
	//キャラクターコントローラー関係
	CharacterController controller;
	Vector3 moveDirection=Vector3.zero;
	public float speedZ;
	public float speedJump;
	
	//ダメージの値をGameControllerに渡すので必要
	public GameController gameController;
	
	//剣関係
	public GameObject sword;
	public GameObject sword2;
	public GameObject swordPosBlock;//剣の回転の中心点のためで、本番ではいらない。
	
	private bool attacked=false;//剣の振ったのか振っていないのか確かめるため
	
	//カメラの向きと体全体の向きを合わせるためのターゲットオブジェクト
	public GameObject target;
	
	//必殺技関連
	public GameObject specialAttackPre;//使うプレファブ
	GameObject specialAttack;//生成するオブジェクト
	Rigidbody rigid;//飛ばすので
	public bool specialAttackOk=false;//必殺技フラグ

	// Use this for initialization
	void Start () {
		controller=GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		//体全体をカメラの向きと同じ方向に向かせる
		transform.rotation= Quaternion.Euler(0,target.transform.localEulerAngles.y,0);
		
			//とりあえずクリックしたら攻撃
			if(Input.GetMouseButtonUp(0)){
				if(attacked==false){	
				attacked=true;
				sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, 45);
				sword2.transform.RotateAround(swordPosBlock.transform.position, transform.right, 45);
				
				//必殺技が打てる状態なら技オブジェクト生成
				if(specialAttackOk==true){
						specialAttackOk=false;     	
					specialAttack=(GameObject)Instantiate(specialAttackPre,transform.position,Quaternion.identity);
            Rigidbody attackRigidbody = specialAttack.GetComponent<Rigidbody>();//プレファブのrigidbodyコンポーネントを取得
            attackRigidbody.AddForce(transform.forward*300);
				}

				}else{//剣を戻す処理
					attacked=false;
					sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, -45);	
					sword2.transform.RotateAround(swordPosBlock.transform.position, transform.right, -45);	

				}
			}
						
			/*
			//GetAxisで、上矢印キーなら1が返され、下矢印キーなら-1が返される。
			if(Input.GetAxis("Vertical")>0.0f){
			//moveDirection.z=Input.GetAxis("Vertical")*speedZ;//1*speedZでまっすぐのみ
			moveDirection=Camera.main.transform.forward*speedZ;
			}else if(Input.GetKey(KeyCode.DownArrow)){
			moveDirection=Camera.main.transform.forward*-1*speedZ;
			}else{//指を縦矢印キーから離れるたびに動きを止める。
			moveDirection.x=0;
			moveDirection.y=0;
			moveDirection.z=0;
			}
			
			//方向転換
			transform.Rotate(0,Input.GetAxis("Horizontal")*3,0);//横方向の矢印キーが押されたらその方向にキャラクターを回転させる。
			
			//スペースキーが押されたらジャンプする処理
			if(Input.GetKeyUp(KeyCode.Space)){
				Debug.Log("スペースキーが押されたでござる");
				moveDirection.y=speedJump;
			}
			
			//渡された値によって実際に動かす
			controller.SimpleMove(moveDirection);
			*/
	}//Update終わり
	
	//敵の武器が自分に衝突したら呼ばれる
	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag=="enemyWeapon"){
			Debug.Log("自分に敵の武器が当たったよ");
			gameController.hpController(1);
		}
	}
	
			//GameControllerクラスから呼ばれる。スコアが500毎に呼ばれる
		public void SpecialAttackCounter(){
				specialAttackOk=true;
		}
}
