using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　MenuManager</para>
/// <para>機能　　　:　タイトルロゴの表示を制御する</para>
/// 　　　　　　　ゲームループで何度もタイトルに戻った際に
/// 　　　　　　　何度もタイトル表示する事を省く
/// </summary>
public class MenuManager : MonoBehaviour {

	// タイトルロゴのスプライト
	[SerializeField]SpriteRenderer TitleLogo;
	// 背景のスプライト
	[SerializeField]SpriteRenderer BackGround;
	// タイトルロゴの表示制御
	// true:表示する　false:表示しない
	static bool showTitle = true;

	/// <summary>
	/// <para>関数名:　Awake</para>
	/// <para>機能　:　タイトルロゴを表示したら非表示にする</para>
	/// <para>　　　　　タイトルに2回目以降戻った際に、映り込ませない</para>
	/// <para>　　　　　ために、Startよりも早く呼ばれるAwakeで処理をしている</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Awake(){
		// ロゴを表示しなくていい時
		if (!showTitle) {
			// タイトルロゴを後ろのレイヤーにする
			TitleLogo.sortingOrder = -3;
			// 背景を後ろのレイヤーにする
			BackGround.sortingOrder = -2;
		}
	}

	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　キー入力でタイトルロゴの非表示制御をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void Update () {
		// タイトルが表示していて、どれかのキーが押された時
		if (showTitle && Input.anyKeyDown) {
			// タイトルロゴを後ろのレイヤーにする
			TitleLogo.sortingOrder = -3;
			// 背景を後ろのレイヤーにする
			BackGround.sortingOrder = -2;
			// 以降、非表示にする
			showTitle = false;
		}
	}
}
/// End of class