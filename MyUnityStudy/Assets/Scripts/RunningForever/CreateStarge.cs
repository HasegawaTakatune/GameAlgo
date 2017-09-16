using UnityEngine;
using System.Collections;

public class CreateStarge : MonoBehaviour {

	// 生成タイプ -----------------------//
	const byte 
	Front = 0,			// 正面
	Up = 1,				// 上
	Down = 2,			// 下
	Pitfalls = 3,		// 落とし穴
	Pitfalls2 = 4;		// 落とし穴

	byte type;
	// -------------------------------- //

	// 前進
	float forward = 2.0f;
	// タイプの再設定時間 
	float time = 0;
	//
	[SerializeField]
	float interval;
	// 生成物
	[SerializeField]
	GameObject[] createObject;
	// 加算値
	Vector2 pos;

	// Use this for initialization
	void Start () {
		pos = Vector2.zero;
		time = interval;
		StartCoroutine (Create ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Create(){
		while (true) {
			// タイプ設定
			switch (type) {
			case Front:
				pos += new Vector2 (forward, 0);
				break;

			case Up:
				pos += new Vector2 (forward, Random.Range (1, 2));
				break;

			case Down:
				pos += new Vector2 (forward, Random.Range (-2, -1));
				break;

			case Pitfalls:
				pos += new Vector2 ((forward * 2), 0);
				break;

			case Pitfalls2:
				pos += new Vector2 ((forward * 3), 0);
				break;

			default:
				Debug.Log ("Error : Value out of range (Type)");
				break;
			}
			time -= interval;
			if (time <= 0) {
				ChangeTheType ();
			}
			// 生成
			Instantiate (createObject[Random.Range (0, createObject.Length)], 
				pos, Quaternion.identity);

			yield return new WaitForSeconds (interval);
		}
	}

	void ChangeTheType(){
		type = (byte)Random.Range (0, 4);
		/*switch (type) {
		case Front:
			time = interval * Random.Range (1, 5);
			break;

		case Up:
			time = interval * Random.Range (1, 3);
			break;

		case Down:
			time = interval * Random.Range (1, 3);
			break;

		case Pitfalls:
			time = interval;
			break;

		default:
			Debug.Log ("Error : Value out of range (Time)");
			break;
		}*/
	}



}
