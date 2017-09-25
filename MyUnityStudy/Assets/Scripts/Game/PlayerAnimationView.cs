using UnityEngine;
using System.Collections;

public class PlayerAnimationView : MonoBehaviour {

	[SerializeField]Player player;
	[SerializeField]Animator animator;
	[SerializeField]SpriteRenderer spriteRenderer;

	void Start () {
		player = GetComponent<Player>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}
	
	void Update () {

		if (player.GetDirection ().x > 0) {
			// 右向き
			spriteRenderer.flipX = false;
		} else if (player.GetDirection ().x < 0) {
			// 左向き
			spriteRenderer.flipX = true;
		}
			
		if (!player.IsGrounded ()) {
			animator.Play ("PlayerJump");
		} else if (player.IsRunning ()) {
			animator.Play ("PlayerRun");
		} else {
			animator.Play ("PlayerIdle");
		}
	}
}
