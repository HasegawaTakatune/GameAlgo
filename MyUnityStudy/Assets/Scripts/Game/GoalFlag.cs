using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　GoalFlag</para>
/// <para>機能　　　:　プレイヤーがゴールをした際に、</para>
/// <para>　　　　　　　プレイヤー情報をメッセージ発行メソッドに送る</para>
/// </summary>
public class GoalFlag : MonoBehaviour {

	// ゲームマネージャー
	// ゴール処理を持つインターフェイス
	RockClimbManagerInterface manager;

	/// <summary>
	/// <para>関数名　:　Start</para>
	/// <para>機能　　:　初期化処理</para>
	/// <para>引数　　:　なし</para>
	/// <para>戻り値　:　なし</para>
	/// </summary>
	void Start () {
		// ゲームマネージャーを取得
		manager = GameObject.Find ("GameManager").GetComponent<RockClimbManagerInterface> ();
	}

	/// <summary>
	/// <para>関数名:　OnTriggerEnter2D</para>
	/// <para>機能　:　ゴールしたプレイヤー情報を、</para>
	/// <para>　　　　　メッセージ発行メソッドに送る</para>
	/// <para>引数　:　Collider2D\ヒット情報</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void OnTriggerEnter2D(Collider2D other){
		// プレイヤーがゴールしたら
		if (other.tag == "Player") {
			// プレイヤー情報をメッセージ発行メソッドに送る
			manager.PlayerGoal_Interface (other.GetComponent<Player> ());
		}
	}
}
/// End of class