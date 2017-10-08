using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　CreateStageX</para>
/// <para>機能　　　:　ステージのランダム生成をする</para>
/// </summary>
public class CreateStageX : MonoBehaviour {

	// 生成するタイプの定数宣言
	const byte WALL = 0,GROUND = 1,GOAL = 2;
	// 生成するタイプの変数
	byte type = WALL;
	// 次に生成するタイプの変数
	byte next = WALL;
	// 生成する方向の定数宣言
	const byte NORMAL = 0,TURN = 1;
	// 生成する方向の変数
	byte direc = NORMAL;
	// 生成オブジェクト(壁)
	[SerializeField]	GameObject[] Wall;
	// 生成オブジェクト(床)
	[SerializeField]	GameObject[] Ground;
	// 生成オブジェクト(ゴール)
	[SerializeField]	GameObject Goal;
	// 生成する座標の変数
	Vector2 pos = new Vector2(0,2);

	/// <summary>
	/// <para>関数名:　CreateStage</para>
	/// <para>機能　:　生成するタイプ・座標を設定して、</para>
	/// <para>          その座標に生成をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	public void CreateStage(){
		while(true){
			// 生成タイプと座標から生成をする
			Instantiate ((type == WALL) ? Wall [direc] : Ground [direc], pos, Quaternion.identity);
			// 次に生成する場所を設定する
			pos += ((type == WALL) ? Wall [direc].GetComponent<Stage> ().vector2 : Ground [direc].GetComponent<Stage> ().vector2);

			// y座標が100まで到達したら
			if (pos.y >= 100) {
				// x座標の微調整
				pos.x += -18;
				// ゴールを生成する
				Instantiate (Goal, pos, Quaternion.identity);
				// ループを抜ける
				break;
			}

			// 次に生成するタイプを設定
			next = (byte)Random.Range (0, 2);
			// 生成するステージの更新
			switch (next) {

			// 生成タイプが壁の時
			case WALL:
				// 床の次に壁を生成する時
				if (type == GROUND) {
					// y座標の微調整 
					pos.y += 6;
				}
				// 生成物を反転させる
				direc = (direc == NORMAL) ? TURN : NORMAL;
				// 生成タイプを壁に設定する
				type = WALL;
				break;

			// 生成タイプが床の時
			case GROUND:
				// 
				if (type == WALL) {
					// 生成する方向を決める
					direc = (byte)Random.Range (0, 2);
				}
				// 方向が反転する時
				if (direc == TURN) {
					// x座標の微調整 
					pos.x += -6;
				}
				// 生成タイプを床に設定する
				type = GROUND;
				break;
			}
		}
	}
}
/// End of class