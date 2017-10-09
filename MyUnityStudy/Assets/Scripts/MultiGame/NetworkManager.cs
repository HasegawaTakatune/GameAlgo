using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>クラス名　:　NetworkManager</para>
/// <para>機能　　　:　ネットワーク通信の初期設定を行う</para>
/// <para>マスターサーバーのロビーに入り、ルームを</para>
/// <para>探索して入室、なければ生成をして入室する。</para>
/// <para>プレイヤーの名前を設定し生成の順に行う</para>
/// </summary>
public class NetworkManager : Photon.MonoBehaviour {
	// 生成するアイテムのパス
	[SerializeField]string ResourcePath = "";
	// 接続状況を知らせるテキスト
	[SerializeField]Text NoticeText;
	// 入力した名前の格納場所
	[SerializeField]Text inputField;
	// 名前入力インターフェイスのオブジェクト
	[SerializeField]GameObject inputFieldObj;
	// ルーム名
	const string ROOM_NAME = "MultiRoom";
	// 入室中判定
	public static bool EnteringTheRoom = false;

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　Photonサーバーへの初期接続処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void Start () {
		// 初期接続
		ConnectPhoton ();
		// 接続中のテキスト表示
		NoticeText.text = "Connecting...";
	}

	/// <summary>
	/// <para>関数名:　ConnectPhoton</para>
	/// <para>機能　:　Photonサーバーに接続する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	public void ConnectPhoton(){
		// Photonサーバーに接続する
		PhotonNetwork.ConnectUsingSettings ("v1.0");
	}
		
	/// <summary>
	/// <para>関数名:　OnJoinedLobby</para>
	/// <para>機能　:　マスターサーバーのロビーに入った際のコールバック</para>
	/// <para>ルーム作成をリクエストする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void OnJoinedLobby(){
		// ルームの作成
		CreateRoom ();
	}

	/// <summary>
	/// <para>関数名:　CreateRoom</para>
	/// <para>機能　:　ユーザー・ルーム設定を行い、ルーム作成または入室する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	public void CreateRoom(){
		// ルーム作成中のテキスト表示
		NoticeText.text = "Creating room...";

		// ユーザー名・IDの設定(ダミーでUnityChanにしている)
		string userName = "UnityChan";
		string userId = "UnityChan";

		// 退室したユーザーにインスタンス作成されたGameObjectとPhotonViewを、
		// Room内のユーザーが破棄するかを決める
		PhotonNetwork.autoCleanUpPlayerObjects = true;

		// カスタムプロパティ
		ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
		// ユーザー名の追加
		customProp.Add ("userName", userName);
		// ユーザーIDの追加
		customProp.Add ("userId", userId);
		// ユーザーのプロパティをセットし、他のユーザーにも同期をする
		PhotonNetwork.SetPlayerCustomProperties(customProp);

		// ルームオプションの設定
		RoomOptions roomOptions = new RoomOptions ();
		roomOptions.CustomRoomProperties = customProp;
		// ロビーえ見えるルーム情報としてカスタムプロパティのuserName,userIdを使う
		roomOptions.CustomRoomPropertiesForLobby = new string[]{"userName","userId"};
		// ルームの最大人数
		roomOptions.MaxPlayers = 4;
		// 入室許可をする
		roomOptions.IsOpen = true;
		// ロビーから見えるようにする
		roomOptions.IsVisible = true;
		// ROOM_NAMEのルームがなければ作成し、あれば入室する
		PhotonNetwork.JoinOrCreateRoom(ROOM_NAME,roomOptions,null);
	}

	/// <summary>
	/// <para>関数名:　JoinRoom</para>
	/// <para>機能　:　ルームに参加する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	public void JoinRoom(){
		// ルーム参加
		PhotonNetwork.JoinRoom (ROOM_NAME);
	}

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　ルーム入室が成功した際に呼ばれるコールバック</para>
	/// <para>プレイヤーの名前入力インターフェイスを表示する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void OnJoinedRoom(){
		// 名前入力インターフェイスを表示
		inputFieldObj.SetActive (true);
		// 名前入力欄を選択状態にする
		inputFieldObj.GetComponent<InputField> ().ActivateInputField ();
		// 入室した判定にする
		EnteringTheRoom = true;
		// 名前を尋ねるテキスト表示
		NoticeText.text = "What's your name?";
	}

	/// <summary>
	/// <para>関数名:　OnEndInputFieldEdit</para>
	/// <para>機能　:　名前を入力し終わった際に呼ばれるコールバック</para>
	/// <para>入力した名前を反映したプレイヤーを生成する</para>
	/// <para>引数　:　string editText　入力した名前</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	public void OnEndInputFieldEdit(string editText){
		// プレイヤーの入室番号を受け取る
		int No = PhotonNetwork.countOfPlayersInRooms;
		// プレイヤーの生成
		Player player = PhotonNetwork.Instantiate (ResourcePath, Vector3.up, Quaternion.identity, 0).GetComponent<Player>();
		// プレイヤーの名前を設定
		player.Name = inputField.text;
		// 名前入力インターフェイスを非表示にする
		inputFieldObj.SetActive (false);
		// エンターキーの入力を促す
		NoticeText.text = "\"ENTER\"";
		// プレイヤーを生成したメッセージを送る
		SendMessage ("PlayerCreatedMessage");
	}

	/// <summary>
	/// <para>関数名:　OnFaliedToConnectToPhoton</para>
	/// <para>機能　:　Photonサーバーへの接続が失敗した際のコールバック</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void OnFaliedToConnectToPhoton(){
		// 接続失敗をテキスト表示
		NoticeText.text = "Connection failure\nConnection to the Photon server is not established.";
	}

	/// <summary>
	/// <para>関数名:　ToExit</para>
	/// <para>機能　:　ルーム退出処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void ToExit(){
		// ルームから退出して、マスターサーバーに戻る
		PhotonNetwork.LeaveRoom ();
		// Photonサーバーから回線切断する
		PhotonNetwork.Disconnect ();
	}

	/// <summary>
	/// <para>関数名:　OnLeftLobby</para>
	/// <para>機能　:　ロビーを退出した際のコールバック</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void OnLeftLobby(){
		// ロビー退出をログ表示
		Debug.Log ("Call OnLeftLobby");
	}

	/// <summary>
	/// <para>関数名:　OnDisconnectedFromPhoton</para>
	/// <para>機能　:　Photonサーバーから切断した際のコールバック</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// <summary>
	void OnDisconnectedFromPhoton(){
		// Photonサーバー切断をログ表示
		Debug.Log ("Call OnDisconnectedFromPhoton");
	}

}
/// End of class