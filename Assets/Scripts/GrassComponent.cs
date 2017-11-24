using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassComponent : MonoBehaviour {
	private bool lerping = true;
	private Vector3 targetScale;
	private float anchorY;

	// Use this for initialization
	void Start () {
		anchorY = transform.position.y;
		targetScale = transform.localScale;
		transform.localScale = new Vector3 (
			transform.localScale.x,
			0f,
			transform.localScale.z
		);
	}

	public void Skip() {
		lerping = false;
		transform.localScale = targetScale;
		transform.position = new Vector3 (
			transform.position.x,
			anchorY,
			transform.position.z
		);
	}
	
	// Update is called once per frame
	void Update () {
		if (lerping) {
			transform.localScale = Vector3.Lerp (
				transform.localScale,
				targetScale,
				0.25f
			);
			if (Mathf.Abs (transform.localScale.y - targetScale.y) < 0.01f) {
				transform.localScale = targetScale;
				lerping = false;
			}

			transform.position = new Vector3 (
				transform.position.x,
				anchorY + transform.localScale.y * 0.5f,
				transform.position.z
			);
		}
	}
}
