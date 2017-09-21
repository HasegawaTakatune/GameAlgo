using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RockClimbManager : MonoBehaviour,Game_RecieveInterface {
	Text TimeCount;
	Text CountDown;
	Text TimeScore;

	public enum STATUS{Wait,CountDown,Play,End}
	STATUS status = STATUS.Wait;

	float timer = 0;
	public float Timer{get{ return timer;}}

	int GoalCount = 0;

	void Start () {
		TimeCount = GameObject.Find ("TimeCount").GetComponent<Text> ();
		CountDown = GameObject.Find ("CountDown").GetComponent<Text> ();
		TimeScore = GameObject.Find ("TimeScore").GetComponent<Text> ();
	}
	

	void Update () {
		if (status != STATUS.Wait)
			timer += Time.deltaTime;
		
		switch (status) {
		case STATUS.Wait:
			if (Input.GetKeyDown (KeyCode.Return)) {
				status = STATUS.CountDown;
			}
			break;

		case STATUS.CountDown:
			float t = 3 - Mathf.Floor (timer);
			CountDown.text = t.ToString ();
			if (t <= 0)
				StartCoroutine (StartMessage ());
			break;

		case STATUS.Play:
			TimeCount.text = (Mathf.Floor (timer * 100) / 100).ToString ();
			break;

		case STATUS.End:
			CountDown.text = "\"ENTER\"";
			if (Input.GetKeyDown (KeyCode.Return))
				SceneManager.LoadScene ("Menu");
			break;
		}
	}

	IEnumerator StartMessage(){
		timer = 0;
		status = STATUS.Play;
		CountDown.text = "Start";
		SendMessage ("CreateStage");
		yield return new WaitForSeconds (.5f);
		CountDown.text = "";
	}



	public void PlayerGoal(Player player){
		if (!player.Goal) {
			GoalCount++;
			TimeScore.text += GoalCount + "位 : " + player.Name + "Time : " + (Mathf.Floor (timer * 100) / 100) + "秒\n";
			player.Goal = true;
			status = STATUS.End;
		}
	}
}
