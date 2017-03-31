using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//雑魚キャラ(敵)のソードコントローラー
public class SwordController : MonoBehaviour {
	//剣と剣が衝突したよきに自分(敵)を押す処理のための
	public EnemyController enemyController;

	//剣と剣がぶつかったとき関連
	public GameObject effectPre;
	GameObject effect;
	Vector3 effectPos;

	Rigidbody rigid;
	GameObject enemy;

	//音声関連
	AudioSource aud;
	public AudioClip[] se;
		
	// Use this for initialization
	void Start () {
		//音声のコンポーネント取得
		aud=GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(enemy.transform.position);
	}

	//剣と剣がぶつかったら、enemyControllerのアニメーションを再生する関数を呼びだす
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "sword") {//主人公の剣と自分(敵)の剣が衝突したときの処理
			Debug.Log("剣と剣がぶつかった");
			aud.PlayOneShot(se[0]);
			enemyController.SwordCollided();
			enemyController.swordCollided = true;
			effect=(GameObject)Instantiate(effectPre,transform.position,Quaternion.identity);
		}
	}

	void OnTriggerExit(Collider other){
		if(enemyController.dead==false){
		enemyController.swordCollided = false;
		}
	}
}
