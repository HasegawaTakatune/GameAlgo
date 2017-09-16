using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeTextViewer : MonoBehaviour {

	Text text;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update () {
		text.text = Mathf.FloorToInt(gameManager.GetLimitTime ()).ToString ();
	}
}
