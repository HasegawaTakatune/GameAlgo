using UnityEngine;
using System.Collections;

public class PlayerAnimationView : MonoBehaviour {

	Player player;
	Animator animator;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
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
