using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　SyncAnimation</para>
/// <para>機能　　　:　アニメーションを同期する</para>
/// </summary>
public class SyncAnimation : Photon.MonoBehaviour {

	/// 移動制御のソースを取得
	[SerializeField]Player player;
	/// アニメーション制御のソースを取得
	[SerializeField]Animator animator;
	/// スプライトレンダラを取得
	[SerializeField]SpriteRenderer spriteRenderer;

	/// アニメーションタイプ　JUMP：ジャンプ	RUN：走る　IDLE：待機
	const int JUMP = 0,RUN = 1,IDLE = 2;
	/// 再生するアニメーションの名前を保存
	/// PlayerJump：ジャンプ　PlayerRun：走る　PlayerIdle：待機
	string[] animationName = new string[]{"PlayerJump","PlayerRun","PlayerIdle"};
	/// 最後に同期した向き
	float lastDirec;

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　初期化処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Start () {
		// プレイヤー情報を取得
		player = GetComponent<Player> ();
		// アニメーター取得
		animator = GetComponent<Animator> ();
		// スプライトレンダラ―取得
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　同期処理をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Update () {
		// クライアントが制御している時
		if (photonView.isMine) {
			// アニメ―ションタイプを同期
			photonView.RPC ("syncPlayAnimation", PhotonTargets.Others, (!player.IsGrounded ()) ? JUMP : (player.IsRunning ()) ? RUN : IDLE);
			// 移動量を向いている方向として格納
			float nowDirec = player.GetDirection ().x;
			// 移動している時かつ、向いている方向が変わった時
			if (nowDirec != 0 && lastDirec != nowDirec) {
				// 向きを同期する
				photonView.RPC ("syncDirection", PhotonTargets.Others, nowDirec);
				// 最後に向いた方向を更新
				lastDirec = nowDirec;
			}
		}
	}

	/// <summary>
	/// <para>関数名:　syncPlayerAnimation</para>
	/// <para>機能　:　アニメーションを同期する</para>
	/// <para>引数　:　int type アニメーションタイプ</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	[PunRPC]
	void syncPlayAnimation(int type){
		// タイプごとにアニメーション再生
		animator.Play (animationName [type]);
	}

	/// <summary>
	/// <para>関数名:　syncDirection</para>
	/// <para>機能　:　向きを同期する</para>
	/// <para>引数　:　float direc 向く方向</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	[PunRPC]
	void syncDirection(float direc){
		// 向きの更新
		spriteRenderer.flipX = (direc > 0) ? false : true;
	}
}
/// End of class