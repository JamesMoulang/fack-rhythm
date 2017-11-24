using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RiseAndFade : AudioEventListener {
	public float Speed = 1f;
	public bool ShouldFade = false;
	public float TimeToFade = 4f;
	private float spawnTime;

	public float MinHeight = 1f;
	public float MaxHeight = 5f;
	public float HeightVariance = 0.25f;

	private float airVelocity = 0f;
	public float Gravity = 1f;
	public float BoostSpeed = 2f;

	public float MinBoostTime = 1f;
	public float MaxBoostTime = 4f;
	public float BoostTimeVariance = 0.25f;
	public float BoostSpeedVariance = 0.25f;

	private float lastBoostTime;
	private bool wasout = true;

	// Use this for initialization
	void Start () {
		recalculateBoostTime ();
		spawnTime = Time.time;
		MinHeight += HeightVariance * MinHeight * (Random.value - 0.5f);
		MaxHeight += HeightVariance * MaxHeight * (Random.value - 0.5f);
		transform.position = new Vector3 (
			transform.position.x,
			MinHeight + (MaxHeight - MinHeight) * Random.value,
			transform.position.z
		);
	}

	public override void Trigger(string key) {
		airVelocity += BoostSpeed * 2;
	}

	void recalculateBoostTime() {
		lastBoostTime = Time.time;
	}

	void Boost() {
		airVelocity += BoostSpeed;
		recalculateBoostTime ();
	}
	
	// Update is called once per frame
	void Update () {
		// 
		if (transform.position.y <= MaxHeight) {
			if (wasout) {
				wasout = false;
				lastBoostTime = Time.time;
			}
			float amount = (MaxHeight - transform.position.y) / (MaxHeight - MinHeight);
			float time = MaxBoostTime - (MaxBoostTime - MinBoostTime) * amount;
			if (Time.time >= lastBoostTime + time) {
				Boost ();
			}
		} else {
			wasout = true;
		}

		airVelocity -= Gravity * Time.deltaTime;
		transform.position += airVelocity * Vector3.up * Time.deltaTime;


		if (ShouldFade && Time.time >= spawnTime + TimeToFade) {
			Destroy (gameObject);
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(RiseAndFade))]
public class riseFadeEditor: Editor 
{
	private RiseAndFade g;

	public void OnSceneGUI() {
		g = this.target as RiseAndFade;
		Handles.color = Color.red;
		Handles.DrawWireCube (
			new Vector3(
				g.transform.position.x,
				g.MinHeight * 0.5f,
				g.transform.position.z
			),
			new Vector3(
				1f,
				g.MinHeight,
				1f
			)
		);

		Handles.color = Color.blue;
		Handles.DrawWireCube (
			new Vector3(
				g.transform.position.x,
				g.MinHeight + (g.MaxHeight - g.MinHeight) * 0.5f,
				g.transform.position.z
			),
			new Vector3(
				1f,
				g.MaxHeight - g.MinHeight,
				1f
			)
		);
	}
}
#endif