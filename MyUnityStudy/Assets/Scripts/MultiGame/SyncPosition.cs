using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : Photon.MonoBehaviour {
	/// 座標同期直後の座標を保存
	Vector3 lastPos;

	void Awake () {
		// 生成時の座標に初期化
		lastPos = transform.position;
	}
	
	void Update () {
		if (photonView.isMine) {
			Vector3 nowPos = transform.position;
			if (Vector3.Distance (nowPos, lastPos) > 0.5f) {
				photonView.RPC ("syncPosition", PhotonTargets.Others, nowPos);
				lastPos = nowPos;
			}
		}
	}

	[PunRPC]
	void syncPosition(Vector3 pos){
		transform.position = Vector3.Lerp (transform.position, pos, 0.5f);
	}

}
