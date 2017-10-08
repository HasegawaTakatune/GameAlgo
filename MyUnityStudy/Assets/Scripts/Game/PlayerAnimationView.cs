using UnityEngine;
using System.Collections;

/// <summary>
/// <para>クラス名　:　PlayerAnimationView</para>
/// <para>機能　　　:　プレイヤーのアニメーション制御をする</para>
/// </summary>
public class PlayerAnimationView : MonoBehaviour {
	// プレイヤーの制御クラス
	[SerializeField]Player player;
	// アニメーター
	[SerializeField]Animator animator;
	// スプライトレンダラ―
	[SerializeField]SpriteRenderer spriteRenderer;

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　初期化処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Start () {
		// プレイヤー制御を取得
		player = GetComponent<Player>();
		// アニメーターを取得
		animator = GetComponent<Animator>();
		// スプライトレンダラ―を取得
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　プレイヤーの向きとアニメーションの制御をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Update () {

		// プレイヤーの向きを移動方向と同じ向きにする
		if (player.GetDirection ().x > 0)
			spriteRenderer.flipX = false;// 右向き
		 else if (player.GetDirection ().x < 0)
			spriteRenderer.flipX = true;// 左向き

		// 着地していない（ジャンプ中）→走っている→止まっている の順にアニメーションを再生する
		if (!player.IsGrounded ())
			animator.Play ("PlayerJump");
		else if (player.IsRunning ())
			animator.Play ("PlayerRun");
		else
			animator.Play ("PlayerIdle");
	}
}
/// End of class