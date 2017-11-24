using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampToGround : MonoBehaviour {
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}

	void RayClamp() {
		float maxDistance = Mathf.Infinity;
		//Raycast for position.

		RaycastHit outDown;
		if (Physics.Raycast (
			transform.position + Vector3.up, 
			Vector3.up * -1, 
			out outDown, 
			maxDistance
		)
		) {
			transform.position = new Vector3(
				startPosition.x,
				outDown.point.y + startPosition.y,
				startPosition.z
			);
		}
	}
	
	// Update is called once per frame
	void Update () {
		RayClamp ();
	}
}
