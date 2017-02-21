using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	//UI
	public Text scoreLabel;
	public Text hpLabel;
	public Image scoreBar;
	public Image hpBar;
	public Text timer;
	public Text enemyNumber;
	float time=300;
	
	//スコアとHP用変数
	private int score;
	public  int hp=100;
	private float scoreAmount;
	public EnemyGenerator enemyGenerator;
	private int enemyNumberSave;
	
	//フレーム経過時間関係
	private float del;
	
	// Use this for initialization
	void Start () {
		scoreLabel.text="0";
		enemyNumberSave=enemyGenerator.enemyNumber;//最初に敵の数を把握しておく
	}
	
	// Update is called once per frame
	void Update () {
		/*
		del+=Time.deltaTime;
		if(del>2){
			del=0;
			scoreAmount+=0.05f;
			scoreBar.fillAmount=scoreAmount;
		}
		*/
		
		//スコアの周りの表示のための処理
		scoreAmount=(float)enemyGenerator.enemyNumber/enemyNumberSave;//%計算
		scoreAmount=1-scoreAmount;//敵の数の値が減っていくスタイルだったので、1引いて倒したのが何%なのか計算
		//Debug.Log(scoreAmount);
		
		//スコアの周りの部分のパラメータを変更してうまく表示
		scoreBar.fillAmount=scoreAmount;
		
		//敵の数を表示
		enemyNumber.text=""+enemyGenerator.enemyNumber;
		
		//スコアラベルの更新
		scoreLabel.text=""+score;//スコア更新
		
		//時間表示
		time-=Time.deltaTime;
		timer.text=""+time.ToString("F0");
		
		//Hpラベルの更新
		hpLabel.text=""+hp+"%";
		
	}
	
	//他クラスから呼ばれる。スコアを加算していく
	public void scoreCounter(int point){
		score+=point;
		//Debug.Log(score);
	}
	//他クラスから呼ばれる。heroクラスから呼ばれる。
	public void hpController(int damage){
		hp-=damage;
	}
}
