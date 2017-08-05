using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageX : MonoBehaviour {
	// 生成のタイプ
	const byte WALL = 0,GROUND = 1,GOAL = 2;
	byte type = WALL;
	// 方向
	const byte NORMAL = 0,TURN = 1;
	byte direc = NORMAL;
	byte rand = WALL;
	// 生成オブジェクト(壁)
	[SerializeField]
	GameObject[] Wall;
	// 生成オブジェクト(床)
	[SerializeField]
	GameObject[] Ground;
	// 生成オブジェクト(ゴール)
	[SerializeField]
	GameObject Goal;
	// 生成場所
	Vector2 pos = new Vector2(0,2);

	// Use this for initialization
	void Start () {
		
	}

	void GameStart(){
		CreateStage ();
	}

	void CreateStage(){
		// 生成処理
		while(true){
			// 生成
			switch(type){
			case WALL:
				Instantiate (Wall [direc], pos, Quaternion.identity);
				// 次に生成する位置に移動
				pos += Wall [direc].GetComponent<Stage> ().vector2;
				break;
			case GROUND:
				Instantiate (Ground [direc], pos, Quaternion.identity);
				// 次に生成する位置に移動
				pos += Ground [direc].GetComponent<Stage> ().vector2;
				break;
			}

			// ゴール生成
			if (pos.y >= 100) {
				pos.x += -18;
				Instantiate (Goal, pos, Quaternion.identity);
				break;
			}

			// 次に生成するモノを決める
			rand = (byte)Random.Range (0, 2);
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
