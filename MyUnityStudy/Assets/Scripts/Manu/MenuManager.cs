using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	[SerializeField]SpriteRenderer TitleLogo;
	[SerializeField]SpriteRenderer BackGround;
	static bool showTitle = true;

	void Awake(){
		if (!showTitle) {
			TitleLogo.sortingOrder = -3;
			BackGround.sortingOrder = -2;
		}
			
	}
	void Start () {
		
	}

	void Update () {
		if (showTitle && Input.anyKeyDown) {
			TitleLogo.sortingOrder = -3;
			BackGround.sortingOrder = -2;
			showTitle = false;
		}
	}
}
