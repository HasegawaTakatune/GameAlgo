using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
	[SerializeField]	bool isSpecial = false;
	public bool IsSpecial{ get { return isSpecial; } set { isSpecial = value; } }
}
