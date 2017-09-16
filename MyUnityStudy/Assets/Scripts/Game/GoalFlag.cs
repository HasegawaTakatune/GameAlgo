using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour {
	RockClimbManager manager;

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<RockClimbManager> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			manager.Goal (other.GetComponent<Player> ());
		}
	}
}
