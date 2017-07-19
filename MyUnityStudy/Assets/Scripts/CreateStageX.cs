using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageX : MonoBehaviour {
	// 生成のタイプ
	const byte WALL = 0,GROUND = 1;
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

	// 生成場所
	Vector2 pos = new Vector2(0,2);

	// Use this for initialization
	void Start () {
		// 生成処理
		for (int i = 0; pos.y <= 100; i++) {
			// 生成
			if (type == WALL) {
				Instantiate (Wall [direc], pos, Quaternion.identity);
				// 次に生成する位置に移動
				pos += Wall [direc].GetComponent<Stage> ().vector2;
			} else {
				Instantiate (Ground [direc], pos, Quaternion.identity);
				// 次に生成する位置に移動
				pos += Ground [direc].GetComponent<Stage> ().vector2;
			}

			// 次に生成するモノを決める
			rand = (byte)Random.Range (0, 2);
			// 生成するステージの更新
			switch (rand) {
			case WALL:
				if (type == GROUND)
					pos.y += 6;
				type = WALL;
				direc = (direc == NORMAL) ? TURN : NORMAL;
				break;
			case GROUND:
				type = GROUND;
				direc = (byte)Random.Range (0, 2);
				if (direc == TURN)
					pos.x += -8;
				break;
			default:
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

}
