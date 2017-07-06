using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	[SerializeField]
	int score;

	bool isGotten;

	void OnTriggerEnter2D(Collider2D other){
		isGotten = true;
	}

	public bool IsGotten() {
		return isGotten;
	}

	public int GetScore() {
		return score;
	}
}
