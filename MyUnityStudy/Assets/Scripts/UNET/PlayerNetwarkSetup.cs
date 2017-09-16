using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwarkSetup : NetworkBehaviour {

	[SerializeField]
	Camera camera;


	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<Player> ().enabled = true;
			camera.enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
