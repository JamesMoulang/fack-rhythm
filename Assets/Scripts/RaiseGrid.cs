using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Grid))]
public class RaiseGrid : AudioEventListener {

	[System.Serializable]
	public class AmplificationEvent {
		public float StartTime;
		public float EndTime;
		public float GlitchMagnification = 1f;
		public float WaveHeightMagnification = 1f;
	}

	public List<AmplificationEvent> AmplificationEvents = new List<AmplificationEvent>();
	private int amplificationIndex = 0;

	Grid grid;
	private Vector3[] startVerts;
	private Vector3[] offsets;
	private Vector3[] moddedVerts;

	public float WaveSpeed;
	public float WaveThickness = 1f;
	public float WaveHeight = 2f;
	public float Glitchiness;

	public bool BackgroundWaves = false;
	public float BackgroundWaveFrequency;

	private float backgroundWaveTimer = 0f;

	private float innerRadius = 0f;
	private float outerRadius;
	private bool waving = false;

	public bool Secondary = false;
	private RaiseGrid otherWave;
	private MeshCollider meshCollider;
	private MeshFilter meshFilter;

	public float PlayTime = 0f;

	// Use this for initialization
	void Start () {
		meshCollider = GetComponent<MeshCollider> ();
		meshFilter = GetComponent<MeshFilter> ();
		grid = GetComponent<Grid> ();
		if (Secondary) {
			RaiseGrid[] otherwaves = GetComponents<RaiseGrid> ();
			for (int i = 0; i < otherwaves.Length; i++) {
				if (!otherwaves [i].Secondary) {
					otherWave = otherwaves [i];
				}
			}
			Debug.Log (otherWave.Secondary);
		}

		startVerts = grid.GetVertices ();
		offsets = new Vector3[startVerts.Length];
		for (int i = 0; i < offsets.Length; i++) {
			offsets [i] = new Vector3 (0f, 0f, 0f);
		}
		moddedVerts = new Vector3[startVerts.Length];
		Wave ();
	}

	public override void Trigger (string key) {
		Wave ();
	}

	public Vector3 GetCenter() {
		if (grid != null) {
			return grid.GetCenter ();
		} else {
			return new Vector3 ();
		}
	}

	public float GetInnerRadius() {
		return innerRadius;
	}

	public float GetOuterRadius() {
		return outerRadius;
	}

	void Wave() {
		waving = true;
		innerRadius = outerRadius = 0f;
	}

	void UpdateWave() {
		float heightMag = 1f;
		float glitchMag = 1f;
		if (amplificationIndex < AmplificationEvents.Count) {
			if (GetPlayTime () > AmplificationEvents [amplificationIndex].StartTime) {
				if (GetPlayTime () > AmplificationEvents [amplificationIndex].EndTime) {
					amplificationIndex++;
				} else {
					heightMag = AmplificationEvents [amplificationIndex].WaveHeightMagnification;
					glitchMag = AmplificationEvents [amplificationIndex].GlitchMagnification;
				}
			}
		}

		Vector3 center = grid.GetCenter ();
		float distanceToRing = innerRadius + WaveThickness * 0.5f;
		for (int i = 0; i < offsets.Length; i++) {
			Vector3 pos = new Vector3 (
				transform.position.x + startVerts[i].x * transform.localScale.x,
				0f,
				transform.position.z + startVerts[i].y * transform.localScale.y
			);
			float distanceToCenter = Vector3.Distance(pos, center);
			if (distanceToCenter > innerRadius && distanceToCenter < outerRadius) {
				float mag = Mathf.Clamp01 ((distanceToCenter - distanceToRing) / (WaveThickness * 0.5f));
				offsets [i].z = -WaveHeight * mag * heightMag;
				offsets [i].x = (Random.value - 0.5f) * mag * Glitchiness * glitchMag;
				offsets [i].y = (Random.value - 0.5f) * mag * Glitchiness * glitchMag;
			} else {
				offsets [i].z = 0f;
				offsets[i].x = 0f;
				offsets[i].y = 0f;
			}
		}
	}

	public Vector3 GetOffset(int index) {
		return offsets [index];
	}

	void UpdateModdedVerts() {
		for (int i = 0; i < moddedVerts.Length; i++) {
			moddedVerts [i] = startVerts [i] + offsets [i];
			if (Secondary && otherWave != null) {
				moddedVerts [i] += otherWave.GetOffset (i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		PlayTime = GetPlayTimeRatio ();
		if (BackgroundWaves) {
			backgroundWaveTimer += Time.deltaTime;
			float backgroundWaveTime = (1f / BackgroundWaveFrequency);
			if (backgroundWaveTimer >= backgroundWaveTime) {
				backgroundWaveTimer -= backgroundWaveTime;
				Wave ();
			}
		}
		if (waving) {
			innerRadius += WaveSpeed * Time.deltaTime;
			outerRadius = innerRadius + WaveThickness;
		} else {
			innerRadius = outerRadius = 0f;
		}
		UpdateWave ();
//		for (int i = 0; i < offsets.Length; i++) {
//			offsets [i].z -= Time.deltaTime;
//		}
		UpdateModdedVerts ();
		if (Secondary) {
			grid.UpdateVertices(moddedVerts);
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(RaiseGrid))]
public class raiseGridEditor: Editor 
{
	private RaiseGrid r;

	public void OnSceneGUI() {
		r = this.target as RaiseGrid;
		Handles.color = Color.red;
		Handles.DrawWireDisc (r.GetCenter(), Vector3.up, r.GetInnerRadius ());
		Handles.DrawWireDisc (r.GetCenter(), Vector3.up, r.GetOuterRadius ());
	}
}
#endif