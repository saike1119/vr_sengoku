using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ボスのソードコントローラー
public class SwordController2 : MonoBehaviour {
	//剣と剣が衝突したよきに自分(敵)を押す処理のための
	public EnemyBossController enemyBossController;
	public GameObject effectPre;
	Rigidbody rigid;
	GameObject enemy;

		
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(enemy.transform.position);
	}

	//剣と剣がぶつかったら、enemyControllerのアニメーションを再生する関数を呼びだす
	void OnCollisionExit(Collision other){
		if (other.gameObject.tag == "sword") {//主人公の剣と自分(敵)の剣が衝突したときの処理
			enemyBossController.SwordCollided();
		}
	}
}
