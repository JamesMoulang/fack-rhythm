using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {
	public float BobHeight = 1f;
	public float BobSpeed = 1f;
	public bool StartMusicOnReachDestination = false;
	public float Gravity = 100f;
	public float BounceAmount = 0.1f;
	public float MinimumDistY = 0.1f;
	public float MinimumSpeedY = 1f;

	private Vector3 targetPosition;
	private Vector3 startPosition;
	private Vector3 offset = new Vector3(0f, 0f, 0f);
	private float targetSpeed = 0f;
	private bool moving = false;
	private bool startedMusic = false;

	// Use this for initialization
	void Start () {
		startPosition = targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		offset.y = Mathf.Lerp(offset.y, Mathf.Sin (Time.time * BobSpeed) * BobHeight, 0.25f);

		if (moving) {
			targetSpeed -= Gravity * Time.deltaTime;
			float ny = startPosition.y + targetSpeed * Time.deltaTime;

			if (ny <= targetPosition.y) {
				targetSpeed *= -BounceAmount;
				if (!startedMusic && StartMusicOnReachDestination) {
					startedMusic = true;
					GameObject.Find ("Music").GetComponent<AudioEventManager> ().StartMusic ();
				}
			} else {
				startPosition = new Vector3 (
					startPosition.x,
					ny,
					startPosition.z
				);
			}

			if (Mathf.Abs (startPosition.y - targetPosition.y) <= MinimumDistY && Mathf.Abs (targetSpeed) < MinimumSpeedY) {
				moving = false;
				offset.y = 0f;
				startPosition = new Vector3 (
					startPosition.x,
					targetPosition.y,
					startPosition.z
				);
			}

			transform.position = startPosition;
		} else {
			transform.position = startPosition + offset;
		}
	}

	public void HeadTowards(float target, float speed) {
		moving = true;
		targetPosition = new Vector3 (
			transform.position.x,
			target,
			transform.position.z
		);
		targetSpeed = -speed;
	}
}
