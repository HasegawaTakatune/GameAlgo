using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>クラス名　:　Player</para>
/// <para>機能　　　:　プレイヤーのキー入力操作を受け持つ</para>
/// </summary>
public class Player : MonoBehaviour {
	// 頭上に名前を表示するテキスト
	[SerializeField]Text nameLabel;
	// キャラクター名
	string _name = "UnityChan";
	public string Name {
		// キャラ名ゲッター
		get{ return _name; }
		// キャラ名セッター、頭上の名前にも反映する
		set {_name = value;	nameLabel.text = _name;	}
	}
	// ヒットステータス
	// None なし  R_Jump 右の壁  L_Jump 左の壁  Ground 床
	const byte None = 0,R_Jump = 1,L_Jump = 2,Ground = 3;
	// 移動ベクトル
	Vector3 velocity;

	// 移動速度
	[SerializeField]const float speed = 0.1f;

	// 当たり判定
	[SerializeField]RaycastHit2D isGrounded;

	// 走り制御
	bool isRunning;

	// ジャンプ中かの判定
	bool jumping = false;
	// ジャンプの種別
	byte jumpType = None;

	// ジャンプ速度
	[SerializeField]float jumpSpeed = 0.65f;
	// 壁キック速度
	[SerializeField]float kickSpeed = 0.055f;

	// 特殊ジャンプエフェクト
	[SerializeField]GameObject sp_JumpEffect;
	// 滞空時間
	float time;

	// 着地したかの判定
	bool landing = false;

	// ゴールしたかの判定
	bool goal = false;
	// ゴールのゲッタ/セッタ
	public bool Goal{ get { return goal; } set { goal = value; } }

	// オーディオソース サウンドの再生先
	[SerializeField]AudioSource audioSource;
	// ジャンプSE
	[SerializeField]AudioClip JumpAudio;
	// 特殊ジャンプSE
	[SerializeField]AudioClip SPJumpAudio;

	/// <summary>
	/// <para>関数名:　Start</para>
	/// <para>機能　:　初期化処理</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Start(){
		// オーディオソースの取得
		audioSource = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　メインループ、移動・ジャンプ関数を常時呼び出す</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Update () {
		// 移動
		Run ();
		// 足下に床があるかの当たり判定
		isGrounded = Physics2D.Raycast (transform.position, Vector2.down,1.6f, 1 << LayerMask.NameToLayer ("Ground"));
		// ジャンプ
		Jump();
		// 座標の更新
		UpdatePosition ();
	}

	/// <summary>
	/// <para>関数名:　Run</para>
	/// <para>機能　:　左右キー入力移動</para>
	/// <para>			キー入力がされた時に、それぞれの移動量を計算しvelocityに格納する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Run(){
		// ベクトルを0に初期化
		velocity = Vector3.zero;
		// 足下の座標を取る
		if (isGrounded) {
			// 着地していな時かつ、床にめり込んだ見た目になっていた時
			if (!landing && transform.position.y - 2.6f <= isGrounded.point.y) {
				// 滞空時間を初期化
				time = 0;
				// 座標の修正
				transform.position = new Vector3 (transform.position.x, isGrounded.point.y + 1.5f, 0);
				// 着地した
				landing = true;
			}
		} else {// 着地していない間
			// 落下する
			Falling ();
			// 着地していない
			landing = false;

			// 一番下まで落下した時
			if (transform.position.y < -3) {
				// 初期地点に戻す
				transform.position = Vector3.zero;
			}
		}

