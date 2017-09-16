using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSyncStatus : NetworkBehaviour {

	Player player;
	[SyncVar]
	float syncDirec;
	float myDirec;
	[SyncVar]
	byte syncAnimationType;
	byte myAnimationType;

	[Command]
	void CmdProvideAnimationTypeToServer(float direc,byte animType){
		syncDirec = direc;
		syncAnimationType = animType;
	}

	[ClientCallback]
	void TransmitAnimationType(){
		if (isLocalPlayer) {
			//CmdProvideAnimationTypeToServer()
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
