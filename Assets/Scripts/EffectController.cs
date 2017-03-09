using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {
	private float del;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		del+=Time.deltaTime;
		if(del>1.0f && gameObject.tag=="spark"){
			Destroy(gameObject);
		}
		
		if(del>5.0f && gameObject.tag=="dark"){
			Destroy(gameObject);
		}
	}
}
