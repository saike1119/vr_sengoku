using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {
	//UI
	public Text scoreLabel;
	/*
	public Text hpLabel;
	public Image scoreBar;
	public Image hpBar;
	public Text timer;
	public Text enemyNumber;
	*/
	public float time=300;//ゲームの制限時間指定
	
	//ダメージ関連　
	public Image damageImage;
	private float damageEffect=0.0f;
	
	//スコアとHP用変数
	public int score;
	public  int hp=4;
	private float scoreAmount;
	private float hpAmount;
	public EnemyGenerator enemyGenerator;//UIを〜％と計算するには、敵の数を知りたい
	private int enemyNumberSave;
	private int hpSave;
	
	//フレーム経過時間関係
	private float del;
	
	//時間制限関係
	private bool timeOver=false;
	
	//必殺技関連
	public HeroController heroController;
	public Text specialAttackLabel;
	
	//最終ボスが倒れたかどうかフラグ
	public bool gameClear=false;
	
	//音声関連
	AudioSource aud;
	public AudioClip[] se;
	private bool onlyOnce=false;
	
	// Use this for initialization
	void Start () {
		//score=500;//必殺技がうまく機能してるか実験
		scoreLabel.text="0";
		enemyNumberSave=enemyGenerator.enemyNumber;//最初に敵の数を把握しておく
		hpSave=hp;
		//EnemyBossControllerでのボスが生成されたことを登録する処理で、ここでリセット
		PlayerPrefs.SetInt("BossExist", 0);//キーに対する値を設定する
		PlayerPrefs.SetInt("Score", 0);//スコアを毎回リセット
		PlayerPrefs.Save();		
		
		aud=GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(hp);
		//Debug.Log(del);
		//ダメージエフェクト処理
		if(!(hp==4)){//少しでもダメージを受けていたら少しずつ回復処理
			del+=Time.deltaTime;
			if(heroController.dead==false){
			damageEffect-=del/20.0f;//色の数値の部分を徐々に減らす(delを割ったりすれば回復の速さ調整可能)
			}
		//Debug.Log(damageEffect);
		//画像の色と透明度の状態を常に更新
		 damageImage.color=new Color(255.0f/255,255.0f/255,255.0f/255,damageEffect/255);
		}else{
		 damageImage.color=new Color(255.0f/255,255.0f/255,255.0f/255,0.0f/255);//最初は非表示
		 del=0;//回復が終わったら減算一時停止			
		}
		//ダメージと透明度の比例。damageEffectは一番下の関数で加算している		
		if(damageEffect<=0) hp=4;
		if(damageEffect>0 && damageEffect<=30) hp=3;
		if(damageEffect>30 && damageEffect<=60) hp=2;
		if(damageEffect>60 && damageEffect<=120) hp=1;
		
		//体力が0になったらLoseシーンへ
		if(hp<=0) {
			//gameClear=true;
			Invoke("DelayMoveScene",3);
		}
		
		//敵を全て倒したらWinシーンへ
		if(gameClear==true){
		PlayerPrefs.SetInt("Score", score);//キーに対する値を設定する
		PlayerPrefs.Save();			
			 SceneManager.LoadScene("WinScene");
		}
		
		//ボス戦になったらボスBGM再生
		if(enemyGenerator.bossNow==true && onlyOnce==false){
			onlyOnce=true;
			 //aud.PlayOneShot(se[1]);
			}
	/*
		if(del>2){
			del=0;
			scoreAmount+=0.05f;
			scoreBar.fillAmount=scoreAmount;
		}
		*/
		
		//スコアの周りの表示のための処理
		//scoreAmount=(float)enemyGenerator.enemyNumber/enemyNumberSave;//%計算
		//scoreAmount=1-scoreAmount;//敵の数の値が減っていくスタイルだったので、1引いて倒したのが何%なのか計算
		//Debug.Log(scoreAmount);
		
		//スコアの周りの部分のパラメータを変更してうまく表示
		//scoreBar.fillAmount=scoreAmount;
				
		//スコアラベルの更新
		scoreLabel.text=""+score;//スコア更新
		
		//敵の数を表示
		//enemyNumber.text=""+enemyGenerator.enemyNumber;
		
		/*
		//時間表示
		if(time>=0){//マイナス時間にならないように
		time-=Time.deltaTime;
		timer.text=""+time.ToString("F0");
		}
		
		//制限時間になったら
		if(time<=0 && timeOver==false){
			timeOver=true;
			Debug.Log("戦い終了 or 時間切れ");
		}
		
		//Hpの周りの部分のパラメータを変更してうまく表示
		hpAmount=(float)hp/hpSave;//%の計算
		　hpBar.fillAmount=hpAmount;
		//Debug.Log(hpAmount);
		
		//Hpの数値を更新(テキスト)
		hpLabel.text=""+hp+"%";
		*/
		
		//隠しコマンド
		if(Input.GetKeyUp(KeyCode.Y)){
			aud.PlayOneShot(se[0]);//野獣音声
		}
	}//update
	
	//プレイヤーが死亡した時、少し時間を設けてからシーン遷移したいので
	void DelayMoveScene(){
			SceneManager.LoadScene("LoseScene");		
	}
	
	
	//enemyControllerクラスから呼ばれる。スコアを加算していく
	public void scoreCounter(int point){
		score+=point;
		//Debug.Log(score);
	}
	//他クラスから呼ばれる。heroクラスから呼ばれる。
	public void hpController(int damage){
		hp-=damage;
		damageEffect+=50;
	}
}
