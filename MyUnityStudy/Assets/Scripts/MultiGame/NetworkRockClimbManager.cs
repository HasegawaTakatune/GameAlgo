using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>クラス名　:　NetworkRockClimbManager</para>
/// <para>機能　　　:　ゲームマネージャー</para>
/// <para>ゲームステータスの制御・切り替えをする</para>
/// </summary>
public class NetworkRockClimbManager : Photon.MonoBehaviour,Game_RecieveInterface {
	// ゲームタイマー表示テキスト
	Text TimeCount;
	// ゲームスタート前のカウントダウン表示テキスト
	Text CountDown;
	// ゴール時のタイム表示テキスト
	Text TimeScore;
	// オーディオソース
	AudioSource audioSource;
	// レースが始まった時に流すBGM
	[SerializeField]AudioClip BattleBGM;
	// レース以外のタイミングで流すBGM
	[SerializeField]AudioClip BreakTimeBGM;
	// ゲームステータスの列挙
	public enum STATUS{Wait,CountDown,Play,End}
	// ゲームステータス
	STATUS status = STATUS.Wait;
	// レース中のタイマー
	float timer = 0;
	public float Timer{get{ return timer;}}
	// ゴールした人数
	int GoalCount = 0;
	// プレイヤー人数
	int playerCount = 0;
	// クライアントの操作するプレイヤーの生成判定
	bool playerCreated = false;

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　初期化処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Start () {
		// レース中のタイマーテキストの取得
		TimeCount = GameObject.Find ("TimeCount").GetComponent<Text> ();
		// ゲームスタート前のカウントダウンテキストの取得
		CountDown = GameObject.Find ("CountDown").GetComponent<Text> ();
		// ゴール時のタイムテキストの取得
		TimeScore = GameObject.Find ("TimeScore").GetComponent<Text> ();
		// オーディオソースの取得
		audioSource = GetComponent<AudioSource> ();
		// BGMの再生(レース以外のBGM)
		audioSource.PlayOneShot (BreakTimeBGM);
	}
	
	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　ゲームステータスごとの処理を行う</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Update () {

		// 待機状態以外の時は、タイムカウントをする
		if (status != STATUS.Wait)
			timer += Time.deltaTime;
		
		switch (status) {

		case STATUS.Wait:// 待機状態
			// エンターキーを押した時、スタートカウントダウンを開始する
			if (Input.GetKeyDown (KeyCode.Return) && playerCreated) {
				photonView.RPC ("syncGameStatus", PhotonTargets.All, STATUS.CountDown);
			}
			break;

		case STATUS.CountDown:// スタートカウントダウン
			// カウントダウン時間を格納
			float t = 3 - Mathf.Floor (timer);
			// カウントダウンをテキスト表示
			CountDown.text = t.ToString ();
			// カウントが0になった時、スタートメッセージを呼ぶ
			if (t <= 0)
				StartCoroutine (StartMessage ());
			break;

		case STATUS.Play:// レース中状態
			// タイマーテキストに経過時間を表示
			TimeCount.text = (Mathf.Floor (timer * 100) / 100).ToString ();
			break;

		case STATUS.End:// レース終了後の状態
			// ゲーム終了テキスト表示
			CountDown.text = "\"Game End\"";
			// 時間経過でルーム退出する(通信切断のタイミングを合わせるため)
			StartCoroutine (Delay (3,() => {
				SendMessage ("ToExit");// 退出メッセージを呼ぶ
				SceneManager.LoadScene ("Menu");// メニュー画面に戻る
			}));
			break;
		}
	}

	/// <summary>
	/// <para>関数名:　StartMessage</para>
	/// <para>機能　:　レーススタート時の初期処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	IEnumerator StartMessage(){
		// 今のBGMを止める
		audioSource.Stop ();
		// レース時のBGMを再生
		audioSource.PlayOneShot (BattleBGM);
		// スタート時のプレイヤー人数を求める
		playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;
		// 経過時間を初期化
		timer = 0;
		// ゲームステータスをスタートに変更
		status = STATUS.Play;
		// スタートテキスト表示
		CountDown.text = "Start";
		// ルーム作成者だけがステージ生成メッセージを呼ぶ
		if (photonView.isMine)
			SendMessage ("CreateStage");
		// 遅延する
		yield return new WaitForSeconds (.5f);
		// スタートテキストを初期化
		CountDown.text = "";
	}

	/// <summary>
	/// <para>関数名:　PlayerCreatedMessage</para>
	/// <para>機能　:　プレイヤー生成された時のメッセージ処理</para>
	/// <para>名前入力でエンターキーを押した際に、流れでゲームが始まってしまわないよう遅延を設ける</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void PlayerCreatedMessage(){
		// 遅延させた後にプレイヤー生成された状態にする
		StartCoroutine (Delay (1,() => playerCreated = true));
	}

	/// <summary>
	/// <para>関数名:	Delay</para>
	/// <para>機能　:　	渡された処理を遅延させて実行する</para>
	/// <para>引数　:	float interval 遅延させる時間(秒)</para>
	/// <para>			Action ac 行いたい処理を記述する</para>
	/// <para>戻り値:	なし</para>
	/// </summary>
	IEnumerator Delay(float interval, Action ac){
		// 遅延
		yield return new WaitForSeconds (interval);
		// 渡された処理の実行
		ac ();
	}

	/// <summary>
	/// <para>関数名:　PlayerGoal</para>
	/// <para>機能　:　プレイヤーがゴールした際の処理</para>
	/// <para>ゴールしたクライアントだけBGM処理を適応させ</para>
	/// <para>順位・名前・タイム表示は同期させる</para>
	/// <para>引数　:　Player player プレイヤー情報</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	public void PlayerGoal(Player player){
		// ゴールしたプレイヤーがゴールした時
		if (!player.Goal) {
			// 今のBGMを止める
			audioSource.Stop ();
			// レース後のBGMを再生
			audioSource.PlayOneShot (BreakTimeBGM);
			// ゴールの結果を同期する
			photonView.RPC ("syncPlayerGoal", PhotonTargets.AllBuffered, player.Name,timer);
			// プレイヤーをゴール状態にする
			player.Goal = true;
		}
	}

	/// <summary>
	/// <para>関数名:	syncPlayerGoal</para>
	/// <para>機能　:	プレイヤーのゴール結果を同期する</para>
	/// <para>引数　:	string name プレイヤー名</para>
	/// <para>			float time ゴールしたタイム</para>
	/// <para>戻り値:	なし</para>
	/// </summary>
	[PunRPC]
	void syncPlayerGoal(string name,float time){
		// ゴール人数の加算
		GoalCount++;
		// ゴール結果のテキスト表示
		TimeScore.text += GoalCount + "位 : " + name + " Time : " + (Mathf.Floor (time * 100) / 100) + "秒\n";
		// 全員がゴールした時、ゲームを終了する
		if (GoalCount >= playerCount)
			status = STATUS.End;
	}

	/// <summary>
	/// <para>関数名:　syncGameStatus</para>
	/// <para>機能　:　ゲームステータスの同期</para>
	/// <para>引数　:　STATUS sts ゲームステータス</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	[PunRPC]
	void syncGameStatus(STATUS sts){
		// ゲームステータスの更新
		status = sts;
	}
}
/// End of class