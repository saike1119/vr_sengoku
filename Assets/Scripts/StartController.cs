using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour {
	public Canvas start;
	float del;

	// Use this for initialization
	void Start () {
		start.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		del+=Time.deltaTime;
		if(del>1.5f){
			start.enabled=true;
		}
		
		if(Input.GetMouseButtonUp(0)){
			SceneManager.LoadScene("GameScene");
		}
	}
}
