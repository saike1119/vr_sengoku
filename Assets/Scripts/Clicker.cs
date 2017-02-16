using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour {
	//キーボード操作、マウス、その他の入力デバイスからの入力をチェック
	public bool clicked(){
		return Input.anyKeyDown;//anyKeyDownは、何らかのキーかマウスボタンを押した最初のフレームのみtrueを返す(読み取り専用)
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
