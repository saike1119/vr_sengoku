using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	public GameObject[] enemyPre;
	public GameObject dark;
	public int enemyNumber;//敵の数を指定
	private int enemyNumberSave;
	GameObject enemy;
	GameObject effect;
	private float del;
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
			if(del>5 && !(enemyNumberSave==generateCount)){//5秒ごとに敵を生成
			del=0;
			//敵の生成位置をランダムに生成
			PosX=Random.Range(-170,-121);//-170から-120まで
			PosZ=Random.Range(-70,-81);//-70から-80
		
			//敵の種類をランダムに
			enemyType=Random.Range(0,enemyPre.Length);
		
			enemyPos=new Vector3(PosX,1,PosZ);
			effect=(GameObject)Instantiate(dark,enemyPos,Quaternion.identity);
			generateCount++;
			Invoke("GenerateLate",2.0f);
			}//〜秒ごと
	}//update
	
	void GenerateLate(){
		enemy=(GameObject)Instantiate(enemyPre[enemyType],enemyPos,Quaternion.identity);
	}
}
