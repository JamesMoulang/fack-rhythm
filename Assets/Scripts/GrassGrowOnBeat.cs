using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrassGrowOnBeat : AudioEventListener {
	public GameObject Grass;
	public float MinRadius = 0f;
	public float MaxRadius = 5f;
	public float MinRadiusIncrease = 0f;
	public float MaxRadiusIncrease = 0f;
	public int Amount = 5;
	public float yy = 0f;

	// Use this for initialization
	void Start () {
		
	}

	float CoinFlip() {
		return (Random.value < 0.5f) ? 1 : -1;
	}

	public override void Trigger (string key) {
		Debug.Log ("triggered");
		for (int i = 0; i < Amount; i++) {
			GameObject grass = GameObject.Instantiate (Grass);
			grass.transform.position = new Vector3 (
				transform.position.x + CoinFlip() * (MinRadius + Random.value * (MaxRadius - MinRadius)),
				yy,
				transform.position.z + CoinFlip() * (MinRadius + Random.value * (MaxRadius - MinRadius))
			);
			MinRadius += MinRadiusIncrease;
			MaxRadius += MaxRadiusIncrease;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(GrassGrowOnBeat))]
public class grassGrowEditor: Editor 
{
	private GrassGrowOnBeat g;

	public void OnSceneGUI() {
		g = this.target as GrassGrowOnBeat;
		Handles.color = Color.red;
		Handles.DrawWireDisc(
			new Vector3(
				g.transform.position.x,
				g.yy,
				g.transform.position.z
			),
			Vector3.up,
			g.MinRadius
		);

		Handles.color = Color.blue;
		Handles.DrawWireDisc(
			new Vector3(
				g.transform.position.x,
				g.yy,
				g.transform.position.z
			),
			Vector3.up,
			g.MaxRadius
		);
	}
}
#endif