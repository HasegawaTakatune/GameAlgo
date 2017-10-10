using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>クラス名　:　RockClimbManager</para>
/// <para>機能　　　:　ゲームマネージャー</para>
/// <para>ゲームステータスの制御・切り替えをする</para>
/// </summary>
public class RockClimbManager : MonoBehaviour,Game_RecieveInterface {
	// ゲームタイマー表示テキスト
	[SerializeField]Text TimeCount;
	// ゲームスタート前のカウントダウン表示テキスト
	[SerializeField]Text CountDown;
	// ゴール時のタイム表示テキスト
	[SerializeField]Text TimeScore;
	// オーディオソース
	[SerializeField]AudioSource audioSource;
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
		// BGMの再生（レース以外のBGM）
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
			if (Input.GetKeyDown (KeyCode.Return)) {
				status = STATUS.CountDown;
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
			// テキストでエンターキー入力を誘導
			CountDown.text = "\"ENTER\"";
			// エンターキーが押された時、メニューに戻る
			if (Input.GetKeyDown (KeyCode.Return))
				SceneManager.LoadScene ("Menu");
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
		// 経過時間を初期化
		timer = 0;
		// ゲームステータスをスタートに変更
		status = STATUS.Play;
		// スタートテキスト表示
		CountDown.text = "Start";
		// ステージ生成メッセージを呼ぶ
		SendMessage ("CreateStage");
		// 遅延する
		yield return new WaitForSeconds (.5f);
		// スタートテキストを初期化
		CountDown.text = "";
	}

	/// <summary>
	/// <para>関数名:　PlayerGoal</para>
	/// <para>機能　:　プレイヤーがゴールした際の処理/para>
	/// <para>引数　:　Player player　プレイヤー情報</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	public void PlayerGoal(Player player){
		// ゴールしていないプレイヤーがゴールした時
		if (!player.Goal) {
			// 今のBGMを止める
			audioSource.Stop ();
			// レース後のBGMを再生
			audioSource.PlayOneShot (BreakTimeBGM);
			// ゴールした人数を加算
			GoalCount++;
			// 順位と経過時間を表示
			TimeScore.text += GoalCount + "位 : " + player.Name + "Time : " + (Mathf.Floor (timer * 100) / 100) + "秒\n";
			// プレイヤーをゴール後状態にする
			player.Goal = true;
			// ゲームステータスをレース終了後状態にする
			status = STATUS.End;
		}
	}
}
/// End of class