using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Text scoreLabel;
	public Text hpLabel;
	public Image scoreBar;
	public Image hpBar;
	public Text timer;
	float time=300;
	
	private int score;
	private int hp;

	// Use this for initialization
	void Start () {
		scoreLabel.text="0";
	}
	
	// Update is called once per frame
	void Update () {
		scoreLabel.text=""+score;//スコア更新
		time-=Time.deltaTime;
		timer.text=""+time.ToString("F0");
	}
	
	public void scoreCounter(int point){
		score+=point;
		Debug.Log(score);
	}
	
	public void hpController(){
		
	}
}
