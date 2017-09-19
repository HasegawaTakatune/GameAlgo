using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	/// 前フレームの最終アニメーションタイプ
	int lastAnim;
	/// 前フレームの最終向き
	float lastDirec;

	void Awake () {
		player = GetComponent<Player> ();
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	void Update () {
		// クライアントが制御している時
		if (photonView.isMine) {
			
			photonView.RPC ("syncPlayAnimation", PhotonTargets.Others, (!player.IsGrounded ()) ? JUMP : (player.IsRunning ()) ? RUN : IDLE);

			float nowDirec = player.GetDirection ().x;
			if (nowDirec != 0 && lastDirec != nowDirec) {
				photonView.RPC ("syncDirection", PhotonTargets.Others, nowDirec);
				lastDirec = nowDirec;
			}
		}
	}

	[PunRPC]
	void syncPlayAnimation(int type){
		animator.Play (animationName [type]);
	}

	[PunRPC]
	void syncDirection(float direc){
		spriteRenderer.flipX = (direc > 0) ? false : true;
	}
}
