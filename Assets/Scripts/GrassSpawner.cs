using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour {

	public GameObject Grass;
	public Vector3 Center;
	public float Width;
	public float Height;
	public int GrassCount = 10;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < GrassCount; i++) {
			GameObject grass = GameObject.Instantiate (Grass);
			grass.transform.position = new Vector3 (
				Center.x + (Random.value - 0.5f) * Width,
				Center.y,
				Center.z + (Random.value - 0.5f) * Height
			);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
