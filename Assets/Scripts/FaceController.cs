using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : AudioEventListener {
	public Material RightEyeOpen;
	public Material RightEyeClosed;
	public Material LeftEyeOpen;
	public Material LeftEyeClosed;
	public float MouthMoveDistance;
	public float MouthLerp = 0.25f;

	private GameObject nose;
	private GameObject rightEyebrow;
	private GameObject leftEyeBrow;
	private GameObject leftEye;
	private GameObject rightEye;
	private GameObject mouthTop;
	private GameObject mouthBottom;

	private Vector3 mouthTopAnchor;
	private Vector3 mouthBottomAnchor;

	// Use this for initialization
	void Start () {
		nose = transform.Find ("Nose").gameObject;
		rightEyebrow = transform.Find ("Right Eyebrow").gameObject;
		leftEyeBrow = transform.Find ("Left Eyebrow").gameObject;
		leftEye = transform.Find ("Left Eye").gameObject;
		rightEye = transform.Find ("Right Eye").gameObject;
		mouthTop = transform.Find ("Mouth Top").gameObject;
		mouthBottom = transform.Find ("Mouth Bottom").gameObject;

		mouthTopAnchor = mouthTop.transform.localPosition;
		mouthBottomAnchor = mouthBottom.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		mouthTop.transform.localPosition = Vector3.Lerp (mouthTop.transform.localPosition, mouthTopAnchor, MouthLerp);
		mouthBottom.transform.localPosition = Vector3.Lerp (mouthBottom.transform.localPosition, mouthBottomAnchor, MouthLerp);
	}

	void Speak() {
		mouthTop.transform.localPosition -= Vector3.up * MouthMoveDistance;
		mouthBottom.transform.localPosition += Vector3.up * MouthMoveDistance;
	}

	public override void Trigger (string key) {
		if (key == "Voc") {
			Speak ();
		}
	}
}
