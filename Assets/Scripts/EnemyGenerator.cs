using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	public GameObject[] enemyPre;//雑魚キャラオブジェクト配列
	public GameObject enemyBossPre;//ボスキャラオブジェクト
	public GameObject dark;//使用するダークパーティクル
	public int enemyNumber;//敵の数を指定
	private int enemyNumberSave;//指定した敵の数を記憶するための変数
	GameObject enemy;//生成する敵キャラ
	GameObject effect;//敵を生成すると同時に暗闇エフェクトも生成
	private float del;//時間関係
	public bool bossNow=false;
	Vector3 enemyPos;
	private float PosX;
	private float PosZ;
	private int enemyType;
	private int generateCount;

	// Use this for initialization
	void Start () {
		enemyNumberSave=enemyNumber;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(enemyNumber);
		del+=Time.deltaTime;
		//3秒ごとに生成、生成すると決めた数と生成した数が一致しない間
			if(del>2f && !(enemyNumberSave==generateCount)){
			del=0;
			//敵の生成位置をランダムに生成
			PosX=Random.Range(-16,25);//-170から-120まで
			PosZ=Random.Range(15,30);//-70から-80
		
			//敵の種類をランダムに
			//enemyType=Random.Range(0,enemyPre.Length);
			enemyType=0;
			enemyPos=new Vector3(PosX,-1,PosZ);
			effect=(GameObject)Instantiate(dark,enemyPos,Quaternion.identity);
			generateCount++;
			Invoke("GenerateLate",2.0f);
			}//〜秒ごと
			
			//指定した数の雑魚キャラ全て倒したら、ボスを生成
			if(enemyNumber<=0 && bossNow==false){
				Debug.Log("ボスの出番やで");
				bossNow=true;
				enemyPos=new Vector3(0.5f,-1,15);//最終的には(0.5f,-1,3)がベスト
				effect=(GameObject)Instantiate(dark,enemyPos,Quaternion.identity);
				Invoke("GenerateLate",2.0f);
			}
	}//update
	
	//暗闇エフェクトの後に敵オブジェクを生成する関数
	void GenerateLate(){
		if(bossNow==false){//生成するのが雑魚キャラなのかボスなのか判別
		enemy=(GameObject)Instantiate(enemyPre[enemyType],enemyPos,Quaternion.identity);
		}else{
		enemy=(GameObject)Instantiate(enemyBossPre,enemyPos,Quaternion.identity);		
		}
	}
}
