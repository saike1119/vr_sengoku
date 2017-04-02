using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (DelayScene ());
	}

	IEnumerator DelayScene(){
		yield return new WaitForSeconds (7.0f);
		SceneManager.LoadScene ("StartScene");
	}
}
