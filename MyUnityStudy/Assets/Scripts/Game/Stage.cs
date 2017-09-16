using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	[SerializeField]
	protected float WIDTH;
	public float width{ get { return WIDTH; } set { WIDTH = value; } }
	[SerializeField]
	protected float HEIGHT;
	public float height{ get { return HEIGHT; } set { HEIGHT = value; } }

	public Vector2 vector2 {
		get{ return new Vector2 (WIDTH, HEIGHT); }
		set {
			WIDTH = value.x;
			HEIGHT = value.y;
		}
	}
}
