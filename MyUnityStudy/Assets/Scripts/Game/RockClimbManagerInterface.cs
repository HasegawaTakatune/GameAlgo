using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RockClimbManagerInterface : MonoBehaviour {

	public void PlayerGoal_Interface(Player player){
		ExecuteEvents.Execute<Game_RecieveInterface> (
			target: gameObject, 
			eventData: null, 
			functor: (reciever, eventData) => reciever.PlayerGoal (player));
	}
}
