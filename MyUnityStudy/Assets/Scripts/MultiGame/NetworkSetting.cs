using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　NetworkSetting</para>
/// <para>機能　　　:　プレイヤー生成時の初期設定をする</para>
/// <para>プレイヤーが操作するキャラの制御系統をアクティブ化する</para>
/// </summary>
public class NetworkSetting : Photon.MonoBehaviour {

	// プレイヤー操作
	[SerializeField]Player player;
	// アニメーション制御
	[SerializeField]PlayerAnimationView playerAnimationView;
	// 当たり判定（ボックスコリジョン）
	[SerializeField]BoxCollider2D boxcollider2d;

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　キャラクターの操作系統を設定</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Start () {
		// クライアントが操作するキャラの時
		if (photonView.isMine) {
			// カメラフォーカスを合わせる
			GameObject.Find ("Camera").GetComponent<CameraScroll> ().Target = transform;
			// プレイヤー操作を有効にする
			player.enabled = true;
			// アニメーション制御を有効にする
			playerAnimationView.enabled = true;
			// 当たり判定を有効にする
			boxcollider2d.enabled = true;
			// キャラクター名を同期する
			photonView.RPC ("syncPlayerName", PhotonTargets.OthersBuffered, player.Name);
		}
	}

	/// <summary>
	/// <para>関数名:　syncPlayerName</para>
	/// <para>機能　:　プレイヤー名を同期する</para>
	/// <para>引数　:　string name プレイヤー名</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	[PunRPC]
	void syncPlayerName(string name){
		// プレイヤー名の更新
		player.Name = name;
	}
}
/// End of class