using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadContrlller : MonoBehaviour {
	public GameController gameController;
	//死亡時のアニメーション作成
	private float del;
	Vector3 centerPoint=new Vector3(0,-2.75f,0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			//もしプレイヤーの体力が0になったら横に倒れるアニメーション作成
			if(gameController.hp<=0){
				del+=Time.deltaTime;
					if(transform.rotation.z<=0 && transform.rotation.z>=-50){
					//transform.RotateAround(centerPoint, transform.forward, -3);
					transform.Rotate(0,0,-5);
					}
			}					
	}
}
