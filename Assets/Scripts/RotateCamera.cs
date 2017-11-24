using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : AudioEventListener {
	public GameObject LookAt;
	public float DistanceFromTarget;
	public float Height;
	public float Zoom = 1f;
	public float Rotation = 0f;
	public Vector3 Offset;
	public Vector3 shakeOffset = new Vector3(0f, 0f, 0f);
	private Vector3 shakeVelocity = new Vector3 (0f, 0f, 0f);
	public float ShakeTime = 0.6f;
	public float ShakeFrequency = 5f;
	public float ShakeMagnitude = 1f;
	public float ShakeFriction = 0.25f;
	public float MinSpring = 1f;
	public float MaxSpring = 5f;
	public float MaxDist = 2f;
	public float MaxRotationVelocity = 10f;
	public float RotationKeyboardAcceleration = 1f;

	public float RotationFriction = 1f;
	private float rotationVelocity = 0f;
	public float RotateSpeed = 0.01f;

	private bool shaking = false;
	private float shakeStartTime;
	private float lastShakeTime;

	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		mainCamera.transparencySortMode = TransparencySortMode.Orthographic;
	}

	public override void Trigger (string key) {
		shaking = true;
		shakeStartTime = Time.time;
		lastShakeTime = -1f;
	}

	void UpdateShake() {
		if (shaking) {
			if (Time.time > lastShakeTime + (1f / ShakeFrequency)) {
				shakeVelocity += new Vector3 (
					Random.value - 0.5f,
					Random.value - 0.5f,
					Random.value - 0.5f
				).normalized * ShakeMagnitude;
				lastShakeTime = Time.time;
			}

			if (Time.time > shakeStartTime + ShakeTime) {
				shaking = false;
			}
		}

		Vector3 toCenter = -shakeOffset;
		float dist = toCenter.magnitude;
		float spring = MinSpring + (MaxSpring - MinSpring) * (dist / MaxDist);
		shakeVelocity += toCenter.normalized * Time.deltaTime * spring;
		shakeVelocity = Vector3.MoveTowards (shakeVelocity, Vector3.zero, ShakeFriction);
		shakeOffset += shakeVelocity * Time.deltaTime;
	}

	private bool storedLast = false;
	private Vector2 lastTouchPosition;
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			if (storedLast) {
				rotationVelocity = (touch.position.x - lastTouchPosition.x) / Time.deltaTime;
				rotationVelocity *= RotateSpeed;
			} else {
				storedLast = true;
			}
			lastTouchPosition = touch.position;
		} else {
			rotationVelocity = Mathf.MoveTowards (
				rotationVelocity,
				0,
				RotationFriction * Time.deltaTime * Mathf.Abs(rotationVelocity)
			);
			if (storedLast) {
				storedLast = false;
			}
		}

		if (Input.GetAxis ("Horizontal") > 0f) {
			rotationVelocity += RotationKeyboardAcceleration;
		} else if (Input.GetAxis ("Horizontal") < 0f) {
			rotationVelocity -= RotationKeyboardAcceleration;
		}

		if (Mathf.Abs (rotationVelocity) > MaxRotationVelocity) {
			rotationVelocity = (rotationVelocity > 0f) ? MaxRotationVelocity : -MaxRotationVelocity;
		}

		Rotation += rotationVelocity * Time.deltaTime;
		if (Rotation > 360f) {
			Rotation = Rotation - 360f;
		} else if (Rotation < 0f) {
			Rotation = 360 + Rotation;
		}

		transform.position = LookAt.transform.position;
		Vector3 direction = Vector3.forward;
		direction = Quaternion.AngleAxis(Rotation, Vector3.up) * direction;

		transform.position += direction * DistanceFromTarget;
		transform.position += Vector3.up * Height;

		transform.LookAt (LookAt.transform.position);
		UpdateShake ();
		transform.position += Offset;
		transform.position += transform.up * shakeOffset.y;
		transform.position += transform.right * shakeOffset.x;
	}
}
