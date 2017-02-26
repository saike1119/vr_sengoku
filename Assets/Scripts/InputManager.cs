using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	Vector2 slideStartPosition;//Vector2は2Dベクトルと位置の表現。Vector3は3Dのベクトルと位置の表現
	Vector2 prevPosition;
	Vector2 delta = Vector2.zero;
	bool moved=false;
	
	// 毎フレーム呼ばれる
	void Update () {
		//スライド操作の処理の流れ
		//1.マウスボタンを押したままカーソルを一定以上動かしたらスライドとみなす
		//2.どのくらいカーソルが移動したかを求める
		//3.マウスボタンを話したらスライド終了
		//スライド開始地点
		if (Input.GetButtonDown ("Fire1"))
			slideStartPosition = GetCursorPosition ();

		//画面の1割以上移動させたら開始と判断する
		if (Input.GetButton ("Fire1")) {
			if (Vector2.Distance (slideStartPosition, GetCursorPosition ()) >= (Screen.width * 0.1f))
				moved = true;
		}

		//スライドそうさが終了したか
		if (!Input.GetButtonUp ("Fire1") && !Input.GetButton ("Fire1"))//「ボタンが離れたら処理」と「マウスクリックされたか(マウスボタンを離してから処理)」が重なるのでそれを防ぐために。
			moved = false;//スライドは終わった
		//移動量を求める(//上記にある、画面の一割以上移動させたらここが処理する)
		if (moved)//現在のカーソルの位置と最初のカーソルの位置の差を求める
			delta = GetCursorPosition () - prevPosition;
		else
			delta = Vector2.zero;//移動していない場合、Vector2を0にする
		
		//カーソル位置を更新(最初の位置)
		prevPosition = GetCursorPosition ();//カーソルの位置を取得する処理をする関数を呼び出す

	}//Update関数の終わり

		//クリックされたか
		public bool Clicked(){
			if(!moved && Input.GetButtonUp("Fire1"))//GetButtonUpはマウスが離れされているかかを判別する
				return true;
			else
				return false;
		}

		//スライド時のカーソルの移動量
		public Vector2 GetDeltaPosition(){
		return delta;
	}
		//スライド中かどうか
		public bool Moved(){
		return moved;
	}
		public  Vector2 GetCursorPosition(){
		return Input.mousePosition;//マウスカーソルのいちはmousePositionプロパティで取得できます
		}

}
