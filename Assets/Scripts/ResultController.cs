using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultController : MonoBehaviour {
	public Text scoreLabel;
	int score;

	// Use this for initialization
	void Start () {
		score=PlayerPrefs.GetInt("Score");
		scoreLabel.text="スコア："+score+"点";	
		}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//剣がオブジェクトを切ったらシーン遷移
	void OnTriggerEnter(Collider other){
		Debug.Log("シーン遷移しようぜ");
		if(other.gameObject.tag=="sword"){
			SceneManager.LoadScene("StartScene");
		}
	}
}
