using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {
	public GameObject Cube;
	public Vector3 Padding;
	public Vector3 Margin;
	public int CubeCount;
	public Vector3 MiddleSize;
	public Vector3 SizeRange;

	// Use this for initialization
	void Start () {
		Camera main = Camera.main;
		for (int i = 0; i < CubeCount; i++) {
			Vector3 pos = new Vector3 (
				Padding.x + Random.value * Margin.x,
				Padding.y + Random.value * Margin.y,
				Padding.z + Random.value * Margin.z
			);
			pos.x *= -1f;
			if (Random.value < 0.5f) { 
				pos.z *= -1f;
			}

			if (Random.value < 0.5f) {
				pos.y *= -1f;
			}
			Vector3 size = new Vector3 (
				MiddleSize.x + (Random.value - 0.5f) * SizeRange.x,
				MiddleSize.y + (Random.value - 0.5f) * SizeRange.y,
				MiddleSize.z + (Random.value - 0.5f) * SizeRange.z
			);
			GameObject cube = GameObject.Instantiate (Cube);
			cube.transform.position = transform.position + pos;
			cube.transform.localScale = size;
			cube.transform.parent = main.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
