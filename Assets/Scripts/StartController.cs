using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//剣がオブジェクトを切ったらシーン遷移
	void OnTriggerEnter(Collider other){
		Debug.Log("シーン遷移しようぜ");
		if(other.gameObject.tag=="sword"){
			SceneManager.LoadScene("GameScene");
		}
	}
}
