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
	public GameObject swordPosBlock;//剣の回転の中心点のためで、本番ではいらない。
	private float del;
	
	private bool attacked=false;//剣の振ったのか振っていないのか確かめるため

	// Use this for initialization
	void Start () {
		controller=GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		del+=Time.deltaTime;
		
		
		//if(controller.isGrounded){
			//とりあえずクリックしたら攻撃
			if(Input.GetMouseButtonUp(0)){
				//Debug.Log("左クリックされました。");
				if(attacked==false){	
				attacked=true;
				sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, 45);
				}else{
					attacked=false;
					sword.transform.RotateAround(swordPosBlock.transform.position, transform.right, -45);	
				}
			}
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
		//}//isGrounded終わり
	}//Update終わり
	
	//敵の武器が自分に衝突したら呼ばれる
	void OnTriggerEnter(Collider other){
			Debug.Log("自分に何かが当たったよ");

		if(other.gameObject.tag=="enemyWeapon"){
			Debug.Log("自分に何かが当たったよ");
			gameController.hpController(5);
		}
	}
}
