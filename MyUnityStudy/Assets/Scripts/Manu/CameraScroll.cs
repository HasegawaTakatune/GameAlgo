using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {

	[SerializeField]Transform _target;
	public Transform Target{ get { return _target; } set { _target = value; } }

	void Update () {
		if (_target != null)
			transform.position += new Vector3 ((_target.position.x - transform.position.x), (3 + _target.position.y - transform.position.y), 0) * .1f;
	}
}
