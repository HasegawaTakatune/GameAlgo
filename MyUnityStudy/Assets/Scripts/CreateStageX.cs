using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageX : MonoBehaviour {
	// 方向
	const byte Right = 0,Left = 1;
	byte direc;
	// 生成のタイプ
	const byte WALL = 0,GROUND = 1;
	// 生成するオブジェクト
	[SerializeField]
	GameObject[] CreateObj;
	// 生成場所
	Vector2 pos = Vector2.zero;
	[SerializeField]
	int createNum = 10;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < createNum; i++) {
			Instantiate (CreateObj [WALL], pos, Quaternion.identity);
			pos.y += CreateObj [WALL].GetComponent<Stage> ().GetHeight ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		


	}

}
