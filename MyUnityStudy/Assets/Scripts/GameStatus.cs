using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour {

	/// <summary>
	/// <para>関数名:　OnRuntimeMethodLoad</para>
	/// <para>機能　:　ゲームを起動して、一番最初に呼ばれるメソッド</para>
	/// <para>画面サイズ・フレームレートを設定する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	[RuntimeInitializeOnLoadMethod]
	static void OnRuntimeMethodLoad(){
		// スクリーンサイズ、フルスクリーン、フレームレートの設定
		Screen.SetResolution (1024, 600, false, 90);
	}
}
/// End of class