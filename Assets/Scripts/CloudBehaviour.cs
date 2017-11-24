using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : AudioEventListener {
	public float StartHeight;
	public float HeightVariance;
	public float EndHeight;
	public float StageX;
	public float StageZ;
	public float MoveSpeed;
	public float LerpSpeed;
	public GameObject Rain;
	public float TimeToRain = 0.5f;
	public float RainFPS = 12f;
	public List<Material> RainMaterials;
	public List<GameObject> SmallerClouds;
	public int ChildCloudCount = 3;
	public float RotationSpeed;

	private MeshRenderer rainMesh;
	private Vector3 targetScale;
	private Vector3 endPosition;
	private float lastRainFrame;
	private bool raining = false;
	private int rainFrameIndex = 0;
	private float rainStartTime;
	private List<GameObject> ChildClouds = new List<GameObject> ();
	private Vector3 spinAnchor;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().material.color = new Color (1f, 1f, 1f, 0.5f);
		rainMesh = Rain.GetComponent<MeshRenderer> ();
		rainMesh.enabled = false;
		transform.position = new Vector3 (
			(Random.value - 0.5f) * StageX,
			StartHeight + (Random.value - 0.5f) * HeightVariance,
			(Random.value - 0.5f) * StageZ
		);

		endPosition = new Vector3 (
			(Random.value - 0.5f) * StageX,
			EndHeight + (Random.value - 0.5f) * HeightVariance,
			(Random.value - 0.5f) * StageZ
		);

		spinAnchor = new Vector3 (
			0f,
			endPosition.y,
			0f
		);

		targetScale = transform.localScale;
		transform.localScale = Vector3.zero;
		SetRainMaterial ();

		for (int i = 0; i < ChildCloudCount; i++) {
			// Create a child cloud.
			int index = Mathf.FloorToInt(Random.value * SmallerClouds.Count);
			GameObject cloud = GameObject.Instantiate (SmallerClouds [index]);
			cloud.transform.parent = transform;
			cloud.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			cloud.transform.localPosition = new Vector3 (
				Random.value - 0.5f,
				0.5f,
				Random.value < 0.5f ? -0.1f : 0.1f
			);
		}
	}

	public override void Trigger (string key) {
		rainStartTime = Time.time;
		lastRainFrame = Time.time;
		rainMesh.enabled = true;
		raining = true;
		rainFrameIndex = Mathf.FloorToInt(Random.value * RainMaterials.Count);
	}

	void SetRainMaterial() {
		rainMesh.material = RainMaterials [rainFrameIndex];
	}
	
	// Update is called once per frame
	void Update () {
		if (raining) {
			if (Time.time >= rainStartTime + TimeToRain) {
				raining = false;
				rainMesh.enabled = false;
			} else if (Time.time >= lastRainFrame + (1f / RainFPS)) {
				lastRainFrame = Time.time;
				rainFrameIndex++;
				if (rainFrameIndex >= RainMaterials.Count) {
					rainFrameIndex = 0;
				}
				SetRainMaterial ();
			}
		}

		transform.localScale = Vector3.Lerp (
			transform.localScale,
			targetScale,
			0.25f
		);

		endPosition = Quaternion.Euler (0f, Time.deltaTime * RotationSpeed, 0f) * endPosition;

		transform.position = Vector3.MoveTowards (
			transform.position,
			endPosition,
			MoveSpeed
		);

		transform.position = Vector3.Lerp (
			transform.position,
			endPosition,
			LerpSpeed
		);
	}
}
