using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageExperiment : MonoBehaviour {
	 float del;
	 public GameObject blockPre;
	 GameObject block;
	 Vector3 blockPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		del+=Time.deltaTime;
		if(del>3){
			del=0;
			blockPos=new Vector3(-150.5f,5,-95);
			 block=(GameObject)Instantiate(blockPre,blockPos,Quaternion.identity);
			  Invoke("Destroyer",2.0f);
		}
	}
	
	void Destroyer(){
		Destroy(block);
	}
}
