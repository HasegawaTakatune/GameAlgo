using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreator : MonoBehaviour {

	[SerializeField]GameObject Block;
	[SerializeField]GameObject BlockX;
	[SerializeField]GameObject Goal;

	const byte WALL=0,GROUND=1,GOAL=2;
	byte createType = WALL;
	const int RIGHT=0;

	public void CreateStage(){
		Vector2 cPos = new Vector2 (0, 2);
		int count = Random.Range (0, 6);
		bool create = true;
		int createNum = Random.Range (6, 9);
		bool direc = true;

		while (create) {

			switch (createType) {
			case WALL:
				Instantiate ((count % 6 == 0) ? BlockX : Block, cPos, Quaternion.Euler (new Vector3 (0, 0, -90)));
				Instantiate (((count + 3) % 6 == 0) ? BlockX : Block, cPos + new Vector2 (6, 0), Quaternion.Euler (new Vector3 (0, 0, 90)));
				count++;
				cPos.y += 2;
				break;

			case GROUND:
				Instantiate (Block, cPos, Quaternion.identity);
				cPos.x += (direc)? 2 : -2;
				break;

			case GOAL:
				Instantiate (Goal, cPos, Quaternion.identity);
				create = false;
				break;
			}

			createNum--;
			if (createNum <= 0) {
				if (cPos.y >= 100) {
					createType = GOAL;
				} else {
					byte type = (Random.Range (0, 2) == 0) ? WALL : GROUND;
					if (createType != type) {
						createType = type;
						if (createType == WALL) {
							cPos += new Vector2 ((direc) ? -6 : 0, 5);
						} else {
							direc = (Random.Range (0, 2) == RIGHT) ? true : false;
							cPos.x += (direc) ? 6 : 0;
						}
					}
					createNum = Random.Range (6, 15);
				}
			}

		}
	}
}
