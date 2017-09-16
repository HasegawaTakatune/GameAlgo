using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenu : MonoBehaviour {

	// ゲームタイプ
	public enum GameType{Single,Multi}
	public GameType gameType;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player")
			SceneManager.LoadScene ((gameType == GameType.Single) ? "SingleGame" : "MultiGame");
	}
}
