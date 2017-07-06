using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const byte None = 0,R_Jump = 1,L_Jump = 2;
	// 移動ベクトル
	Vector3 velocity;

	// 移動速度
	[SerializeField]
	float speed = 1.5f;
	float jumpSpeed;
	float time;

	[SerializeField]
	RaycastHit2D isGrounded;
	bool hitRight;
	bool hitLeft;
	[SerializeField]
	bool isRunning;
	Vector3 groundPos;
	// ジャンプ制御
	bool jumping = false;
	byte jumpType = None;
	// 着地
	bool landing = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// 移動
		Run ();
		// 足元の確認
		CheckGround ();
		// ジャンプ
		Jump();
		// 移動
		Move ();
	}

	void Run(){
		// ベクトルを０に初期化
		velocity = Vector3.zero;
		// 一番下の座標を取る
		if (isGrounded) {
			groundPos = isGrounded.point;
			if (!landing && transform.position.y - 2.6f <= groundPos.y) {
				transform.position = new Vector3 (transform.position.x, isGrounded.point.y + 1.5f, 0);
				landing = true;
			}
		} else {
			groundPos = Vector3.down * 100;
			Falling ();
			landing = false;
		}

		// キー入力によりベクトルを加算
		if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && !hitRight) {
			velocity.x += speed;
		}
		if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && !hitLeft) {
			velocity.x += -speed;
		}
		// 高速移動
		if (Input.GetKey (KeyCode.LeftShift) && isGrounded) {
			velocity = velocity * 4;
		}
		// 急降下
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (transform.position.y - 2.6f >= groundPos.y) {
				velocity.y -= speed * 4;
			}
		}

	}

	void Jump(){
		//スペースキーでジャンプ
		if (Input.GetKeyDown (KeyCode.Space)) {
			//地面についているときだけジャンプ
			if (isGrounded || hitRight || hitLeft) {
				jumping = true;
				jumpSpeed = 0.5f;
				time = 0;
				if (!isGrounded)
					jumpType = (hitRight == true) ? R_Jump : (hitLeft == true) ? L_Jump : None;
			}
		}
		if (jumping) {
			// 壁キック処理
			switch(jumpType){
			case R_Jump:velocity.x -= speed * 1.5f;break;
			case L_Jump:velocity.x += speed * 1.5f;break;
			}
			// ジャンプの移動量分加算
			velocity.y += jumpSpeed;
			// ジャンプ中の時間を加算
			time += Time.deltaTime;
			// ジャンプの減速量の計算
			jumpSpeed -= (9.8f * (time * time)) / 2;
			if (jumpSpeed <= 0) {
				jumping = false;
				time = 0;
			}
		}
	}

	public bool IsRunning(){
		return isRunning;
	}

	public Vector3 GetDirection(){
		// ベクトルの向きだけが欲しいので、正規化します。
		return velocity.normalized;
	}

	public bool IsGrounded(){
		return isGrounded;
	}

	void Move(){
		// 座標更新
		transform.position += velocity;

		// velocityの長さが0以外であれば移動している
		isRunning = !(velocity.magnitude == 0);
	}

	void Falling(){
		// 落下
		if (!jumping) {
			time += Time.deltaTime;
			velocity.y -= (0.98f * (time * time)) / 2;
		}
	}

	void CheckGround(){
		// 地面についているかを算出
		isGrounded = Physics2D.Raycast (transform.position, Vector2.down,1.6f, 1 << LayerMask.NameToLayer ("Ground"));
		hitRight = Physics2D.Raycast (transform.position, Vector2.right, 0.8f, 1 << LayerMask.NameToLayer ("Ground"));
		hitLeft = Physics2D.Raycast (transform.position, Vector2.left, 1.0f, 1 << LayerMask.NameToLayer ("Ground"));
	}
}