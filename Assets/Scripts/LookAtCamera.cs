using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
	private Camera camera;
	public bool StayUpright = true;

	// Use this for initialization
	void Start () {
		camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lookPoint = transform.position - camera.transform.position;
		transform.LookAt(lookPoint);

		if (StayUpright) {
			transform.LookAt (lookPoint + Vector3.up * camera.transform.position.y);
		} else {
			transform.LookAt (lookPoint);
		}
	}
}
