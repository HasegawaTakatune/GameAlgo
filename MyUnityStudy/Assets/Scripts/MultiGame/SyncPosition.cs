using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : Photon.MonoBehaviour {
	[SerializeField]PhotonView myPhotonView;

	void Awake () {
		myPhotonView = GetComponent<PhotonView> ();
	}
	
	void Update () {
		if (photonView.isMine) {
			myPhotonView.RPC ("syncPosition", PhotonTargets.Others, transform.position);
		}
	}

	[PunRPC]
	void syncPosition(Vector3 pos){
		transform.position = Vector3.Lerp (transform.position, pos, 0.5f);
	}

}
