using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour {
	private float offset = 0f;
	private float offsetVelocity = 0f;
	private int offsetDirection = -1;
	private MeshFilter meshFilter;
	private Vector3 topRightVert;
	private Vector3 topLeftVert;

	public float OffsetBounds = 1f;
	public float OffsetAcceleration = 1f;

	// Use this for initialization
	void Start () {
		meshFilter = GetComponent<MeshFilter> ();
		Vector3[] verts = meshFilter.mesh.vertices;
		offset = (Random.value - 0.5f) * OffsetBounds * 2f;

		topRightVert = new Vector3 (
			verts[1].x,
			verts[1].y,
			verts[1].z
		);

		topLeftVert = new Vector3 (
			verts[3].x,
			verts[3].y,
			verts[3].z
		);

		// 1 is top right
		// 3 is top left.
		// that's weird but ok
		verts[1].x -= 0.5f;
		verts [3].x -= 0.5f;

	}
	
	// Update is called once per frame
	void Update () {
		if (offsetDirection == -1) {
			offset -= OffsetAcceleration * Time.deltaTime;
			if (offset <= -OffsetBounds) {
				offsetDirection = 1;
			}
		} else {
			offset += OffsetAcceleration * Time.deltaTime;
			if (offset >= OffsetBounds) {
				offsetDirection = -1;
			}
		}

		Vector3[] verts = meshFilter.mesh.vertices;
		verts [1] = topRightVert + Vector3.right * offset;
		verts [3] = topLeftVert + Vector3.right * offset;
		meshFilter.mesh.vertices = verts;
	}
}
