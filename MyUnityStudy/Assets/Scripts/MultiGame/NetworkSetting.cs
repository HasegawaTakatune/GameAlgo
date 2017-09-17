using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSetting : Photon.MonoBehaviour {

	[SerializeField]Player player;
	[SerializeField]PlayerAnimationView playerAnimationView;
	[SerializeField]BoxCollider2D boxcollider2d;

	void Start () {
		if (photonView.isMine) {
			GameObject.Find ("Camera").GetComponent<CameraScroll> ().Target = transform;
			player.enabled = true;
			playerAnimationView.enabled = true;
			boxcollider2d.enabled = true;
		}
	}
	
}