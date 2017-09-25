using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour {
	// 生成するアイテムのパス
	[SerializeField]string ResourcePath = "";
	// 接続状況を知らせるテキスト
	[SerializeField]Text NoticeText;
	// 入力した名前の格納場所
	[SerializeField]Text inputField;
	[SerializeField]GameObject inputFieldObj;
	// 部屋名
	const string ROOM_NAME = "MultiRoom";
	// 入室中判定
	public static bool EnteringTheRoom = false;
	//
	Player player;

	/// 初期の呼び出し関数
	void Start () {
		ConnectPhoton ();	// 初期接続
		NoticeText.text = "Connecting...";
	}



	/// Photonに接続する最初のおまじない
	public void ConnectPhoton(){
		PhotonNetwork.ConnectUsingSettings ("v1.0");
	}

	/// マスターサーバーのロビーに入った際に呼ばれる
	void OnJoinedLobby(){
		// 接続後の処理

		CreateRoom ();
	}

	/// ルーム作成
	public void CreateRoom(){
		NoticeText.text = "Creating room...";

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

	/// ルーム参加呼び出し
	public void JoinRoom(){
		// ルームの参加
		PhotonNetwork.JoinRoom (ROOM_NAME);
	}

	/// ルーム入室が成功した際に呼ばれるコールバックメソッド
	void OnJoinedRoom(){
		inputFieldObj.SetActive (true);
		//player = PhotonNetwork.Instantiate (ResourcePath, Vector3.up, Quaternion.identity, 0).GetComponent<Player>();
		EnteringTheRoom = true;
		NoticeText.text = "What's your name?";
	}

	/// 入力した名前を受け取り、プレイヤー生成をする
	public void OnEndInputFieldEdit(string editText){
		int No = PhotonNetwork.countOfPlayersInRooms;
		player = PhotonNetwork.Instantiate (ResourcePath, Vector3.up, Quaternion.identity, 0).GetComponent<Player>();
		player.Name = inputField.text;
		inputFieldObj.SetActive (false);
		NoticeText.text = "\"ENTER\"";
		SendMessage ("PlayerCreatedMessage");
	}

	/// Photonサーバーへの接続が失敗した際のコールバック
	void OnFaliedToConnectToPhoton(){
		NoticeText.text = "Connection failure\nConnection to the Photon server is not established.";
	}

}