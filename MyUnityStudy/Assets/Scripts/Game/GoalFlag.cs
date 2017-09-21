using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour {
	RockClimbManagerInterface manager;

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<RockClimbManagerInterface> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			manager.PlayerGoal_Interface (other.GetComponent<Player> ());
		}
	}
}