using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageX : MonoBehaviour {
	// 方向
	const byte Right = 0,Left = 1;
	byte direc;
	// 生成のタイプ
	const byte WALL = 0,GROUND = 1,GROUND_TURN = 2;
	byte type = WALL;
	byte rand = WALL;
	// 生成するオブジェクト
	[SerializeField]
	GameObject[] C_Obj;
	List<Stage> S_Obj = new List<Stage> ();
	// 生成場所
	Vector2 pos = new Vector2(0,2);

	// Use this for initialization
	void Start () {
		// 
		for (int i = 0; i < C_Obj.Length; i++)
			S_Obj.Add (C_Obj [i].GetComponent<Stage> ());
		// 生成処理
		for (int i = 0;pos.y <= 100; i++) {
			// 生成
			Instantiate (C_Obj [type], pos, Quaternion.identity);
			// 次に生成する位置に移動
			pos += S_Obj [type].vector2;
			// 次に生成するモノを決める
			if (type == WALL)
				rand = (byte)Random.Range (0, 4);
			else {
				rand = (byte)Random.Range (0, 1);
				rand = (rand == 0) ? WALL : type;
			}
			// 生成するステージの更新
			if (type != rand) {
				switch (rand) {
				case WALL:
					type = WALL;
					pos.y += 6;
					break;
				case GROUND:
					type = GROUND;
					pos.x += 1;
					break;
				case GROUND_TURN:
					type = GROUND_TURN;
					pos.x -= 8;
					break;
				default:
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

}
