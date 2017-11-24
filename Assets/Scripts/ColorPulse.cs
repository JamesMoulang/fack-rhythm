using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPulse : AudioEventListener {
	public Color StartColor;
	public Color[] PulseColors;
	public float Speed = 0.25f;

	private int colorIndex = -1;
	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		mainCamera.backgroundColor = StartColor;
	}

	public override void Trigger (string key) {
		colorIndex++;
		if (colorIndex >= PulseColors.Length) {
			colorIndex = 0;
		}
		mainCamera.backgroundColor = PulseColors[colorIndex];
	}
	
	// Update is called once per frame
	void Update () {
		mainCamera.backgroundColor = Color.Lerp (
			mainCamera.backgroundColor,
			StartColor,
			Speed
		);
	}
}
