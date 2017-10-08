using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>クラス名　:　SelectionMenu</para>
/// <para>機能　　　:　ゲームモードの選択とシーン遷移を制御する</para>
/// </summary>
public class SelectionMenu : MonoBehaviour {

	// ゲームモードの列挙宣言
	public enum GameType{Single,Multi}
	// ゲームモード変数
	public GameType gameType;

	/// <summary>
	/// <para>関数名:　OnTriggerEnter2D</para>
	/// <para>機能　:　オブジェクトがオーバーラップした際に、選択されたモードのシーンに遷移する</para>
	/// 　　　　　      ゲームUIとして、選択したいモードの道を進む選択方法をとっているため、
	/// <para>　　　　　オーバーラップイベントに埋め込んでいる</para>
	/// <para>引数　:　Collider2D/ヒット情報</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void OnTriggerEnter2D(Collider2D other){
		// プレイヤーが当たった時
		if (other.tag == "Player")
			// シーン遷移を行う
			SceneManager.LoadScene ((gameType == GameType.Single) ? "SingleGame" : "MultiGame");
	}
}
/// End of class