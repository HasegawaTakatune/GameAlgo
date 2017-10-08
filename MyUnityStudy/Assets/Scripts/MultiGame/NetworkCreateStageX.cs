using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　Stage</para>
/// <para>機能　　　:　生成するオブジェクトの高さ・幅を設定する</para>
/// </summary>
public class NetworkCreateStageX : Photon.MonoBehaviour {
	
	const byte WALL = 0,GROUND = 1,GOAL = 2;	// 生成のタイプ
	byte type = WALL;
	const byte NORMAL = 0,TURN = 1;				// 方向
	byte direc = NORMAL;
	byte rand = WALL;
	[SerializeField]GameObject[] Wall;		// 生成オブジェクト(壁)
	[SerializeField]GameObject[] Ground;	// 生成オブジェクト(床)
	[SerializeField]GameObject Goal;		// 生成オブジェクト(ゴール)
	[SerializeField]string[] WallPath;		// 生成オブジェクト(壁)
	[SerializeField]string[] GroundPath;	// 生成オブジェクト(床)
	[SerializeField]string GoalPath;		// 生成オブジェクト(ゴール)
	Vector2 pos = new Vector2(0,2);				// 生成場所

	void CreateStage(){
		// 生成処理
		while (true) {
			switch (type) {
			case WALL:
				PhotonNetwork.Instantiate (WallPath [direc], pos, Quaternion.identity, 0);	// 床の生成
				pos += Wall [direc].GetComponent<Stage> ().vector2;		// 次に生成する位置に移動
				break;
			case GROUND:
				PhotonNetwork.Instantiate (GroundPath [direc], pos, Quaternion.identity, 0);	// 床の生成
				pos += Ground [direc].GetComponent<Stage> ().vector2;	// 次に生成する位置に移動
				break;
			}

			if (pos.y >= 100) {	// 最上階に到達したら
				pos.x += -18;
				PhotonNetwork.Instantiate (GoalPath, pos, Quaternion.identity, 0);	// ゴールを生成する
				break;
			}

			rand = (byte)Random.Range (0, 2);	// 次に生成するモノを決める
			// 生成するステージの更新
			switch (rand) {
			case WALL:
				if (type == GROUND)
					pos.y += 6;
				direc = (direc == NORMAL) ? TURN : NORMAL;
				type = WALL;
				break;
			case GROUND:
				if (rand != type)
					direc = (byte)Random.Range (0, 2);
				if (direc == TURN)
					pos.x += -6;
				type = GROUND;
				break;
			default:
				break;
			}

		}
	}
}
