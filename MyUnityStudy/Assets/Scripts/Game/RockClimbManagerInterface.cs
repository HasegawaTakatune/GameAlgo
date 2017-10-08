using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// <para>クラス名　:　RockClimbManager</para>
/// <para>機能　　　:　ゲームマネージャーに関数呼び出し用に、リクエストを流す中継役</para>
/// <para>通常・通信モードでクラスが変わっても、メソッド呼び出しに差異が出ないようメッセージを使う</para>
/// </summary>
public class RockClimbManagerInterface : MonoBehaviour {

	/// <summary>
	/// <para>関数名:　PlayerGoal_Interface</para>
	/// <para>機能　:　ゴールメッセージの呼び出し処理</para>
	/// <para>引数　:　Player player　プレイヤー情報</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	public void PlayerGoal_Interface(Player player){
		// ゴール時のメッセージ呼び出し処理
		ExecuteEvents.Execute<Game_RecieveInterface> (
			target: gameObject, 
			eventData: null, 
			functor: (reciever, eventData) => reciever.PlayerGoal (player));
	}
}
/// End of class