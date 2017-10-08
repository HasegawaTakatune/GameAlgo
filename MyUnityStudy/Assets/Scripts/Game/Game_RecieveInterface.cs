using UnityEngine.EventSystems;

/// <summary>
/// メソッド名　:　Game_RecieveInterface
/// 機能　　　　:　メッセージ処理インターフェイス
/// 　　　　　　　　同じオブジェクト内でメッセージが発行された際に、受け取り口を宣言する
/// 　　　　　　　　継承先でメソッドを宣言して扱う
/// </summary>
public interface Game_RecieveInterface : IEventSystemHandler {
	
	/// <summary>
	/// 関数名:　PlayerGoal
	/// 機能　:　プレイヤーがゴールした後の処理を目的として用意している
	/// 引数　:　Player :　プレイヤーステータス
	/// 戻り値:　なし
	/// </summary>
	void PlayerGoal(Player player);
}
/// End method