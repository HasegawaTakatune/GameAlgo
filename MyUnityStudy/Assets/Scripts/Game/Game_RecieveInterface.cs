using UnityEngine.EventSystems;

public interface Game_RecieveInterface : IEventSystemHandler {
	void PlayerGoal(Player player);
}