using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField]
	int score;
	const int MAX_SCORE = 9999;
	[SerializeField]
	float limitTime;

	// Use this for initialization
	void Start () {
		score = 0;
		limitTime = 60;
	}
	
	// Update is called once per frame
	void Update () {
		Coin[] coins = FindObjectsOfType<Coin> ();

		foreach (Coin coin in coins) {
			if (coin.IsGotten ()) {
				AddScore (coin.GetScore ());
				Destroy (coin.gameObject);
			}
		}

		limitTime -= Time.deltaTime;
		if (limitTime < 0) {
			limitTime = 0;
		}
	}

	void AddScore(int value) {
		if(score + value >= MAX_SCORE) {
			score = MAX_SCORE;
		}
		else {
			score += value;
		}
	}

	public int GetScore() {
		return score;
	}

	public float GetLimitTime() {
		return limitTime;
	}
}
