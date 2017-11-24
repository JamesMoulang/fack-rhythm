using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonController : MonoBehaviour {
	private AudioEventManager audioManager;
	private GameObject face;
	private float faceStartY;
	public float FaceVelocity = 10f;
	private bool pressed = false;
	private Vector3 targetScale;
	private Vector3 startScale;
	public float PressedScale;
	public float LerpSpeed = 0.25f;

	// Use this for initialization
	void Start () {
		audioManager = GameObject.Find ("Music").GetComponent<AudioEventManager>();
		face = GameObject.Find ("Face");
		faceStartY = face.transform.position.y;
		face.transform.position += Vector3.up * 150f;
		startScale = transform.localScale;
		targetScale = transform.localScale * PressedScale;
	}

	void PlayButtonReleased() {
		Bob bob = face.GetComponent<Bob> ();
		bob.HeadTowards (faceStartY, FaceVelocity);
		bob.StartMusicOnReachDestination = true;
		Destroy (gameObject);
	}

	void PlayButtonDown() {
		pressed = true;
		// audioManager.StartMusic ();
	}

	void OnMouseDown() {
		PlayButtonDown ();
	}

	void OnMouseUp() {
		PlayButtonReleased ();
	}

	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.Lerp (transform.localScale, pressed ? targetScale : startScale, LerpSpeed);
	}
}
