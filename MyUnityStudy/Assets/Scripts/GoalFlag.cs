using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour {
	[SerializeField]
	RockClimbManager manager;

	int playerCount;
	int GoalPlayerCount = 0;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<RockClimbManager> ();
		playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			Player player = other.GetComponent<Player> ();
			if (!player.Goal) {
				player.MyTime = (Mathf.Floor (manager.Timer * 100) / 100);
				player.Goal = true;
				GoalPlayerCount++;

				if (playerCount >= GoalPlayerCount)
					manager.GameEndMessage ();
			}
		}
	}
}
