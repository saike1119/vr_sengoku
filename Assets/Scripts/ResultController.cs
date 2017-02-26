using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
