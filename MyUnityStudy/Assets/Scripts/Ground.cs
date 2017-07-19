using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
	[SerializeField]
	bool isSpecial = false;
	public bool IsSpecial{ get { return isSpecial; } set { isSpecial = value; } }

	// Use this for initialization
	void Start () {
		//Destroy (this.gameObject, 50);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