		// 右矢印キー/Dキーが押され、右に壁がない時
		if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && !Raycast2D(Vector2.right)) {
			// 移動量を速度分加算
			velocity.x += speed;
		}
		// 左矢印キー/Aキーが押され、右に壁がない時
		if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && !Raycast2D(Vector2.left)) {
			// 移動量を速度分減算
			velocity.x += -speed;
		}
		// シフトキーが押され、床にいる時
		if (Input.GetKey (KeyCode.LeftShift) && isGrounded) {
			// 移動量を4倍にする
			velocity = velocity * 4;
		}

	}

	/// <summary>
	/// <para>関数名:　Jump</para>
	/// <para>機能　:　スペースキー入力でジャンプする</para>
	/// <para>			床・左右の壁の当たり判定をして、普通のジャンプか壁キックを使い分ける</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Jump(){
		//スペースキーが押された時
		if (Input.GetKeyDown (KeyCode.Space)) {
			// 左右のヒット判定
			RaycastHit2D R_Hit = Raycast2D (Vector2.right);
			RaycastHit2D L_Hit = Raycast2D (Vector2.left);

			// 地面・壁に接していたら
			if (isGrounded || R_Hit || L_Hit) {
				// ジャンプフラグをtrueにする
				jumping = true;	
				// 滞空時間の初期化
				time = 0;

				// ジャンプの種類を識別する
				// 地面に接していない時に、壁キックの判定をして、
				// 地面に接している時だけ普通のジャンプをするように設定する
				jumpType = (isGrounded)? Ground : (R_Hit == true) ? R_Jump : (L_Hit == true) ? L_Jump : None;

				// ジャンプのタイプごとに、どの方向の壁で特殊壁の判定をするかを決める
				if ((jumpType == R_Jump) ? R_Hit.collider.GetComponent<Ground> ().IsSpecial :
					(jumpType == L_Jump) ? L_Hit.collider.GetComponent<Ground> ().IsSpecial : false) {
					// 特殊壁のジャンプステータスを設定
					SetSpecialJumpStatus ();
				} else
					// 通常壁のじぇんぷステータスを設定
					SetNormalJumpStatus ();
			}
		}
		// ジャンプをしている時
		if (jumping) {
			// 壁キックの方向ごとにｘ方向の移動量を加算
			velocity.x += (jumpType==R_Jump)?-kickSpeed:(jumpType==L_Jump)?kickSpeed:0;
			// 上に衝突するモノが無ければ、y方向に移動量分加算をする
			if(!Raycast2D(Vector2.up))velocity.y += jumpSpeed;
			// 滞空時間を加算
			time += Time.deltaTime;
			// ジャンプ減速量の計算
			jumpSpeed -= (9.8f * (time * time)) / 2;
			// ジャンプ速度が0以下になった時
			if (jumpSpeed <= 0) {
				// ジャンプフラグをfalseにする
				jumping = false;
				// 滞空時間を初期化
				time = 0;
			}
		}
	}

	/// <summary>
	/// <para>関数名:　SetNormalJumpStatus</para>
	/// <para>機能　:　通常ジャンプのステータス設定をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void SetNormalJumpStatus(){
		// 上昇速度を設定
		jumpSpeed = .65f;//n_jumpSpeed;
		// 壁キックで横に跳ぶ速度を設定
		kickSpeed = .1f;//n_kickSpeed;
		// ジャンプSEの再生
		audioSource.PlayOneShot (JumpAudio);
	}
	/// <summary>
	/// <para>関数名:　SetSpecialJumpStatus</para>
	/// <para>機能　:　特殊ジャンプのステータス設定をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void SetSpecialJumpStatus(){
		// 上昇速度を設定
		jumpSpeed = .76f;
		// 壁キックで横に跳ぶ速度を設定
		kickSpeed = .25f;
		// ジャンプSEを再生
		audioSource.PlayOneShot (SPJumpAudio);
		// 特殊ジャンプ時のエフェクトを生成
		Destroy (Instantiate (sp_JumpEffect, transform.position + Vector3.down, Quaternion.identity), 0.8f);
	}

	/// <summary>
	/// <para>関数名:　IsRunning</para>
	/// <para>機能　:　移動中かの判定を返す</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　bool isRunning</para>  
	/// <para>		   true:移動中 false:停止中</para>
	/// </summary>
	public bool IsRunning(){
		return isRunning;
	}

	/// <summary>
	/// <para>関数名:　GetDirection</para>
	/// <para>機能　:　移動量の正負を返す</para>
	/// <para>			戻り値の正負でキャラクターの向きを判定する</para>
	/// <para>			正:右向き　負:左向き</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　Vector3 velocity</para>
	/// </summary>
	public Vector3 GetDirection(){
		// ベクトルの向きだけが欲しいので、正規化します。
		return velocity.normalized;
	}

	/// <summary>
	/// <para>関数名:　IsGrounded</para>
	/// <para>機能　:　床に着地しているかの判定を返す</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　bool isGrounded</para>
	/// <para>		   true:着地している false:着地していない</para>
	/// </summary>
	public bool IsGrounded(){
		return isGrounded;
	}

	/// <summary>
	/// <para>関数名:　Raycast2D</para>
	/// <para>機能　:　指定された方向にレイキャストを使い当たり判定をとる</para>
	/// <para>引数　:　Vector2 v  レイキャストを行う方向</para>
	/// <para>戻り値:　RaycastHit2D Physics2D.Raycase() 当たり判定の情報</para>
	/// </summary>
	RaycastHit2D Raycast2D(Vector2 v){
		// 方向と距離を指定して、当たり判定を飛ばす
		return Physics2D.Raycast (transform.position, v, 1.15f, 1 << LayerMask.NameToLayer ("Ground"));
	}

	/// <summary>
	/// <para>関数名:　UpdatePosition</para>
	/// <para>機能　:　移動量を加算する事で、座標移動をする</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void UpdatePosition(){
		// 座標更新
		transform.position += velocity;

		// velocityの長さが0以外であれば移動している
		isRunning = !(velocity.magnitude == 0);
	}

	/// <summary>
	/// <para>関数名:　Falling</para>
	/// <para>機能　:　滞空中の時間ごとに落下をしていく</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Falling(){
		// 滞空中の時
		if (!jumping) {
			// 滞空時間を加算
			time += Time.deltaTime;
			// 落ちる速度を移動量に加える
			velocity.y -= (0.98f * time) / 2;
		}
	}
}
/// End of class