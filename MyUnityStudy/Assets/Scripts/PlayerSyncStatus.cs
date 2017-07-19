using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSyncStatus : NetworkBehaviour {

	Player player;
	[SyncVar]
	float syncDirec;
	[SyncVar]
	byte syncAnimationType;

	[Command]
	void CmdProvideAnimationTypeToServer(float direc){
		syncDirec = direc;
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
