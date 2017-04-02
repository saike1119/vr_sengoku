using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ボスのソードコントローラー
public class SwordController2 : MonoBehaviour {
	//剣と剣が衝突したよきに自分(敵)を押す処理のための
	public EnemyBossController enemyBossController;
	Rigidbody rigid;
	GameObject enemy;
	
	//剣と剣がぶつかったとき関連
	public GameObject effectPre;
	GameObject effect;
	Vector3 effectPos=new Vector3(0,0.5f,2);

	//音声関連
	AudioSource aud;
	public AudioClip[] se;

		
	// Use this for initialization
	void Start () {
		aud=GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(enemy.transform.position);
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "sword") {//主人公の剣と自分(敵)の剣が衝突したときの処理
			Debug.Log("剣と剣がぶつかった");
			aud.PlayOneShot(se[0]);
			enemyBossController.SwordCollided();
			enemyBossController.swordCollided = true;
			effect=(GameObject)Instantiate(effectPre,effectPos,Quaternion.identity);
		}
	}
		
	void OnTriggerExit(Collider other){
		if(enemyBossController.dead==false){
		enemyBossController.swordCollided = false;
		}
	}
}
