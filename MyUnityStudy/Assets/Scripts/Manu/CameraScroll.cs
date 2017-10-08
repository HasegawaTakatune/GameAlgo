using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>クラス名　:　CameraScroll</para>
/// <para>機能　　　:　ターゲットを中心としてカメラを移動する</para>
/// </summary>
public class CameraScroll : MonoBehaviour {

	// カメラ位置を合わせるターゲット
	[SerializeField]Transform _target;
	// ターゲットのゲッタ/セッタ
	public Transform Target{ get { return _target; } set { _target = value; } }

	/// <summary>
	/// <para>関数名:　Update</para>
	/// <para>機能　:　ターゲット座標を受け取り、カメラ位置を調節する</para>
	/// <para>引数　:　なし</para>
	/// <para>戻り値:　なし</para>
	/// </summary>
	void Update () {
		// ターゲットが設定されている時
		if (_target != null) {
			// ターゲット座標とカメラ座標の開き具合を見て、カメラ移動量を決定し移動する
			transform.position += new Vector3 ((_target.position.x - transform.position.x), (3 + _target.position.y - transform.position.y), 0) * .1f;
		}
	}
}
/// End of class