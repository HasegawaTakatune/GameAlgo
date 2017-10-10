using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　SyncPosition</para>
/// <para>機能　　　:　プレイヤー座標を同期する</para>
/// </summary>
public class SyncPosition : Photon.MonoBehaviour {
	
	/// 最後に同期した座標
	Vector3 lastPos;

	/// <summary>
	/// <para>関数名:　Awake</para>
	/// <para>機能　:　初期化処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Awake () {
		// 生成時の座標に初期化
		lastPos = transform.position;
	}

	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　座標同期・同期先の座標更新をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Update () {
		// クライアントが制御している時
		if (photonView.isMine) {
			// 座標の取得
			Vector3 nowPos = transform.position;
			// 座標に差ができた時
			if (Vector3.Distance (nowPos, lastPos) > 0.5f) {
				// 座標を同期する
				photonView.RPC ("syncPosition", PhotonTargets.Others, nowPos);
				// 最後に同期した座標を更新
				lastPos = nowPos;
			}
		} else {
			// クライアント制御でない時
			// 同期した座標に移動する
			transform.position = Vector3.Lerp (transform.position, lastPos, 0.1f);
		}
	}

	/// <summary>
	/// <para>関数名:　syncPosition</para>
	/// <para>機能　:　座標を同期する</para>
	/// <para>引数　:　Vector3 pos 同期座標</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	[PunRPC]
	void syncPosition(Vector3 pos){
		// 同期座標の更新
		lastPos = pos;
	}
}
