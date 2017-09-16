using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	[SerializeField]
	float interval;

	[SerializeField]
	GameObject coinPrefab;

	Object coin;

	// Use this for initialization
	void Start () {
		StartCoroutine (Exec ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Exec(){
		while (true) {
			if (coin == null) {
				Generate ();
			}
			yield return new WaitForSeconds (interval);
		}
	}


	void Generate(){
		coin = Instantiate (coinPrefab, transform.position, Quaternion.identity);
	}

}