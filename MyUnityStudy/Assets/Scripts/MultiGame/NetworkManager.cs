using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.MonoBehaviour {
	// 生成するアイテムのパス
	[SerializeField]string ResourcePath = "";
	// 部屋名
	const string ROOM_NAME = "MultiRoom";
	// 入室中判定
	public static bool EnteringTheRoom = false;

	void Start () {
		ConnectPhoton ();	// 初期接続
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
		int No = PhotonNetwork.countOfPlayersInRooms;

		PhotonNetwork.Instantiate (ResourcePath, Vector3.up, Quaternion.identity, 0);
		EnteringTheRoom = true;
	}

}