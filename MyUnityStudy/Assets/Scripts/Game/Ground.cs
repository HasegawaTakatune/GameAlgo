using UnityEngine;
using System.Collections;

/// <summary>
/// <para>クラス名　:　Ground</para>
/// <para>機能　　　:　特殊壁の判定</para>
/// <para>壁キックをした際に、より速く上昇できる特殊壁を設定する</para>
/// </summary>
public class Ground : MonoBehaviour {
	// 特殊壁フラグ  true:特殊壁　false:通常壁
	[SerializeField]bool isSpecial = false;
	public bool IsSpecial{ get { return isSpecial; } set { isSpecial = value; } }
}
