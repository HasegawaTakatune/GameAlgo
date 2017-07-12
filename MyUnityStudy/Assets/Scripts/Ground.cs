using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
	[SerializeField]
	bool IsSpecial = false;
	[SerializeField]
	float ADD_SPEED = 0.55f;
	public float addSpeed{get{ return ADD_SPEED;}set{ ADD_SPEED = value;}}

	// Use this for initialization
	void Start () {
		if (!IsSpecial)
			ADD_SPEED = 0.5f;
		//Destroy (this.gameObject, 50);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
