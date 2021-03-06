﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　NetworkStageCreator</para>
/// <para>機能　　　:　ステージのランダム生成をする</para>
/// </summary>
public class NetworkStageCreator : Photon.MonoBehaviour {

	// 床・壁ブロック
	[SerializeField]string Block = "Block";
	// 特殊壁ブロック
	[SerializeField]string BlockSP = "BlockSP";
	// ゴール
	[SerializeField]string Goal = "Goal";

	/// 変数をローカルで宣言することで、メモリの圧迫をステージ生成時のみに集中させる
	/// <summary>
	/// <para>関数名:　CreateStage</para>
	/// <para>機能　:　ある程度の高さまで床・壁の生成を行い</para>
	/// <para>最後にゴールを生成してループを抜ける</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	public void CreateStage(){
		// 生成物の種分け
		const byte WALL=0,GROUND=1,GOAL=2;
		// 実際に生成するオブジェクトのタイプ
		byte createType = WALL;
		// 生成ポジション
		Vector2 pos = new Vector2 (0, 2);
		// 生成の回数（数字には意味がなく、ただカウントするだけ）
		int count = Random.Range (0, 6);
		// 生成中の判定
		bool create = true;
		// 一回に生成する数
		int createNum = Random.Range (6, 9);
		// 生成する方向　true:右 false:左
		bool direc = true;

		// 生成可能までループ
		while (create) {
			// 生成の種別
			switch (createType) {
			// 壁
			case WALL:
				// 左右に分かれて生成する、交互に特殊壁の生成高さ(countで特殊壁を判定)をずらして生成をしている
				PhotonNetwork.Instantiate ((count % 6 == 0) ? BlockSP : Block, pos, Quaternion.Euler (new Vector3 (0, 0, -90)), 0);
				PhotonNetwork.Instantiate (((count + 3) % 6 == 0) ? BlockSP : Block, pos + new Vector2 (6, 0), Quaternion.Euler (new Vector3 (0, 0, 90)), 0);
				// カウントアップ
				count++;
				// ブロック1個分上にする
				pos.y += 2;
				break;

			// 床
			case GROUND:
				// 床の生成
				PhotonNetwork.Instantiate (Block, pos, Quaternion.identity, 0);
				// 生成する方向にずらす
				pos.x += (direc) ? 2 : -2;
				break;

			// ゴール
			case GOAL:
				// ゴールの生成
				PhotonNetwork.Instantiate (Goal, pos, Quaternion.identity, 0);
				// 生成を終了する
				create = false;
				break;
			}

			// 生成個数を減らす
			createNum--;
			// 個数分生成したら
			if (createNum <= 0) {
				// 高さが100以上まで達した時、ゴールの生成をする
				if (pos.y >= 100) {
					createType = GOAL;
					// 方向転換するための壁を生成
					for (int ii = 0; ii <= 4; ii++)
						PhotonNetwork.Instantiate (Block, pos + new Vector2 (6, ii * 2), Quaternion.Euler (new Vector3 (0, 0, 90)), 0);
				} else {// それ以外、					
					// 生成タイプの設定
					byte type = (Random.Range (0, 2) == 0) ? WALL : GROUND;
					// 生成タイプが変更した時
					if (createType != type) {
						// 生成タイプの更新
						createType = type;
						// 壁に変更された時
						if (createType == WALL) {
							// 生成ポジションの調整
							pos += new Vector2 ((direc) ? -6 : 0, 5);
						} else {
							// 生成する方向の設定
							direc = (Random.Range (0, 2) == 0) ? true : false;
							// 生成する壁の位置
							float xx = (direc) ? 0 : 6;
							// 生成する壁の向き
							Quaternion qq = (direc) ? Quaternion.Euler (new Vector3 (0, 0, -90)) : Quaternion.Euler (new Vector3 (0, 0, 90));
							// 方向転換するための壁を生成
							for (int ii = 0; ii <= 4; ii++)
								PhotonNetwork.Instantiate (Block, pos + new Vector2 (xx, ii * 2), qq, 0);
							// 生成ポジションの調整
							pos.x += (direc) ? 6 : 0;
						}
					}
					// 生成数の設定
					createNum = Random.Range (6, 15);
				}
			}
		}
	}
		
}
/// End of class