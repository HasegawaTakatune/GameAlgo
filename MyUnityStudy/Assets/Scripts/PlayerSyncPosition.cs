using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSyncPosition : NetworkBehaviour {

	// ホストから全クライアントへ送られる
	[SyncVar]
	private Vector3 syncPos;
	// Playerの現在座標
	Transform myTransform;
	// Lerp:２ベクトル間を補間する
	float lerpRate = 15;

	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
		
	void FixedUpdate(){
		TransmitPosition ();
		LerpPosition ();
	}

	// ポジション補間用メソッド
	void LerpPosition(){
		// 補間対象は相手プレイヤーのみ
		if(!isLocalPlayer){
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	// クライアントからホストへ、Position情報を送る
	[Command]
	void CmdProvidePositionToServer(Vector3 pos){
		// サーバー側が受け取る値
		syncPos = pos;
	}

	// クライアントのみ実行される
	[ClientCallback]
	// 位置情報を送るメソッド
	void TransmitPosition(){
		if (isLocalPlayer) {
			CmdProvidePositionToServer (myTransform.position);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
